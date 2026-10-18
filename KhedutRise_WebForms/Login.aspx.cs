using System;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    //  User Login - Simple and clean Mobile OTP authentication with role-based routing
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["error"] == "auth_required")
                {
                    lblLoginMsg.Text = "Authentication Required: Please sign in to access portal features.";
                    lblLoginMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                }
            }
        }

        // Send simulated OTP to user mobile
        protected void btnSendOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("MobileAuth");
            if (!Page.IsValid)
            {
                return;
            }

            string mob = txtMobile.Text.Trim();
            long parsedMob;
            if (string.IsNullOrWhiteSpace(mob) || mob.Length != 10 || !long.TryParse(mob, out parsedMob))
            {
                lblLoginMsg.Text = "Please enter a valid 10-digit mobile number before requesting OTP.";
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            lblLoginMsg.Text = "OTP sent to +91 " + mob + "! Your verification OTP is: <b>482190</b>";
            lblLoginMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
            txtOtp.Text = "482190";
        }

        // Establish user role session and navigate to respective dashboard via OTP
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

            long parsedMob;
            if (string.IsNullOrWhiteSpace(mob) || mob.Length != 10 || !long.TryParse(mob, out parsedMob))
            {
                lblLoginMsg.Text = "Please enter a valid 10-digit mobile number.";
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(otp) || otp.Length < 4)
            {
                lblLoginMsg.Text = "Please enter a valid OTP.";
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            CompleteLogin(mob);
        }

        private void CompleteLogin(string mob)
        {
            string returnUrl = Request.QueryString["returnUrl"];
            string selectedRole = "Farmer";
            if (rbBuyer.Checked) selectedRole = "Buyer";
            else if (rbOperator.Checked) selectedRole = "Operator";

            int userId = 0;
            string userName = "";
            string role = selectedRole;

            try
            {
                SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Mob", mob) };
                DataTable dt = DbHelper.ExecuteQuery("SELECT TOP 1 UserId, FullName, UserRole, Location FROM Users WHERE MobileNo = @Mob", prms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    userId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    userName = dt.Rows[0]["FullName"].ToString();
                    role = dt.Rows[0]["UserRole"].ToString();
                }
                else
                {
                    // If user is first time signing in with mobile OTP, create real user record in Users table
                    userName = selectedRole == "Buyer" ? "Registered Buyer" : (selectedRole == "Operator" ? "Machinery Operator" : "Registered Farmer");
                    userId = UserRepository.RegisterUser(userName, mob, mob + "@khedutrise.com", selectedRole, "Gujarat", "Agriculture", "482190");
                }
            }
            catch (Exception ex)
            {
                lblLoginMsg.Text = "Login error: " + ex.Message;
                lblLoginMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            // Establish secure server-side session values with real database primary key
            Session["UserId"] = userId;
            Session["UserName"] = userName;
            Session["UserRole"] = role;
            Session["UserMobile"] = mob;

            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.EndsWith(".aspx") && returnUrl != "Default.aspx")
            {
                Response.Redirect(returnUrl);
            }
            else if (role == "Buyer")
            {
                Response.Redirect("BuyerDashboard.aspx");
            }
            else if (role == "Operator")
            {
                Response.Redirect("TractorRentals.aspx");
            }
            else
            {
                Response.Redirect("FarmerDashboard.aspx");
            }
        }
    }
}
