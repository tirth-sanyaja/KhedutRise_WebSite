using System;

namespace KhedutRise
{
    // Role selection logic that establishes role workflow
    public partial class SelectRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Allow immediate role activation for rapid demonstration and workflow switching
            string role = Request.QueryString["role"];
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Farmer")
                {
                    Response.Redirect("RegisterFarmer.aspx");
                }
                else if (role == "Buyer")
                {
                    Response.Redirect("RegisterBuyer.aspx");
                }
                else if (role == "Operator")
                {
                    Response.Redirect("RegisterOperator.aspx");
                }
                else if (role == "Admin")
                {
                    Response.Redirect("AdminLogin.aspx");
                }
            }
        }
    }
}
