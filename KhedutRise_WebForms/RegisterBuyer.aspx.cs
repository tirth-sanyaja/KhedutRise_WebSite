using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Buyer Account Creation — requires email OTP verification before saving.
    public partial class RegisterBuyer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // Send OTP to the email address already entered in txtEmail
        protected void btnSendBuyerOtp_Click(object sender, EventArgs e)
        {
            string email = (txtEmail.Text ?? "").Trim().ToLower();

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                ShowError("Please enter a valid business email address in the Email field above, then click Send OTP.");
                return;
            }

            // Check if email already registered
            var dtCheck = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = @Email",
                new SqlParameter[] { new SqlParameter("@Email", email) });

            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                ShowError("This email address is already registered. Please use a different email or sign in.");
                return;
            }

            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(email, "REGISTER", Request.UserHostAddress);

            if (result.Success)
            {
                pnlBuyerOtp.Visible          = true;
                pnlBuyerEmailVerified.Visible = false;
                hfBuyerEmailVerified.Value    = "0";
                lblBuyerOtpInfo.Text = string.Format(
                    "<i class='bi bi-envelope-check me-1'></i> OTP sent to {0}. Enter it below and click Verify OTP.",
                    EmailOtpService.MaskEmail(email));
                lblBuyerMsg.Text = string.Empty;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        // Verify OTP
        protected void btnVerifyBuyerOtp_Click(object sender, EventArgs e)
        {
            string email = (txtEmail.Text ?? "").Trim().ToLower();
            string otp   = (txtBuyerOtp.Text ?? "").Trim();

            if (string.IsNullOrEmpty(otp)) { ShowError("Please enter the OTP sent to your email."); return; }

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

            if (hfBuyerEmailVerified.Value != "1")
            {
                ShowError("Please verify your email address with the OTP before creating your account.");
                pnlBuyerOtp.Visible = true;
                return;
            }

            string fullName       = txtFullName.Text.Trim();
            string mobile         = txtMobile.Text.Trim();
            string email          = txtEmail.Text.Trim().ToLower();
            string bizType        = ddlBizType.SelectedValue;
            string interests      = txtInterests.Text.Trim();
            string gstin          = txtGstin.Text.Trim();
            string password       = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)        { ShowError("Please enter your name."); return; }
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)          { ShowError("Please enter a valid 10-digit mobile number."); return; }
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))          { ShowError("Please enter a valid business email."); return; }
            if (string.IsNullOrWhiteSpace(bizType))                                { ShowError("Please select your business type."); return; }
            if (string.IsNullOrWhiteSpace(gstin))                                  { ShowError("Please provide your GSTIN or license number."); return; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)        { ShowError("Password must be at least 6 characters."); return; }
            if (password != confirmPassword)                                        { ShowError("Passwords do not match."); return; }

            try
            {
                string query = "INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status) VALUES (@Name, @Mobile, @Email, 'Buyer', 'Gujarat', @Field, 'Verified')";
                DbHelper.ExecuteNonQuery(query, new SqlParameter[]
                {
                    new SqlParameter("@Name",   fullName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Email",  email),
                    new SqlParameter("@Field",  bizType + " | " + interests + " (GST: " + gstin + ")")
                });

                Session["UserRole"]   = "Buyer";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                lblBuyerMsg.Text     = "Buyer account registered successfully! Redirecting to your dashboard...";
                lblBuyerMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=BuyerDashboard.aspx");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE") || ex.Message.Contains("duplicate"))
                    ShowError("This mobile number is already registered. Please sign in or use another number.");
                else
                    ShowError("Registration failed: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblBuyerMsg.Text     = " " + msg;
            lblBuyerMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
