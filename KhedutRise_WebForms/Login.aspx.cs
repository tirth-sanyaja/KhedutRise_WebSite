using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace KhedutRise
{
    // Production-Ready Authentication: Email + Password Authentication with Role-Based Routing
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check flash status messages
                string msg = Request.QueryString["msg"];
                if (msg == "registered")
                {
                    lblLoginMsg.Text = 
                        "<div class='alert alert-success d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' style='background-color: #ecfdf5; border-color: #10b981; color: #065f46;'>" +
                        "<i class='bi bi-check-circle-fill fs-5 me-2 flex-shrink-0 text-success'></i>" +
                        "<div><strong>Registration Successful!</strong> Your email has been verified. Please login to continue.</div>" +
                        "</div>";
                }
                else if (msg == "reset_success")
                {
                    lblLoginMsg.Text = 
                        "<div class='alert alert-success d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' style='background-color: #ecfdf5; border-color: #10b981; color: #065f46;'>" +
                        "<i class='bi bi-check-circle-fill fs-5 me-2 flex-shrink-0 text-success'></i>" +
                        "<div><strong>Password Updated!</strong> Your password has been successfully updated. Please login with your new password.</div>" +
                        "</div>";
                }

                // Check authorization error alerts
                string error = Request.QueryString["error"];
                if (error == "auth_required")
                {
                    lblLoginMsg.Text = 
                        "<div class='alert alert-warning d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm'>" +
                        "<i class='bi bi-exclamation-triangle-fill fs-5 me-2 flex-shrink-0'></i>" +
                        "<div><strong>Authentication Required:</strong> Please sign in to access portal features.</div>" +
                        "</div>";
                }
                else if (error == "unauthorized_role")
                {
                    lblLoginMsg.Text = 
                        "<div class='alert alert-danger d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm'>" +
                        "<i class='bi bi-shield-x fs-5 me-2 flex-shrink-0'></i>" +
                        "<div><strong>Access Denied:</strong> Your account role is not authorized to access that dashboard.</div>" +
                        "</div>";
                }

                // Pre-fill email from Remember Me cookie if available
                HttpCookie rememberCookie = Request.Cookies["KR_REMEMBER"];
                if (rememberCookie != null && !string.IsNullOrEmpty(rememberCookie.Value))
                {
                    txtEmail.Text = rememberCookie.Value.Trim();
                    chkRememberMe.Checked = true;
                    txtPassword.Focus();
                }
                else
                {
                    txtEmail.Focus();
                }
            }
        }

        // Direct Email + Password Authentication
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Page.Validate("LoginAuth");
            if (!Page.IsValid) return;

            string emailInput = (txtEmail.Text ?? "").Trim();
            string password   = (txtPassword.Text ?? "").Trim();

            // 1. Validate required fields
            if (string.IsNullOrEmpty(emailInput))
            {
                ShowError("Please enter your registered email address.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Please enter your account password.");
                return;
            }

            // 2. Query user from database
            string query = @"SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole, PasswordHash, Status 
                             FROM Users 
                             WHERE LOWER(Email) = LOWER(@Ident) OR MobileNo = @Ident";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@Ident", emailInput)
            };

            DataTable dt = DbHelper.ExecuteQuery(query, prms);
            if (dt == null || dt.Rows.Count == 0)
            {
                // Unregistered email -> standard generic error to prevent account enumeration
                ShowError("Invalid email or password.");
                return;
            }

            DataRow user = dt.Rows[0];
            string storedHash = user["PasswordHash"] != DBNull.Value ? user["PasswordHash"].ToString() : "";

            // 3. Verify password hash using SecurityHelper
            if (!SecurityHelper.VerifyPassword(password, storedHash))
            {
                ShowError("Invalid email or password.");
                return;
            }

            // 4. Verify Account Status
            string status = user["Status"] != DBNull.Value ? user["Status"].ToString().Trim() : "Verified";

            // Blocked or Deactivated check
            if (status.Equals("Blocked", StringComparison.OrdinalIgnoreCase) || 
                status.Equals("Inactive", StringComparison.OrdinalIgnoreCase) || 
                status.Equals("Suspended", StringComparison.OrdinalIgnoreCase))
            {
                ShowError("Your account has been deactivated or suspended. Please contact support.");
                return;
            }

            // Unverified account check
            if (!status.Equals("Verified", StringComparison.OrdinalIgnoreCase))
            {
                string userEmail = user["Email"] != DBNull.Value ? user["Email"].ToString().Trim() : emailInput;
                string userRole  = user["UserRole"] != DBNull.Value ? user["UserRole"].ToString().Trim() : "Farmer";

                lblLoginMsg.Text = string.Format(
                    "<div class='alert alert-warning mb-3 p-3 rounded-3 shadow-sm'>" +
                    "<div class='fw-bold mb-1'><i class='bi bi-envelope-exclamation me-1'></i> Email Not Verified</div>" +
                    "<div class='small text-muted mb-2'>Your email is not verified yet. Please complete verification to continue.</div>" +
                    "<a href='VerifyOtp.aspx?purpose=REGISTER&email={0}&role={1}' class='btn btn-sm btn-warning fw-bold px-3 py-1 text-dark'>" +
                    "Verify Email Now &rarr;</a></div>",
                    Server.UrlEncode(userEmail), Server.UrlEncode(userRole));
                lblLoginMsg.CssClass = "d-block mb-3";
                return;
            }

            // 5. Successful Authentication -> Create Session
            int userId = Convert.ToInt32(user["UserId"]);
            string fullName = user["FullName"] != DBNull.Value ? user["FullName"].ToString().Trim() : "User";
            string role = user["UserRole"] != DBNull.Value ? user["UserRole"].ToString().Trim() : "Farmer";
            string dbEmail = user["Email"] != DBNull.Value ? user["Email"].ToString().Trim() : emailInput;
            string dbMobile = user["MobileNo"] != DBNull.Value ? user["MobileNo"].ToString().Trim() : "";

            Session["UserId"]     = userId;
            Session["UserName"]   = fullName;
            Session["UserRole"]   = role;
            Session["UserEmail"]  = dbEmail;
            Session["UserMobile"] = dbMobile;

            // 6. Handle Remember Me persistent cookie
            if (chkRememberMe.Checked)
            {
                HttpCookie authCookie = new HttpCookie("KR_REMEMBER", dbEmail);
                authCookie.Expires = DateTime.Now.AddDays(14);
                authCookie.HttpOnly = true;
                Response.Cookies.Add(authCookie);
            }
            else
            {
                if (Request.Cookies["KR_REMEMBER"] != null)
                {
                    HttpCookie authCookie = new HttpCookie("KR_REMEMBER", "");
                    authCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(authCookie);
                }
            }

            // 7. Route based on returnUrl or UserRole
            string returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) && 
                !returnUrl.Contains("Login.aspx") && 
                !returnUrl.Contains("VerifyOtp.aspx") && 
                !returnUrl.Contains("ForgotPassword.aspx"))
            {
                Response.Redirect(returnUrl);
                return;
            }

            // Role-based Dashboard redirection
            RedirectToRoleDashboard(role);
        }

        private void RedirectToRoleDashboard(string role)
        {
            if (role == "Farmer")
            {
                Response.Redirect("FarmerDashboard.aspx");
            }
            else if (role == "Buyer")
            {
                Response.Redirect("BuyerDashboard.aspx");
            }
            else if (role == "Operator")
            {
                Response.Redirect("TractorRentals.aspx");
            }
            else if (role == "Admin")
            {
                Response.Redirect("AdminAnalytics.aspx");
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }

        private void ShowError(string message)
        {
            lblLoginMsg.Text = string.Format(
                "<div class='alert alert-danger d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' role='alert'>" +
                "<i class='bi bi-exclamation-triangle-fill fs-5 me-2 flex-shrink-0 text-danger'></i>" +
                "<div>{0}</div></div>",
                message);
            lblLoginMsg.CssClass = "d-block mb-3";
        }
    }
}
