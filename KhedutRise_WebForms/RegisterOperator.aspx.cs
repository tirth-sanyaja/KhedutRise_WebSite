using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Operator Account Creation — requires email OTP verification before saving.
    public partial class RegisterOperator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // Send OTP to operator's email
        protected void btnSendOpOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("OpEmailVerify");
            if (!Page.IsValid) return;

            string email = txtOpEmail.Text.Trim().ToLower();

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
                pnlOpOtp.Visible          = true;
                pnlOpEmailVerified.Visible = false;
                hfOpEmailVerified.Value   = "0";
                lblOpOtpInfo.Text = string.Format(
                    "<i class='bi bi-envelope-check me-1'></i> OTP sent to {0}. Enter it below and click Verify OTP.",
                    EmailOtpService.MaskEmail(email));
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

            if (string.IsNullOrEmpty(otp)) { ShowError("Please enter the OTP sent to your email."); return; }

            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "REGISTER");

            if (result.Success)
            {
                hfOpEmailVerified.Value   = "1";
                pnlOpOtp.Visible          = false;
                pnlOpEmailVerified.Visible = true;
                lblOpMsg.Text             = string.Empty;
                txtOpEmail.ReadOnly       = true;
                btnSendOpOtp.Enabled      = false;
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

            string fullName       = txtName.Text.Trim();
            string mobile         = txtMobile.Text.Trim();
            string email          = txtOpEmail.Text.Trim().ToLower();
            string model          = txtModel.Text.Trim();
            string rate           = txtRate.Text.Trim();
            string attachments    = txtAttachments.Text.Trim();
            string radius         = txtRadius.Text.Trim();
            string password       = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)   { ShowError("Please enter your full name."); return; }
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)     { ShowError("Please enter a valid 10-digit mobile number."); return; }
            if (string.IsNullOrWhiteSpace(model))                             { ShowError("Please enter your machinery model."); return; }
            if (string.IsNullOrWhiteSpace(rate))                              { ShowError("Please enter your rental rate."); return; }
            if (string.IsNullOrWhiteSpace(attachments))                       { ShowError("Please specify available attachments."); return; }
            if (string.IsNullOrWhiteSpace(radius))                            { ShowError("Please specify your service radius."); return; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)  { ShowError("Password must be at least 6 characters."); return; }
            if (password != confirmPassword)                                   { ShowError("Passwords do not match."); return; }

            try
            {
                string query = "INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status) VALUES (@Name, @Mobile, @Email, 'Operator', 'Gujarat', @Field, 'Pending')";
                DbHelper.ExecuteNonQuery(query, new SqlParameter[]
                {
                    new SqlParameter("@Name",   fullName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Email",  email),
                    new SqlParameter("@Field",  model + " | Rate: ₹" + rate + "/hr | Area: " + radius)
                });

                Session["UserRole"]   = "Operator";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                lblOpMsg.Text     = "Machinery Operator registered successfully! Redirecting to rentals portal...";
                lblOpMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=TractorRentals.aspx");
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
            lblOpMsg.Text     = msg;
            lblOpMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
