using System;
using System.Web.UI;

namespace KhedutRise
{
    // User Sign In - Secure dynamic Mobile Number + SMS OTP Authentication with strict role verification
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

        // Handle Send OTP request: generates secure OTP, hashes it, and dispatches via SMS Gateway
        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("MobileAuth");
            if (!Page.IsValid)
            {
                return;
            }

            string mob = txtMobile.Text.Trim();
            string selectedRole = GetSelectedRole();

            // Request OTP generation & SMS dispatch through unified OtpService
            OtpRequestResult result = OtpService.GenerateAndSendOtp(
                mob,
                selectedRole,
                "LOGIN",
                Request.UserHostAddress);

            if (result.Success)
            {
                lblLoginMsg.Text = "<i class='bi bi-check-circle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
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

        // Handle OTP verification and authenticated session establishment
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("MobileAuth");
            Page.Validate("OtpAuth");
            if (!Page.IsValid)
            {
                return;
            }

            string mob = txtMobile.Text.Trim();
            string otp = txtOtp.Text.Trim();
            string selectedRole = GetSelectedRole();

            // Perform cryptographic OTP verification with database role validation
            OtpVerificationResult result = OtpService.VerifyOtp(mob, otp, selectedRole, "LOGIN");

            if (!result.Success)
            {
                lblLoginMsg.Text = "<i class='bi bi-x-circle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                txtOtp.Text = string.Empty;
                txtOtp.Focus();
                return;
            }

            // Establish secure server-side session values with real database primary key & verified role
            Session["UserId"] = result.UserId;
            Session["UserName"] = result.FullName;
            Session["UserRole"] = result.UserRole;
            Session["UserMobile"] = result.MobileNo;
            Session["UserEmail"] = result.Email;

            // Route user to their verified role dashboard or requested return URL
            string returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) && 
                returnUrl.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) && 
                !returnUrl.Equals("Default.aspx", StringComparison.OrdinalIgnoreCase) &&
                !returnUrl.Contains("Admin"))
            {
                Response.Redirect(returnUrl);
                return;
            }

            // Role-based dashboard dispatching
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

        // Helper to retrieve selected public role from the radio button group
        private string GetSelectedRole()
        {
            if (rbBuyer.Checked) return "Buyer";
            if (rbOperator.Checked) return "Operator";
            return "Farmer";
        }
    }
}
