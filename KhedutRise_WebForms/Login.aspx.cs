using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace KhedutRise
{
    // Secure Two-Step Authentication: Credentials Check + Genuine Email OTP Verification
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

        // STEP 1: Validate User Credentials against Database and Dispatch Genuine Email OTP
        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("LoginAuth");
            if (!Page.IsValid) return;

            string identifier   = (txtMobile.Text ?? "").Trim();
            string password     = (txtPassword.Text ?? "").Trim();
            string selectedRole = GetSelectedRole();

            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
            {
                lblLoginMsg.Text = "Please enter both your registered email/mobile and password.";
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            // 1. Verify credentials against Users table
            string query = @"SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole, PasswordHash, Status 
                             FROM Users 
                             WHERE (MobileNo = @Ident OR LOWER(Email) = LOWER(@Ident)) AND UserRole = @Role";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@Ident", identifier),
                new SqlParameter("@Role", selectedRole)
            };

            DataTable dt = DbHelper.ExecuteQuery(query, prms);
            if (dt == null || dt.Rows.Count == 0)
            {
                lblLoginMsg.Text = string.Format("No active {0} account was found matching '{1}'. Please check your credentials or register.", selectedRole, identifier);
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            DataRow user = dt.Rows[0];
            string storedHash = user["PasswordHash"] != DBNull.Value ? user["PasswordHash"].ToString() : "";
            
            // Validate password using SecurityHelper
            if (!SecurityHelper.VerifyPassword(password, storedHash))
            {
                lblLoginMsg.Text = "Incorrect account password. Please verify and try again.";
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            string userEmail = user["Email"] != DBNull.Value ? user["Email"].ToString().Trim() : "";
            if (string.IsNullOrEmpty(userEmail) || !userEmail.Contains("@"))
            {
                lblLoginMsg.Text = "No verified email address is linked to this account. Please contact support.";
                lblLoginMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                return;
            }

            // 2. Credentials valid -> Dispatch genuine Email OTP via SmtpClient
            OtpRequestResult result = EmailOtpService.GenerateLoginEmailOtp(
                identifier,
                userEmail,
                selectedRole,
                Request.UserHostAddress);

            if (result.Success)
            {
                pnlOtpSection.Visible = true;
                txtMobile.ReadOnly = true;
                txtPassword.Attributes["value"] = password;
                hfTargetEmail.Value = result.TargetEmail;
                hfTargetUser.Value = identifier;
                hfCooldownRemaining.Value = "60";

                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-3 text-center mb-3 shadow-sm' style='background-color: #ecfdf5; border: 1.5px solid #10b981; color: #065f46; border-radius: 8px;'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-envelope-check-fill me-1 text-success'></i> Credentials Verified!</div>" +
                    "<div class='small text-muted'>A 6-digit verification OTP has been delivered to your registered email <b>{0}</b>. Please check your inbox and enter the code below to complete sign-in.</div>" +
                    "</div>",
                    EmailOtpService.MaskEmail(result.TargetEmail));
                lblLoginMsg.CssClass = "d-block mb-2";

                txtOtp.Text = string.Empty;
                txtOtp.Focus();
            }
            else
            {
                lblLoginMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";

                if (result.CooldownRemainingSeconds > 0)
                {
                    hfCooldownRemaining.Value = result.CooldownRemainingSeconds.ToString();
                    pnlOtpSection.Visible = true;
                }
            }
        }

        // STEP 2: Verify OTP entered from email and grant authenticated dashboard session
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("OtpAuth");
            if (!Page.IsValid) return;

            string identifier   = !string.IsNullOrEmpty(hfTargetUser.Value) ? hfTargetUser.Value : (txtMobile.Text ?? "").Trim();
            string otp          = (txtOtp.Text ?? "").Trim();
            string selectedRole = GetSelectedRole();

            OtpVerificationResult result = EmailOtpService.VerifyLoginOtp(identifier, otp, selectedRole);

            if (!result.Success)
            {
                lblLoginMsg.Text = "<i class='bi bi-x-circle-fill me-1'></i> " + result.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                txtOtp.Text = string.Empty;
                txtOtp.Focus();
                return;
            }

            // Create Authenticated Session
            Session["UserId"]     = result.UserId;
            Session["UserName"]   = result.FullName;
            Session["UserRole"]   = result.UserRole;
            Session["UserMobile"] = result.MobileNo;
            Session["UserEmail"]  = result.Email;

            string returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.Contains("Login.aspx"))
            {
                Response.Redirect(returnUrl);
                return;
            }

            // Role-based Dashboard routing
            if (result.UserRole == "Farmer")
            {
                Response.Redirect("FarmerDashboard.aspx");
            }
            else if (result.UserRole == "Buyer")
            {
                Response.Redirect("BuyerDashboard.aspx");
            }
            else if (result.UserRole == "Operator")
            {
                Response.Redirect("TractorRentals.aspx");
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }

        // RESEND OTP
        protected void btnResendOtp_Click(object sender, EventArgs e)
        {
            string identifier   = !string.IsNullOrEmpty(hfTargetUser.Value) ? hfTargetUser.Value : (txtMobile.Text ?? "").Trim();
            string targetEmail  = hfTargetEmail.Value;
            string selectedRole = GetSelectedRole();

            OtpRequestResult result = EmailOtpService.GenerateLoginEmailOtp(
                identifier,
                targetEmail,
                selectedRole,
                Request.UserHostAddress);

            if (result.Success)
            {
                hfCooldownRemaining.Value = "60";
                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-success d-block p-2 text-center small mb-3'>" +
                    "<i class='bi bi-check-circle-fill me-1 text-success'></i> A fresh OTP has been sent to <b>{0}</b>. Please check your inbox." +
                    "</div>",
                    EmailOtpService.MaskEmail(result.TargetEmail));
                txtOtp.Text = string.Empty;
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

        private string GetSelectedRole()
        {
            if (rbBuyer.Checked)    return "Buyer";
            if (rbOperator.Checked) return "Operator";
            return "Farmer";
        }
    }
}
