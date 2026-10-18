using System;
using System.Data;

namespace KhedutRise
{
    public partial class FarmerDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            int farmerId = Convert.ToInt32(Session["UserId"]);
            string name = Session["UserName"] != null ? Session["UserName"].ToString() : "Farmer";
            litFarmerGreeting.Text = name;

            if (!IsPostBack)
            {
                LoadFarmerMetrics(farmerId);
            }
        }

        private void LoadFarmerMetrics(int farmerId)
        {
            try
            {
                DataTable dt = AnalyticsRepository.GetFarmerDashboardMetrics(farmerId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];

                    decimal acres = r["TotalAcres"] != DBNull.Value ? Convert.ToDecimal(r["TotalAcres"]) : 0;
                    litTotalAcres.Text = acres.ToString("0.#") + " Acres";

                    string crop = r["CurrentCrop"] != DBNull.Value ? r["CurrentCrop"].ToString() : "Mixed Crops";
                    litCurrentCrop.Text = crop;

                    int orders = r["ActiveOrders"] != DBNull.Value ? Convert.ToInt32(r["ActiveOrders"]) : 0;
                    litActiveOrders.Text = orders.ToString() + " Active";

                    int bookings = r["TotalBookings"] != DBNull.Value ? Convert.ToInt32(r["TotalBookings"]) : 0;
                    litUpcomingBookings.Text = bookings.ToString() + " Bookings";

                    decimal revenue = r["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(r["TotalRevenue"]) : 0;
                    litTotalRevenue.Text = "₹" + revenue.ToString("N0");
                }
            }
            catch (Exception)
            {
                // Fallback to graceful defaults
                litTotalAcres.Text = "0 Acres";
                litCurrentCrop.Text = "Mixed Crops";
                litActiveOrders.Text = "0 Active";
                litUpcomingBookings.Text = "0 Bookings";
                litTotalRevenue.Text = "₹0";
            }
        }
    }
}
