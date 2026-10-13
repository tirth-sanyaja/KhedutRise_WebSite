using System;

namespace KhedutRise
{
    //  Admin Portal Login - Verifies admin access and establishes admin role
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAdminPass.Attributes["value"] = "11111111";
                if (Request.QueryString["error"] == "admin_required")
                {
                    lblAdminMsg.Text = "Administrative Access Required: Please sign in with administrator credentials.";
                    lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
        }

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string email = txtAdminEmail.Text.Trim().ToLower();
            string pass = txtAdminPass.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                lblAdminMsg.Text = "Please enter both admin email and password.";
                lblAdminMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                return;
            }

            // Default credentials: admin@khedutrise.com / 11111111
            if (email == "admin@khedutrise.com" && pass == "11111111")
            {
                Session["UserRole"] = "Admin";
                Session["UserName"] = "System Admin";
                Response.Redirect("AdminDashboard.aspx");
            }
            else
            {
                lblAdminMsg.Text = "Invalid Admin credentials. Please use admin@khedutrise.com / 11111111";
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
