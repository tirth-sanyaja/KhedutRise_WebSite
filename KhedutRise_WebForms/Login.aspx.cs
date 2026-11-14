using System;
using System.Web.UI;

namespace KhedutRise
{
    // User Sign In — Pure, direct Email OTP authentication with role verification
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

        // STEP 1: Send 6-digit OTP to user's registered email
        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailAuth");
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim().ToLower();
            string selectedRole = GetSelectedRole();

            OtpRequestResult result = EmailOtpService.GenerateAndSendEmailOtp(
                email,
                "LOGIN",
                Request.UserHostAddress,
                selectedRole);

            if (result.Success)
            {
                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background:#ecfdf5; border:1.5px solid #10b981; color:#065f46; border-radius:8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-envelope-check-fill me-1 text-success'></i> OTP Sent Successfully to {0}!</div>" +
                    "<div class='small text-muted mt-1'>Please check your email inbox and enter the 6-digit OTP below.</div>" +
                    "</div>",
                    EmailOtpService.MaskEmail(email));
                lblLoginMsg.CssClass = "d-block mb-2";
                hfCooldownRemaining.Value = "60";
                txtOtp.Focus();
            }
            else
            {
                lblLoginMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                if (result.CooldownRemainingSeconds > 0)
                {
                    hfCooldownRemaining.Value = result.CooldownRemainingSeconds.ToString();
                }
            }
        }

        // STEP 2: Verify OTP and create authenticated session
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("EmailAuth");
            Page.Validate("OtpAuth");
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim().ToLower();
            string otp = txtOtp.Text.Trim();
            string selectedRole = GetSelectedRole();

            // Verify OTP securely via EmailOtpService
            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "LOGIN", selectedRole);

            if (!result.Success)
            {
                lblLoginMsg.Text = "<i class='bi bi-x-circle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                txtOtp.Text = string.Empty;
                txtOtp.Focus();
                return;
            }

            // Establish secure server-side session
            Session["UserId"]     = result.UserId;
            Session["UserName"]   = result.FullName;
            Session["UserRole"]   = result.UserRole;
            Session["UserEmail"]  = result.Email;
            Session["UserMobile"] = result.MobileNo;

            // Route to requested return URL if valid
            string returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) &&
                returnUrl.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) &&
                !returnUrl.Equals("Default.aspx", StringComparison.OrdinalIgnoreCase) &&
                !returnUrl.Contains("Admin"))
            {
                Response.Redirect(returnUrl);
                return;
            }

            // Role-based dashboard redirect
            if (result.UserRole.Equals("Buyer", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("BuyerDashboard.aspx");
            }
            else if (result.UserRole.Equals("Operator", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("TractorRentals.aspx");
            }
            else
            {
                Response.Redirect("FarmerDashboard.aspx");
            }
        }

        private string GetSelectedRole()
        {
            if (rbBuyer.Checked) return "Buyer";
            if (rbOperator.Checked) return "Operator";
            return "Farmer";
        }
    }
}
