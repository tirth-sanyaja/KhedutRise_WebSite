using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Buyer / Corporate Account Creation — requires genuine email OTP verification before saving.
    public partial class RegisterBuyer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // Send genuine OTP to buyer email (inline flow)
        protected void btnSendBuyerOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailVerify");
            if (!Page.IsValid) return;

            string email = (txtEmail.Text ?? "").Trim().ToLower();

            var dtCheck = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = @Email",
                new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                ShowError("This email address is already registered. Please sign in or use another email.");
                return;
            }

            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(email, "REGISTER", Request.UserHostAddress);

            if (result.Success)
            {
                pnlBuyerOtp.Visible           = true;
                pnlBuyerEmailVerified.Visible = false;
                hfBuyerEmailVerified.Value    = "0";
                lblBuyerOtpInfo.Text = string.Format(
                    "<div class='alert alert-success py-2 px-3 mb-2 small fw-bold'>" +
                    "<i class='bi bi-envelope-check-fill me-1 text-success'></i> Verification OTP has been dispatched to {0}! " +
                    "Please check your email inbox (and spam folder) and enter the 6-digit code below." +
                    "</div>",
                    EmailOtpService.MaskEmail(email));
                txtBuyerOtp.Text = string.Empty;
                txtBuyerOtp.Focus();
                lblBuyerMsg.Text = string.Empty;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        // Verify OTP (inline flow)
        protected void btnVerifyBuyerOtp_Click(object sender, EventArgs e)
        {
            string email = (txtEmail.Text ?? "").Trim().ToLower();
            string otp   = (txtBuyerOtp.Text ?? "").Trim();

            if (string.IsNullOrEmpty(otp)) { ShowError("Please enter the 6-digit OTP sent to your email."); return; }

            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "REGISTER");

            if (result.Success)
            {
                hfBuyerEmailVerified.Value    = "1";
                pnlBuyerOtp.Visible           = false;
                pnlBuyerEmailVerified.Visible = true;
                lblBuyerMsg.Text              = string.Empty;
                txtEmail.ReadOnly             = true;
                btnSendBuyerOtp.Enabled       = false;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        protected void btnSubmitBuyer_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName        = txtFullName.Text.Trim();
            string mobile          = txtMobile.Text.Trim();
            string email           = txtEmail.Text.Trim().ToLower();
            string bizType         = ddlBizType.SelectedValue;
            string interests       = txtInterests.Text.Trim();
            string gstin           = txtGstin.Text.Trim();
            string password        = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)   { ShowError("Please enter full business or personal name."); return; }
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)     { ShowError("Please enter a valid 10-digit mobile number."); return; }
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))     { ShowError("Please enter a valid email address."); return; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)   { ShowError("Password must be at least 6 characters."); return; }
            if (password != confirmPassword)                                   { ShowError("Passwords do not match."); return; }

            var dtMob = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE MobileNo = @Mob OR LOWER(Email) = LOWER(@Email)",
                new SqlParameter[]
                {
                    new SqlParameter("@Mob", mobile),
                    new SqlParameter("@Email", email)
                });
            if (dtMob != null && dtMob.Rows.Count > 0)
            {
                ShowError("This mobile number or email is already registered. Please sign in.");
                return;
            }

            string specificField = bizType + " | GSTIN: " + (string.IsNullOrWhiteSpace(gstin) ? "N/A" : gstin) + " | Crops: " + interests;

            // If not verified inline yet -> dispatch OTP and redirect to VerifyOtp.aspx
            if (hfBuyerEmailVerified.Value != "1")
            {
                OtpRequestResult otpRes = EmailOtpService.GenerateAndSendEmailOtp(email, "REGISTER", Request.UserHostAddress);
                if (!otpRes.Success)
                {
                    ShowError(otpRes.Message);
                    return;
                }

                PendingRegistration pending = new PendingRegistration
                {
                    Role          = "Buyer",
                    FullName      = fullName,
                    MobileNo      = mobile,
                    Email         = email,
                    Location      = "Gujarat",
                    SpecificField = specificField,
                    PasswordHash  = SecurityHelper.HashPassword(password)
                };
                Session["PendingRegistration"] = pending;

                Response.Redirect("VerifyOtp.aspx?purpose=REGISTER&email=" + Server.UrlEncode(email) + "&role=Buyer");
                return;
            }

            try
            {
                string passHash = SecurityHelper.HashPassword(password);
                string query = @"INSERT INTO Users 
                    (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, PasswordHash, CreatedDate) 
                    VALUES (@Name, @Mobile, @Email, 'Buyer', 'Gujarat', @Specific, 'Verified', @PassHash, GETDATE())";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name",     fullName),
                    new SqlParameter("@Mobile",   mobile),
                    new SqlParameter("@Email",    email),
                    new SqlParameter("@Specific", specificField),
                    new SqlParameter("@PassHash", passHash)
                };
                DbHelper.ExecuteNonQuery(query, prms);

                object newId = DbHelper.ExecuteScalar("SELECT TOP 1 UserId FROM Users WHERE Email = @Email ORDER BY UserId DESC",
                    new SqlParameter[] { new SqlParameter("@Email", email) });
                if (newId != null)
                {
                    Session["UserId"] = Convert.ToInt32(newId);
                }

                Session["UserRole"]   = "Buyer";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                lblBuyerMsg.Text     = "Buyer enterprise account created and verified! Redirecting to marketplace...";
                lblBuyerMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=BuyerDashboard.aspx");
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
            lblBuyerMsg.Text     = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + msg;
            lblBuyerMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
