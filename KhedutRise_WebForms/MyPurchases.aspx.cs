using System;
using System.Data;

namespace KhedutRise
{
    // My Purchases - View buyer orders and escrow status
    public partial class MyPurchases : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPurchases();
            }
        }

        // READ: Fetch all buyer purchases using multi-table SQL JOIN (Orders + Users + Products)
        private void LoadPurchases()
        {
            try
            {
                // Multi-table Relational SQL JOIN via Foreign Keys (FarmerId, BuyerId, ProductId)
                string sql = @"SELECT o.OrderId, o.TotalPrice, o.OrderDate, o.Status, o.Quantity,
                                      ISNULL(p.ProductName, o.Commodity) AS Commodity,
                                      ISNULL(uFarmer.FullName, o.FarmerName) AS FarmerName,
                                      ISNULL(uBuyer.FullName, o.BuyerName) AS BuyerName,
                                      uFarmer.MobileNo AS FarmerContact
                               FROM Orders o
                               LEFT JOIN Users uFarmer ON o.FarmerId = uFarmer.UserId
                               LEFT JOIN Users uBuyer ON o.BuyerId = uBuyer.UserId
                               LEFT JOIN Products p ON o.ProductId = p.ProductId
                               ORDER BY o.OrderId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql);
                if (dt.Rows.Count > 0)
                {
                    rptPurchases.DataSource = dt;
                    rptPurchases.DataBind();
                }
            }
            catch { }
        }
    }
}
