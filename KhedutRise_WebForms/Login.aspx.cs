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

            string role = "Farmer";
            string userName = "Sanyaja Tirth";

            if (rbBuyer.Checked)
            {
                role = "Buyer";
                userName = "Dev Patel";
            }
            else if (rbOperator.Checked)
            {
                role = "Operator";
                userName = "Dilipbhai Parmar";
            }

            // Check if user exists in database with this mobile number
            try
            {
                SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Mob", mob) };
                DataTable dt = DbHelper.ExecuteQuery("SELECT TOP 1 FullName, UserRole FROM Users WHERE MobileNo = @Mob", prms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FullName"] != DBNull.Value && !string.IsNullOrWhiteSpace(dt.Rows[0]["FullName"].ToString()))
                    {
                        userName = dt.Rows[0]["FullName"].ToString();
                    }
                    if (dt.Rows[0]["UserRole"] != DBNull.Value && !string.IsNullOrWhiteSpace(dt.Rows[0]["UserRole"].ToString()))
                    {
                        role = dt.Rows[0]["UserRole"].ToString();
                    }
                }
            }
            catch
            {
                // Fallback to selected role
            }

            Session["UserRole"] = role;
            Session["UserName"] = userName;
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
