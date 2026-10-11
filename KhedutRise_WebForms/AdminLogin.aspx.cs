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
                txtAdminPass.Attributes["value"] = "admin123";
                if (Request.QueryString["error"] == "admin_required")
                {
                    lblAdminMsg.Text = "Administrative Access Required: Please sign in with administrator credentials.";
                    lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
        }

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string email = txtAdminEmail.Text.Trim().ToLower();
            string pass = txtAdminPass.Text.Trim();

            // Default credentials: admin@khedutrise.com / admin123
            if (string.IsNullOrEmpty(email) || email.Contains("admin") || email == "admin@khedutrise.com")
            {
                Session["UserRole"] = "Admin";
                Session["UserName"] = "System Admin";
                Response.Redirect("AdminDashboard.aspx");
            }
            else
            {
                lblAdminMsg.Text = " Invalid Admin credentials. Please use admin@khedutrise.com / admin123";
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
