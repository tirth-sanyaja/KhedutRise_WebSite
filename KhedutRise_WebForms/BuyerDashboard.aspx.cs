using System;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Buyer Dashboard - Overview of orders and purchase metrics
    public partial class BuyerDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string name = Session["UserName"] != null ? Session["UserName"].ToString() : "Buyer";
                if (litBuyerGreeting != null)
                {
                    litBuyerGreeting.Text = name;
                }
                LoadOrders();
            }
        }

        // Fetch recent orders for active buyer (or platform recent orders if guest)
        private void LoadOrders()
        {
            try
            {
                int buyerId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
                string sql;
                SqlParameter[] prms = null;

                if (buyerId > 0)
                {
                    sql = @"SELECT TOP 4 
                                  o.OrderId, o.TotalPrice, o.OrderDate, o.Status, o.Quantity,
                                  ISNULL(p.ProductName, o.Commodity) AS Commodity,
                                  ISNULL(uFarmer.FullName, o.FarmerName) AS FarmerName,
                                  ISNULL(uBuyer.FullName, o.BuyerName) AS BuyerName
                           FROM Orders o
                           LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
                           LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
                           LEFT JOIN Products p ON o.ProductId = p.ProductId
                           WHERE o.BuyerId = @BuyerId
                           ORDER BY o.OrderId DESC";
                    prms = new SqlParameter[] { new SqlParameter("@BuyerId", buyerId) };
                }
                else
                {
                    sql = @"SELECT TOP 4 
                                  o.OrderId, o.TotalPrice, o.OrderDate, o.Status, o.Quantity,
                                  ISNULL(p.ProductName, o.Commodity) AS Commodity,
                                  ISNULL(uFarmer.FullName, o.FarmerName) AS FarmerName,
                                  ISNULL(uBuyer.FullName, o.BuyerName) AS BuyerName
                           FROM Orders o
                           LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
                           LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
                           LEFT JOIN Products p ON o.ProductId = p.ProductId
                           ORDER BY o.OrderId DESC";
                }

                DataTable dt = DbHelper.ExecuteQuery(sql, prms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptRecentOrders.DataSource = dt;
                    rptRecentOrders.DataBind();
                }
            }
            catch { }
        }
    }
}
