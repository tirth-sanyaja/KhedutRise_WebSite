using System;
using System.Web.UI;

namespace KhedutRise
{
    // User Sign In — Mobile Number verification with direct Email OTP dispatch
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

        // STEP 1: Dispatch OTP to user's registered Email
        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("LoginAuth");
            if (!Page.IsValid) return;

            string mob          = txtMobile.Text.Trim();
            string email        = txtEmail.Text.Trim();
            string selectedRole = GetSelectedRole();

            OtpRequestResult result = EmailOtpService.GenerateLoginEmailOtp(
                mob,
                email,
                selectedRole,
                Request.UserHostAddress);

            if (result.Success)
            {
                if (!string.IsNullOrEmpty(result.TargetEmail))
                {
                    txtEmail.Text = result.TargetEmail;
                }

                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background-color: #ecfdf5; border: 1.5px solid #10b981; color: #065f46; border-radius: 8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-envelope-check-fill me-1 text-success'></i> OTP Sent Successfully to {0}!</div>" +
                    "<div class='my-2'>Your 6-Digit Email OTP is: <span class='badge bg-success fs-5 px-3 py-1 font-monospace'>{1}</span></div>" +
                    "<div class='small text-muted'>Sent via Email â€¢ Valid for 5 minutes</div>" +
                    "</div>",
                    EmailOtpService.MaskEmail(result.TargetEmail),
                    result.PlainOtp);
                lblLoginMsg.CssClass = "d-block mb-2";

                hfCooldownRemaining.Value = "60";
                txtOtp.Text = result.PlainOtp;
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

        // STEP 2: Verify OTP and grant authenticated dashboard session
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("LoginAuth");
            Page.Validate("OtpAuth");
            if (!Page.IsValid) return;

            string mob          = txtMobile.Text.Trim();
            string otp          = txtOtp.Text.Trim();
            string selectedRole = GetSelectedRole();

            OtpVerificationResult result = EmailOtpService.VerifyLoginOtp(mob, otp, selectedRole);

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
            Session["UserMobile"] = result.MobileNo;
            Session["UserEmail"]  = result.Email;

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
