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
                if (string.IsNullOrEmpty(txtAdminEmail.Text))
                {
                    txtAdminEmail.Text = "admin@khedutrise.com";
                }
                txtAdminPass.Attributes["value"] = "admin123";

                if (Request.QueryString["error"] == "admin_required")
                {
                    lblAdminMsg.Text = "Administrative Access Required: Please sign in with administrator credentials.";
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

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string email = (txtAdminEmail.Text ?? "").Trim().ToLower();
            string pass = txtAdminPass.Text.Trim();

            // Fallback for password textbox in ASP.NET WebForms postback
            if (string.IsNullOrEmpty(pass))
            {
                pass = (Request.Form[txtAdminPass.UniqueID] ?? "").Trim();
            }

            // Sync custom reset password if submitted
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

            // Accept standard administrator credentials
            bool isEmailValid = (email == "admin@khedutrise.com" || email == "admin");
            bool isPassValid = (pass == "admin123" || pass == "11111111" || pass == "admin" || (!string.IsNullOrEmpty(customPass) && pass == customPass));

            if (isEmailValid && isPassValid)
            {
                Session["UserRole"] = "Admin";
                Session["UserName"] = "System Admin";
                Session["UserEmail"] = "admin@khedutrise.com";
                Response.Redirect("AdminDashboard.aspx");
            }
            else
            {
                lblAdminMsg.Text = "Invalid Admin credentials. Please use admin@khedutrise.com with password 'admin123' or '11111111'.";
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
