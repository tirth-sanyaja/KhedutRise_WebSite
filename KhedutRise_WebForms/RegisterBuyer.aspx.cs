using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace KhedutRise
{
    // Buyer / Corporate Account Creation — requires genuine email OTP verification before saving.
    public partial class RegisterBuyer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Preserve password fields across postback
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

        // Send genuine OTP to buyer email (inline flow)
        protected void btnSendBuyerOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailVerify");
            if (!Page.IsValid) return;

            string email = (txtEmail.Text ?? "").Trim().ToLower();
            string rawMobile = (txtMobile.Text ?? "").Trim();
            string cleanMob = EmailOtpService.CleanMobile(rawMobile);

            var dtCheck = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = @Email",
                new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                ShowError(string.Format("The email address '{0}' is already registered. Please sign in or use another email.", email));
                return;
            }

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

            string fullName        = (txtFullName.Text ?? "").Trim();
            string rawMobile       = (txtMobile.Text ?? "").Trim();
            string email           = (txtEmail.Text ?? "").Trim().ToLower();
            string bizType         = ddlBizType.SelectedValue;
            string interests       = (txtInterests.Text ?? "").Trim();
            string gstin           = (txtGstin.Text ?? "").Trim();
            string password        = (txtPassword.Text ?? "").Trim();
            string confirmPassword = (txtConfirmPassword.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)
            { 
                ShowError("Please enter full business or personal name."); 
                return; 
            }

            string mobile = EmailOtpService.CleanMobile(rawMobile);
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10 || !Regex.IsMatch(mobile, @"^[6-9]\d{9}$"))
            { 
                ShowError("Please enter a valid 10-digit Indian mobile number (e.g. 9825012345)."); 
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

            string specificField = bizType + " | GSTIN: " + (string.IsNullOrWhiteSpace(gstin) ? "N/A" : gstin) + " | Crops: " + interests;
            string passHash = SecurityHelper.HashPassword(password);

            // If not verified inline yet -> dispatch OTP and redirect to VerifyOtp.aspx
            if (hfBuyerEmailVerified.Value != "1")
            {
                PendingRegistration pending = new PendingRegistration
                {
                    Role          = "Buyer",
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

                Response.Redirect("VerifyOtp.aspx?purpose=REGISTER&email=" + Server.UrlEncode(email) + "&role=Buyer");
                return;
            }

            try
            {
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

                object newId = DbHelper.ExecuteScalar("SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = LOWER(@Email) ORDER BY UserId DESC",
                    new SqlParameter[] { new SqlParameter("@Email", email) });
                if (newId != null)
                {
                    Session["UserId"] = Convert.ToInt32(newId);
                }

                Session["UserRole"]   = "Buyer";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                // Send welcome email
                try
                {
                    string welcomeErr;
                    EmailService.SendWelcomeEmail(email, fullName, "Buyer", out welcomeErr);
                }
                catch { }

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
