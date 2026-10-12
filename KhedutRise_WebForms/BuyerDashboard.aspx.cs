using System;
using System.Data;

namespace KhedutRise
{
    //  Buyer Dashboard - Overview of orders and purchase metrics
    public partial class BuyerDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        // READ: Fetch recent 4 orders from database
        private void LoadOrders()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT TOP 4 * FROM Orders ORDER BY OrderId DESC");
                if (dt.Rows.Count > 0)
                {
                    rptRecentOrders.DataSource = dt;
                    rptRecentOrders.DataBind();
                }
            }
            catch { }
        }
    }
}
