using System;

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
            if (string.IsNullOrWhiteSpace(mob) || mob.Length < 10)
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

            if (string.IsNullOrWhiteSpace(mob) || mob.Length < 10)
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

            CompleteLogin();
        }

        private void CompleteLogin()
        {
            string returnUrl = Request.QueryString["returnUrl"];

            if (rbBuyer.Checked)
            {
                Session["UserRole"] = "Buyer";
                Session["UserName"] = "Dev Patel";
                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.EndsWith(".aspx") && returnUrl != "Default.aspx")
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    Response.Redirect("BuyerDashboard.aspx");
                }
            }
            else if (rbOperator.Checked)
            {
                Session["UserRole"] = "Operator";
                Session["UserName"] = "Dilipbhai Parmar";
                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.EndsWith(".aspx") && returnUrl != "Default.aspx")
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    Response.Redirect("TractorRentals.aspx");
                }
            }
            else
            {
                Session["UserRole"] = "Farmer";
                Session["UserName"] = "Sanyaja Tirth";
                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.EndsWith(".aspx") && returnUrl != "Default.aspx")
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    Response.Redirect("FarmerDashboard.aspx");
                }
            }
        }
    }
}
