using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Farmer Account Creation — requires genuine email OTP verification before saving.
    public partial class RegisterFarmer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // Send genuine OTP to the entered email address via SMTP
        protected void btnSendRegOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailVerify");
            if (!Page.IsValid) return;

            string email = txtRegEmail.Text.Trim().ToLower();

            // Check if email is already registered
            var dtCheck = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = @Email",
                new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                ShowError("This email address is already registered. Please use a different email or sign in.");
                return;
            }

            string fullName = (txtFullName.Text ?? "").Trim();
            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(email, "REGISTER", Request.UserHostAddress, fullName);

            if (result.Success)
            {
                pnlOtpVerify.Visible     = true;
                pnlEmailVerified.Visible = false;
                hfEmailVerified.Value    = "0";
                lblOtpInfo.Text = string.Format(
                    "<div class='alert alert-success py-2 px-3 mb-2 small fw-bold'>" +
                    "<i class='bi bi-envelope-check-fill me-1 text-success'></i> Verification OTP has been dispatched to {0}! " +
                    "Please check your email inbox (and spam folder) and enter the 6-digit code below." +
                    "</div>",
                    EmailOtpService.MaskEmail(email));
                txtRegOtp.Text = string.Empty;
                txtRegOtp.Focus();
                lblRegMsg.Text = string.Empty;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        // Verify OTP entered by the user from their email (inline flow)
        protected void btnVerifyRegOtp_Click(object sender, EventArgs e)
        {
            string email  = txtRegEmail.Text.Trim().ToLower();
            string otp    = txtRegOtp.Text.Trim();

            if (string.IsNullOrEmpty(otp))
            {
                ShowError("Please enter the 6-digit OTP sent to your email.");
                return;
            }

            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "REGISTER");

            if (result.Success)
            {
                hfEmailVerified.Value    = "1";
                pnlOtpVerify.Visible     = false;
                pnlEmailVerified.Visible = true;
                lblRegMsg.Text = string.Empty;

                // Lock email field so user cannot change it after verification
                txtRegEmail.ReadOnly = true;
                btnSendRegOtp.Enabled = false;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        // Final form submission — validates details and ensures email OTP verification
        protected void btnSubmitFarmer_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName        = txtFullName.Text.Trim();
            string mobile          = txtMobile.Text.Trim();
            string crop            = txtCrop.Text.Trim();
            string area            = txtArea.Text.Trim();
            string email           = txtRegEmail.Text.Trim().ToLower();
            string password        = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)        { ShowError("Please enter your full name."); return; }
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)          { ShowError("Please enter a valid 10-digit mobile number."); return; }
            if (string.IsNullOrWhiteSpace(crop))                                   { ShowError("Please enter your primary crop."); return; }
            if (string.IsNullOrWhiteSpace(area))                                   { ShowError("Please specify your farm area."); return; }
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))          { ShowError("Please enter a valid email address."); return; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)        { ShowError("Password must be at least 6 characters."); return; }
            if (password != confirmPassword)                                       { ShowError("Passwords do not match."); return; }

            // Check if mobile or email is already registered
            var dtDup = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE MobileNo = @Mob OR LOWER(Email) = LOWER(@Email)",
                new SqlParameter[]
                {
                    new SqlParameter("@Mob", mobile),
                    new SqlParameter("@Email", email)
                });

            if (dtDup != null && dtDup.Rows.Count > 0)
            {
                ShowError("This mobile number or email address is already registered. Please sign in or use another.");
                return;
            }

            // If user has not yet verified their email inline, dispatch OTP and redirect to dedicated VerifyOtp page
            if (hfEmailVerified.Value != "1")
            {
                OtpRequestResult otpRes = EmailOtpService.GenerateAndSendEmailOtp(email, "REGISTER", Request.UserHostAddress, fullName);
                if (!otpRes.Success)
                {
                    ShowError(otpRes.Message);
                    return;
                }

                PendingRegistration pending = new PendingRegistration
                {
                    Role          = "Farmer",
                    FullName      = fullName,
                    MobileNo      = mobile,
                    Email         = email,
                    Location      = "Gujarat",
                    SpecificField = crop + " (" + area + " Acres)",
                    PasswordHash  = SecurityHelper.HashPassword(password)
                };
                Session["PendingRegistration"] = pending;

                Response.Redirect("VerifyOtp.aspx?purpose=REGISTER&email=" + Server.UrlEncode(email) + "&role=Farmer");
                return;
            }

            // If already verified inline, save directly to database
            try
            {
                string passHash = SecurityHelper.HashPassword(password);
                string query = @"INSERT INTO Users 
                    (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, PasswordHash, CreatedDate) 
                    VALUES (@Name, @Mobile, @Email, 'Farmer', 'Gujarat', @Crop, 'Verified', @PassHash, GETDATE())";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name",     fullName),
                    new SqlParameter("@Mobile",   mobile),
                    new SqlParameter("@Email",    email),
                    new SqlParameter("@Crop",     crop + " (" + area + " Acres)"),
                    new SqlParameter("@PassHash", passHash)
                };
                DbHelper.ExecuteNonQuery(query, prms);

                // Fetch new user id
                object newId = DbHelper.ExecuteScalar("SELECT TOP 1 UserId FROM Users WHERE Email = @Email ORDER BY UserId DESC",
                    new SqlParameter[] { new SqlParameter("@Email", email) });
                if (newId != null)
                {
                    Session["UserId"] = Convert.ToInt32(newId);
                }

                Session["UserRole"]   = "Farmer";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                lblRegMsg.Text     = "Farmer account created and verified successfully! Redirecting to your dashboard...";
                lblRegMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=FarmerDashboard.aspx");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE") || ex.Message.Contains("duplicate"))
                    ShowError("This mobile number or email is already registered. Please sign in.");
                else
                    ShowError("Registration failed: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblRegMsg.Text     = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + msg;
            lblRegMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
