using System;
using System.Data;

namespace KhedutRise
{
    public partial class MyPurchases : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            if (!IsPostBack)
            {
                LoadPurchases();
            }
        }

        private void LoadPurchases()
        {
            try
            {
                int buyerId = Convert.ToInt32(Session["UserId"]);
                DataTable dt = OrderRepository.GetBuyerOrders(buyerId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptPurchases.DataSource = dt;
                    rptPurchases.DataBind();
                    divPurchasesTable.Visible = true;
                    pnlNoPurchases.Visible = false;
                }
                else
                {
                    divPurchasesTable.Visible = false;
                    pnlNoPurchases.Visible = true;
                }
            }
            catch (Exception)
            {
                divPurchasesTable.Visible = false;
                pnlNoPurchases.Visible = true;
            }
        }
    }
}
