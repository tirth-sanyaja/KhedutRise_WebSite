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

        // READ: Fetch recent 4 orders using SQL JOIN (Orders + Users + Products)
        private void LoadOrders()
        {
            try
            {
                // Multi-table SQL JOIN via Foreign Keys
                string sql = @"SELECT TOP 4 
                                      o.OrderId, o.TotalPrice, o.OrderDate, o.Status, o.Quantity,
                                      ISNULL(p.ProductName, o.Commodity) AS Commodity,
                                      ISNULL(uFarmer.FullName, o.FarmerName) AS FarmerName,
                                      ISNULL(uBuyer.FullName, o.BuyerName) AS BuyerName
                               FROM Orders o
                               LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
                               LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
                               LEFT JOIN Products p ON o.ProductId = p.ProductId
                               ORDER BY o.OrderId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql);
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
