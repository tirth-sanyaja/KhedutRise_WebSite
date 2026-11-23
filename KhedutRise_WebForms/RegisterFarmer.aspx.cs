using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace KhedutRise
{
    // Farmer Account Creation — requires genuine email OTP verification before saving.
    public partial class RegisterFarmer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Preserve password fields across postback so user doesn't have to retype
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Attributes["value"] = txtPassword.Text;
                }
                if (!string.IsNullOrEmpty(txtConfirmPassword.Text))
                {
                    txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;
                }
            }
        }

        // Send genuine OTP to the entered email address via SMTP (inline flow)
        protected void btnSendRegOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailVerify");
            if (!Page.IsValid) return;

            string email = (txtRegEmail.Text ?? "").Trim().ToLower();
            string rawMobile = (txtMobile.Text ?? "").Trim();
            string cleanMob = EmailOtpService.CleanMobile(rawMobile);

            // 1. Check if email is already registered
            var dtEmail = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = @Email",
                new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dtEmail != null && dtEmail.Rows.Count > 0)
            {
                ShowError(string.Format("The email address '{0}' is already registered. Please use a different email or sign in.", email));
                return;
            }

            // 2. If mobile is entered, check if mobile is already registered
            if (!string.IsNullOrEmpty(cleanMob))
            {
                var dtMob = DbHelper.ExecuteQuery(
                    "SELECT TOP 1 UserId FROM Users WHERE MobileNo = @Mob",
                    new SqlParameter[] { new SqlParameter("@Mob", cleanMob) });

                if (dtMob != null && dtMob.Rows.Count > 0)
                {
                    ShowError(string.Format("The mobile number (+91 {0}) is already registered with another account. Please sign in or use a different number.", cleanMob));
                    return;
                }
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
            string email = (txtRegEmail.Text ?? "").Trim().ToLower();
            string otp   = (txtRegOtp.Text ?? "").Trim();

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
                lblRegMsg.Text           = string.Empty;

                // Mark verified
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

            string fullName        = (txtFullName.Text ?? "").Trim();
            string rawMobile       = (txtMobile.Text ?? "").Trim();
            string crop            = (txtCrop.Text ?? "").Trim();
            string area            = (txtArea.Text ?? "").Trim();
            string email           = (txtRegEmail.Text ?? "").Trim().ToLower();
            string password        = (txtPassword.Text ?? "").Trim();
            string confirmPassword = (txtConfirmPassword.Text ?? "").Trim();

            // Field-level validations
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)
            { 
                ShowError("Please enter your full name."); 
                return; 
            }

            string mobile = EmailOtpService.CleanMobile(rawMobile);
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10 || !Regex.IsMatch(mobile, @"^[6-9]\d{9}$"))
            { 
                ShowError("Please enter a valid 10-digit Indian mobile number (e.g. 9825012345)."); 
                return; 
            }

            if (string.IsNullOrWhiteSpace(crop))
            { 
                ShowError("Please enter your primary crop."); 
                return; 
            }

            if (string.IsNullOrWhiteSpace(area))
            { 
                ShowError("Please specify your farm area."); 
                return; 
            }

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { 
                ShowError("Please enter a valid email address."); 
                return; 
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            { 
                ShowError("Password must be at least 6 characters."); 
                return; 
            }

            if (password != confirmPassword)
            { 
                ShowError("Passwords do not match."); 
                return; 
            }

            // Check duplicate Mobile specifically
            var dtMob = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE MobileNo = @Mob",
                new SqlParameter[] { new SqlParameter("@Mob", mobile) });

            if (dtMob != null && dtMob.Rows.Count > 0)
            {
                ShowError(string.Format("The mobile number (+91 {0}) is already registered with another account. Please sign in or use a different mobile number.", mobile));
                return;
            }

            // Check duplicate Email specifically
            var dtEmail = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = LOWER(@Email)",
                new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dtEmail != null && dtEmail.Rows.Count > 0)
            {
                ShowError(string.Format("The email address '{0}' is already registered with an existing account. Please sign in or use a different email.", email));
                return;
            }

            string specificField = crop + " (" + area + " Acres)";
            string passHash = SecurityHelper.HashPassword(password);

            // If user has not yet verified their email inline, dispatch OTP and redirect to dedicated VerifyOtp page
            if (hfEmailVerified.Value != "1")
            {
                PendingRegistration pending = new PendingRegistration
                {
                    Role          = "Farmer",
                    FullName      = fullName,
                    MobileNo      = mobile,
                    Email         = email,
                    Location      = "Gujarat",
                    SpecificField = specificField,
                    PasswordHash  = passHash
                };

                string payload = pending.Serialize();

                OtpRequestResult otpRes = EmailOtpService.GenerateAndSendEmailOtp(
                    email, 
                    "REGISTER", 
                    Request.UserHostAddress, 
                    fullName, 
                    payload);

                if (!otpRes.Success)
                {
                    ShowError(otpRes.Message);
                    return;
                }

                Session["PendingRegistration"] = pending;

                Response.Redirect("VerifyOtp.aspx?purpose=REGISTER&email=" + Server.UrlEncode(email) + "&role=Farmer");
                return;
            }

            // If already verified inline, save directly to database
            try
            {
                string query = @"INSERT INTO Users 
                    (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, PasswordHash, CreatedDate) 
                    VALUES (@Name, @Mobile, @Email, 'Farmer', 'Gujarat', @Crop, 'Verified', @PassHash, GETDATE())";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name",     fullName),
                    new SqlParameter("@Mobile",   mobile),
                    new SqlParameter("@Email",    email),
                    new SqlParameter("@Crop",     specificField),
                    new SqlParameter("@PassHash", passHash)
                };
                DbHelper.ExecuteNonQuery(query, prms);

                // Fetch new user id
                object newId = DbHelper.ExecuteScalar("SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = LOWER(@Email) ORDER BY UserId DESC",
                    new SqlParameter[] { new SqlParameter("@Email", email) });
                if (newId != null)
                {
                    Session["UserId"] = Convert.ToInt32(newId);
                }

                Session["UserRole"]   = "Farmer";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                // Send welcome email
                try
                {
                    string welcomeErr;
                    EmailService.SendWelcomeEmail(email, fullName, "Farmer", out welcomeErr);
                }
                catch { }

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
