using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace KhedutRise
{
    // Production-Ready Password Recovery via 6-Digit Email OTP Verification
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlRequestOtp.Visible     = true;
                pnlVerifyAndReset.Visible = false;
            }
        }

        // STEP 1: Handle Password Reset Request & Dispatch Branded Email OTP
        protected void btnSendResetOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailRequest");
            if (!Page.IsValid) return;

            string email = (txtResetEmail.Text ?? "").Trim().ToLower();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                ShowError("Please enter a valid registered email address.");
                return;
            }

            // Look up user account
            string sql = "SELECT TOP 1 UserId, FullName, Email, Status FROM Users WHERE LOWER(Email) = LOWER(@Email)";
            DataTable dt = DbHelper.ExecuteQuery(sql, new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dt == null || dt.Rows.Count == 0)
            {
                // Account enumeration protection: display standard message
                ShowError("If this email address is registered with KhedutRise, a password reset code has been dispatched. Please check your inbox or verify your email.");
                return;
            }

            DataRow u = dt.Rows[0];
            string status = u["Status"] != DBNull.Value ? u["Status"].ToString().Trim() : "Verified";
            if (status.Equals("Blocked", StringComparison.OrdinalIgnoreCase) || 
                status.Equals("Inactive", StringComparison.OrdinalIgnoreCase) || 
                status.Equals("Suspended", StringComparison.OrdinalIgnoreCase))
            {
                ShowError("This account is currently suspended or deactivated. Please contact customer support.");
                return;
            }

            string fullName = u["FullName"] != DBNull.Value ? u["FullName"].ToString().Trim() : "Valued Member";

            // Dispatch 6-digit OTP using EmailOtpService with branded email template
            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(
                email,
                "FORGOT_PASSWORD",
                Request.UserHostAddress,
                fullName);

            if (!result.Success)
            {
                ShowError(result.Message);
                return;
            }

            // Switch to Step 2
            pnlRequestOtp.Visible     = false;
            pnlVerifyAndReset.Visible = true;
            hfResetEmail.Value        = email;
            hfCooldownRemaining.Value = "60";
            Session["ResetUserEmail"] = email;
            Session["ResetUserName"]  = fullName;

            divTargetEmail.InnerText = EmailOtpService.MaskEmail(email);
            ShowSuccess("A 6-digit password reset code has been dispatched to your email. Please check your inbox and enter the code below.");
            txtResetOtp.Focus();
        }

        // STEP 2: Verify OTP and Update Password in Database
        protected void btnSubmitReset_Click(object sender, EventArgs e)
        {
            Page.Validate("ResetPassAuth");
            if (!Page.IsValid) return;

            string email    = !string.IsNullOrEmpty(hfResetEmail.Value) ? hfResetEmail.Value : (Session["ResetUserEmail"] != null ? Session["ResetUserEmail"].ToString() : "");
            string otp      = (txtResetOtp.Text ?? "").Trim();
            string newPass  = (txtNewPassword.Text ?? "").Trim();
            string confPass = (txtConfirmPassword.Text ?? "").Trim();

            if (string.IsNullOrEmpty(email))
            {
                ShowError("Session expired. Please request a new password reset code.");
                pnlRequestOtp.Visible     = true;
                pnlVerifyAndReset.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(otp) || otp.Length != 6)
            {
                ShowError("Please enter the complete 6-digit verification code.");
                return;
            }

            if (string.IsNullOrEmpty(newPass) || newPass.Length < 6)
            {
                ShowError("New password must be at least 6 characters long.");
                return;
            }

            if (newPass != confPass)
            {
                ShowError("Passwords do not match. Please verify and try again.");
                return;
            }

            // 1. Verify OTP securely against UserOTPs table
            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "FORGOT_PASSWORD");
            if (!result.Success)
            {
                ShowError(result.Message);
                txtResetOtp.Text = string.Empty;
                txtResetOtp.Focus();
                return;
            }

            // 2. Hash new password securely
            string passHash = SecurityHelper.HashPassword(newPass);

            try
            {
                // 3. Update password in database
                string updateSql = "UPDATE Users SET PasswordHash = @Hash WHERE LOWER(Email) = LOWER(@Email)";
                DbHelper.ExecuteNonQuery(updateSql, new SqlParameter[]
                {
                    new SqlParameter("@Hash", passHash),
                    new SqlParameter("@Email", email)
                });

                // 4. Invalidate any remaining reset OTPs
                DbHelper.ExecuteNonQuery(
                    "UPDATE UserOTPs SET IsUsed = 1 WHERE LOWER(Email) = LOWER(@Email) AND OTPPurpose = 'FORGOT_PASSWORD'",
                    new SqlParameter[] { new SqlParameter("@Email", email) });

                // 5. Send Password Changed confirmation email
                string userName = Session["ResetUserName"] != null ? Session["ResetUserName"].ToString() : "Valued Member";
                try
                {
                    string emailErr;
                    EmailService.SendPasswordChangedEmail(email, userName, out emailErr);
                }
                catch { }

                // Clear reset session
                Session["ResetUserEmail"] = null;
                Session["ResetUserName"]  = null;

                // 6. Redirect to Login Page with success flash message
                Response.Redirect("Login.aspx?msg=reset_success");
            }
            catch (Exception ex)
            {
                ShowError("Password update failed: " + ex.Message);
            }
        }

        // Resend Password Reset Code
        protected void btnResendResetOtp_Click(object sender, EventArgs e)
        {
            string email = !string.IsNullOrEmpty(hfResetEmail.Value) ? hfResetEmail.Value : (Session["ResetUserEmail"] != null ? Session["ResetUserEmail"].ToString() : "");
            string name  = Session["ResetUserName"] != null ? Session["ResetUserName"].ToString() : null;

            if (string.IsNullOrEmpty(email))
            {
                ShowError("Please start password recovery again.");
                pnlRequestOtp.Visible     = true;
                pnlVerifyAndReset.Visible = false;
                return;
            }

            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(
                email,
                "FORGOT_PASSWORD",
                Request.UserHostAddress,
                name);

            if (result.Success)
            {
                ShowSuccess("A new 6-digit verification code has been dispatched to " + EmailOtpService.MaskEmail(email));
                hfCooldownRemaining.Value = "60";
            }
            else
            {
                ShowError(result.Message);
                if (result.CooldownRemainingSeconds > 0)
                {
                    hfCooldownRemaining.Value = result.CooldownRemainingSeconds.ToString();
                }
            }
        }

        private void ShowError(string message)
        {
            lblResetMsg.Text = string.Format(
                "<div class='alert alert-danger d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' role='alert'>" +
                "<i class='bi bi-exclamation-triangle-fill fs-5 me-2 flex-shrink-0 text-danger'></i>" +
                "<div>{0}</div></div>",
                message);
            lblResetMsg.CssClass = "d-block mb-3";
        }

        private void ShowSuccess(string message)
        {
            lblResetMsg.Text = string.Format(
                "<div class='alert alert-success d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' role='alert' style='background-color: #ecfdf5; border-color: #10b981; color: #065f46;'>" +
                "<i class='bi bi-check-circle-fill fs-5 me-2 flex-shrink-0 text-success'></i>" +
                "<div>{0}</div></div>",
                message);
            lblResetMsg.CssClass = "d-block mb-3";
        }
    }
}
