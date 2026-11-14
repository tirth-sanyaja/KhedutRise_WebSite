using System;
using System.Web.UI;

namespace KhedutRise
{
    // User Sign In — supports both Mobile SMS OTP and Email OTP authentication.
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string error = Request.QueryString["error"];
                if (error == "auth_required")
                {
                    lblLoginMsg.Text = "Authentication Required: Please sign in to access portal features.";
                    lblLoginMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                }
                else if (error == "unauthorized_role")
                {
                    lblLoginMsg.Text = "Access Denied: Your account role is not authorized to access that dashboard.";
                    lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
        }

        // ── SMS OTP: Send OTP via SMS ─────────────────────────────────────────
        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            hfAuthMethod.Value = "sms";
            Page.Validate("MobileAuth");
            if (!Page.IsValid) return;

            string mob          = txtMobile.Text.Trim();
            string selectedRole = GetSelectedRole();

            OtpRequestResult result = OtpService.GenerateAndSendOtp(mob, selectedRole, "LOGIN", Request.UserHostAddress);

            if (result.Success)
            {
                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background:#ecfdf5; border:1.5px solid #10b981; color:#065f46; border-radius:8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-check-circle-fill me-1 text-success'></i> OTP Sent to +91 {0}</div>" +
                    "<div class='small text-muted mt-1'>Enter the OTP below and click Verify OTP &amp; Sign In.</div>" +
                    "</div>",
                    OtpService.MaskMobile(mob));
                lblLoginMsg.CssClass = "d-block mb-2";
                hfCooldownRemaining.Value = "60";
                txtOtp.Focus();
            }
            else
            {
                lblLoginMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                if (result.CooldownRemainingSeconds > 0)
                    hfCooldownRemaining.Value = result.CooldownRemainingSeconds.ToString();
            }
        }

        // ── Email OTP: Send OTP via Email ─────────────────────────────────────
        protected void btnSendEmailOtp_Click(object sender, EventArgs e)
        {
            hfAuthMethod.Value = "email";
            Page.Validate("EmailAuth");
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim().ToLower();

            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(email, "LOGIN", Request.UserHostAddress);

            if (result.Success)
            {
                // Mask and show success — never expose the OTP in the UI for email (use inbox)
                string masked = EmailOtpService.MaskEmail(email);
                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background:#ecfdf5; border:1.5px solid #10b981; color:#065f46; border-radius:8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-envelope-check-fill me-1 text-success'></i> OTP Sent to {0}</div>" +
                    "<div class='small text-muted mt-1'>Check your inbox and enter the 6-digit OTP below.</div>" +
                    "</div>",
                    masked);
                lblLoginMsg.CssClass = "d-block mb-2";
                hfEmailCooldownRemaining.Value = "60";
                txtOtp.Focus();
            }
            else
            {
                lblLoginMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                if (result.CooldownRemainingSeconds > 0)
                    hfEmailCooldownRemaining.Value = result.CooldownRemainingSeconds.ToString();
            }
        }

        // ── Verify OTP & create session (handles both SMS and Email) ──────────
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("OtpAuth");
            if (!Page.IsValid) return;

            string authMethod = (hfAuthMethod.Value ?? "sms").ToLower();
            string otp        = txtOtp.Text.Trim();
            OtpVerificationResult result;

            if (authMethod == "email")
            {
                string email = txtEmail.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(email))
                {
                    ShowError("Please enter your email address and request an OTP first.");
                    return;
                }
                result = EmailOtpService.VerifyEmailOtp(email, otp, "LOGIN");
            }
            else
            {
                Page.Validate("MobileAuth");
                if (!Page.IsValid) return;

                string mob          = txtMobile.Text.Trim();
                string selectedRole = GetSelectedRole();
                result = OtpService.VerifyOtp(mob, otp, selectedRole, "LOGIN");
            }

            if (!result.Success)
            {
                ShowError(result.Message);
                txtOtp.Text = string.Empty;
                txtOtp.Focus();
                return;
            }

            // Establish verified session
            Session["UserId"]     = result.UserId;
            Session["UserName"]   = result.FullName;
            Session["UserRole"]   = result.UserRole;
            Session["UserMobile"] = result.MobileNo;
            Session["UserEmail"]  = result.Email;

            // Route to role dashboard
            string returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) &&
                returnUrl.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) &&
                !returnUrl.Equals("Default.aspx", StringComparison.OrdinalIgnoreCase) &&
                !returnUrl.Contains("Admin"))
            {
                Response.Redirect(returnUrl);
                return;
            }

            if (result.UserRole.Equals("Buyer",    StringComparison.OrdinalIgnoreCase)) Response.Redirect("BuyerDashboard.aspx");
            else if (result.UserRole.Equals("Operator", StringComparison.OrdinalIgnoreCase)) Response.Redirect("TractorRentals.aspx");
            else Response.Redirect("FarmerDashboard.aspx");
        }

        private string GetSelectedRole()
        {
            if (rbBuyer.Checked)    return "Buyer";
            if (rbOperator.Checked) return "Operator";
            return "Farmer";
        }

        private void ShowError(string msg)
        {
            lblLoginMsg.Text = "<i class='bi bi-x-circle-fill me-1'></i> " + msg;
            lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
