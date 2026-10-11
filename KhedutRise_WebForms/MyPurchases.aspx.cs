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

        // READ: Fetch all buyer purchases from Orders table
        private void LoadPurchases()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT * FROM Orders ORDER BY OrderId DESC");
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
