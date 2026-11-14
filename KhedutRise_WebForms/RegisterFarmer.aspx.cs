using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Farmer Account Creation — requires email OTP verification before saving.
    public partial class RegisterFarmer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        // Send OTP to the entered email address
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

            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(email, "REGISTER", Request.UserHostAddress);

            if (result.Success)
            {
                pnlOtpVerify.Visible     = true;
                pnlEmailVerified.Visible = false;
                hfEmailVerified.Value    = "0";
                lblOtpInfo.Text = string.Format(
                    "<i class='bi bi-envelope-check me-1'></i> OTP sent to {0}. Enter it below and click Verify OTP.",
                    EmailOtpService.MaskEmail(email));
                lblRegMsg.Text = string.Empty;
            }
            else
            {
                ShowError(result.Message);
            }
        }

        // Verify OTP entered by the user
        protected void btnVerifyRegOtp_Click(object sender, EventArgs e)
        {
            string email  = txtRegEmail.Text.Trim().ToLower();
            string otp    = txtRegOtp.Text.Trim();

            if (string.IsNullOrEmpty(otp))
            {
                ShowError("Please enter the OTP sent to your email.");
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

        // Final form submission — only proceeds if email is verified
        protected void btnSubmitFarmer_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // Guard: email must be verified
            if (hfEmailVerified.Value != "1")
            {
                ShowError("Please verify your email address with the OTP before creating your account.");
                pnlOtpVerify.Visible = true;
                return;
            }

            string fullName       = txtFullName.Text.Trim();
            string mobile         = txtMobile.Text.Trim();
            string crop           = txtCrop.Text.Trim();
            string area           = txtArea.Text.Trim();
            string email          = txtRegEmail.Text.Trim().ToLower();
            string password       = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)        { ShowError("Please enter your full name."); return; }
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)          { ShowError("Please enter a valid 10-digit mobile number."); return; }
            if (string.IsNullOrWhiteSpace(crop))                                   { ShowError("Please enter your primary crop."); return; }
            if (string.IsNullOrWhiteSpace(area))                                   { ShowError("Please specify your farm area."); return; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)        { ShowError("Password must be at least 6 characters."); return; }
            if (password != confirmPassword)                                        { ShowError("Passwords do not match."); return; }

            try
            {
                string query = "INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status) VALUES (@Name, @Mobile, @Email, 'Farmer', 'Gujarat', @Crop, 'Verified')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name",   fullName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Email",  email),
                    new SqlParameter("@Crop",   crop + " (" + area + " Acres)")
                };
                DbHelper.ExecuteNonQuery(query, prms);

                Session["UserRole"]   = "Farmer";
                Session["UserName"]   = fullName;
                Session["UserMobile"] = mobile;
                Session["UserEmail"]  = email;

                lblRegMsg.Text     = "Farmer account created successfully! Redirecting to your dashboard...";
                lblRegMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=FarmerDashboard.aspx");
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
            lblRegMsg.Text     = " " + msg;
            lblRegMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
