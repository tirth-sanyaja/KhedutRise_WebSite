using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Machinery Operator Registration — requires genuine email OTP verification before saving.
    public partial class RegisterOperator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // Send genuine OTP to operator email
        protected void btnSendOpOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailVerify");
            if (!Page.IsValid) return;

            string email = txtOpEmail.Text.Trim().ToLower();

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
                pnlOpOtp.Visible           = true;
                pnlOpEmailVerified.Visible = false;
                hfOpEmailVerified.Value    = "0";
                lblOpOtpInfo.Text = string.Format(
                    "<div class='alert alert-success py-2 px-3 mb-2 small fw-bold'>" +
                    "<i class='bi bi-envelope-check-fill me-1 text-success'></i> Verification OTP has been dispatched to {0}! " +
                    "Please check your email inbox (and spam folder) and enter the 6-digit code below." +
                    "</div>",
                    EmailOtpService.MaskEmail(email));
                txtOpOtp.Text = string.Empty;
                txtOpOtp.Focus();
                lblOpMsg.Text = string.Empty;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        // Verify OTP
        protected void btnVerifyOpOtp_Click(object sender, EventArgs e)
        {
            string email = txtOpEmail.Text.Trim().ToLower();
            string otp   = (txtOpOtp.Text ?? "").Trim();

            if (string.IsNullOrEmpty(otp)) { ShowError("Please enter the 6-digit OTP sent to your email."); return; }

            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "REGISTER");

            if (result.Success)
            {
                hfOpEmailVerified.Value    = "1";
                pnlOpOtp.Visible           = false;
                pnlOpEmailVerified.Visible = true;
                lblOpMsg.Text              = string.Empty;
                txtOpEmail.ReadOnly        = true;
                btnSendOpOtp.Enabled       = false;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        protected void btnSubmitOp_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (hfOpEmailVerified.Value != "1")
            {
                ShowError("Please verify your email address with the OTP before creating your account.");
                pnlOpOtp.Visible = true;
                return;
            }

            string fullName        = txtName.Text.Trim();
            string mobile          = txtMobile.Text.Trim();
            string email           = txtOpEmail.Text.Trim().ToLower();
            string machineModel    = txtModel.Text.Trim();
            string baseRate        = txtRate.Text.Trim();
            string attachments     = txtAttachments.Text.Trim();
            string radius          = txtRadius.Text.Trim();
            string password        = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)     { ShowError("Please enter your full name."); return; }
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)       { ShowError("Please enter a valid 10-digit mobile number."); return; }
            if (string.IsNullOrWhiteSpace(machineModel))                        { ShowError("Please specify tractor / equipment model and HP."); return; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)     { ShowError("Password must be at least 6 characters."); return; }
            if (password != confirmPassword)                                     { ShowError("Passwords do not match."); return; }

            var dtMob = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE MobileNo = @Mob",
                new SqlParameter[] { new SqlParameter("@Mob", mobile) });
            if (dtMob != null && dtMob.Rows.Count > 0)
            {
                ShowError("This mobile number is already registered. Please sign in.");
                return;
            }

            try
            {
                string passHash = SecurityHelper.HashPassword(password);
                string specificField = machineModel + " (₹" + baseRate + "/hr) | " + attachments + " | " + radius + " km";
                string query = @"INSERT INTO Users 
                    (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, PasswordHash, CreatedDate) 
                    VALUES (@Name, @Mobile, @Email, 'Operator', 'Gujarat', @Specific, 'Verified', @PassHash, GETDATE())";
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

                Session["UserRole"]   = "Operator";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                lblOpMsg.Text     = "Machinery Operator account registered and verified! Redirecting to equipment rentals...";
                lblOpMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=TractorRentals.aspx");
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
            lblOpMsg.Text     = " " + msg;
            lblOpMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
