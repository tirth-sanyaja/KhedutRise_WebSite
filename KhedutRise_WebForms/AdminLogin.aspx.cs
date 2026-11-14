using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Administrator Portal Sign In — Two-Factor Authentication with Credentials and Dynamic Email OTP Verification
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(txtAdminEmail.Text))
                {
                    txtAdminEmail.Text = "admin@khedutrise.com";
                }
                txtAdminPass.Attributes["value"] = "admin123";

                if (Request.QueryString["error"] == "admin_required")
                {
                    lblAdminMsg.Text = "<i class='bi bi-shield-slash-fill me-1'></i> Administrative Access Required: Please sign in with administrator credentials and Email OTP.";
                    lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
            else
            {
                string postPass = Request.Form[txtAdminPass.UniqueID] ?? txtAdminPass.Text;
                if (!string.IsNullOrEmpty(postPass))
                {
                    txtAdminPass.Attributes["value"] = postPass;
                }
            }
        }

        // STEP 1: Validate Administrator Credentials & Dispatch Secure Email OTP
        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            string email = (txtAdminEmail.Text ?? "").Trim().ToLower();
            string pass = txtAdminPass.Text.Trim();

            if (string.IsNullOrEmpty(pass))
            {
                pass = (Request.Form[txtAdminPass.UniqueID] ?? "").Trim();
            }

            if (hfAdminCustomPass != null && !string.IsNullOrEmpty(hfAdminCustomPass.Value))
            {
                Session["AdminCustomPassword"] = hfAdminCustomPass.Value.Trim();
            }
            string customPass = Session["AdminCustomPassword"] as string;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                lblAdminMsg.Text = "Please enter both admin email and password.";
                lblAdminMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                return;
            }

            // Verify administrator password
            bool isEmailValid = (email == "admin@khedutrise.com" || email == "admin");
            bool isPassValid = (pass == "admin123" || pass == "11111111" || pass == "admin" || (!string.IsNullOrEmpty(customPass) && pass == customPass));

            if (!isEmailValid || !isPassValid)
            {
                lblAdminMsg.Text = "Invalid Admin credentials. Please verify your administrative email and password.";
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            string adminEmail = "admin@khedutrise.com";
            ViewState["AdminEmail"] = adminEmail;

            // Generate & Send OTP via Email
            OtpRequestResult otpRes = EmailOtpService.GenerateAndSendEmailOtp(
                adminEmail,
                "ADMIN_LOGIN",
                Request.UserHostAddress);

            if (otpRes.Success)
            {
                pnlAdminStep1.Visible = false;
                pnlAdminStep2.Visible = true;
                litAdminMaskedMobile.Text = EmailOtpService.MaskEmail(adminEmail);
                lblAdminMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background-color: #ecfdf5; border: 1.5px solid #10b981; color: #065f46; border-radius: 8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-shield-check me-1 text-success'></i> Credentials Verified &amp; OTP Sent!</div>" +
                    "<div class='small text-muted'>A 6-digit OTP has been sent to <strong>{0}</strong> via Email. Valid for 5 minutes.</div>" +
                    "</div>",
                    EmailOtpService.MaskEmail(adminEmail));
                lblAdminMsg.CssClass = "d-block mb-2";
                txtAdminOtp.Text = string.Empty;
                txtAdminOtp.Focus();
            }
            else
            {
                lblAdminMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + otpRes.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        // STEP 2: Verify Email OTP & Grant Admin Portal Session
        protected void btnAdminVerifyOtp_Click(object sender, EventArgs e)
        {
            string adminEmail = ViewState["AdminEmail"] as string ?? "admin@khedutrise.com";
            string otp = txtAdminOtp.Text.Trim();

            // Verify OTP via EmailOtpService
            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(adminEmail, otp, "ADMIN_LOGIN");

            if (!result.Success)
            {
                lblAdminMsg.Text = "<i class='bi bi-x-circle-fill me-1'></i> " + result.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                txtAdminOtp.Text = string.Empty;
                txtAdminOtp.Focus();
                return;
            }

            // Establish full Admin session
            Session["UserRole"]  = "Admin";
            Session["UserName"]  = !string.IsNullOrEmpty(result.FullName) ? result.FullName : "System Admin";
            Session["UserEmail"] = !string.IsNullOrEmpty(result.Email) ? result.Email : "admin@khedutrise.com";
            Session["UserId"]    = result.UserId;

            Response.Redirect("AdminDashboard.aspx");
        }

        // Resend Admin OTP
        protected void btnAdminResendOtp_Click(object sender, EventArgs e)
        {
            string adminEmail = ViewState["AdminEmail"] as string ?? "admin@khedutrise.com";

            OtpRequestResult otpRes = EmailOtpService.GenerateAndSendEmailOtp(
                adminEmail,
                "ADMIN_LOGIN",
                Request.UserHostAddress);

            if (otpRes.Success)
            {
                lblAdminMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background-color: #ecfdf5; border: 1.5px solid #10b981; color: #065f46; border-radius: 8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-arrow-clockwise me-1 text-success'></i> New OTP Dispatched!</div>" +
                    "<div class='small text-muted'>Sent via Email to <strong>{0}</strong> â€¢ Valid for 5 minutes</div>" +
                    "</div>",
                    EmailOtpService.MaskEmail(adminEmail));
                lblAdminMsg.CssClass = "d-block mb-2";
                txtAdminOtp.Text = string.Empty;
                txtAdminOtp.Focus();
            }
            else
            {
                lblAdminMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + otpRes.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        // Return to credentials step
        protected void btnAdminBackToCreds_Click(object sender, EventArgs e)
        {
            pnlAdminStep1.Visible = true;
            pnlAdminStep2.Visible = false;
            lblAdminMsg.Text = string.Empty;
        }
    }
}
