using System;
using System.Data;

namespace KhedutRise
{
    public partial class AdminAnalytics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce administrator role security
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("AdminLogin.aspx?error=admin_required");
                return;
            }

            if (!IsPostBack)
            {
                litGeneratedTime.Text = "Telemetry Generated: " + DateTime.Now.ToString("dd MMM yyyy, HH:mm");
                LoadAnalyticsData();
            }
        }

        private void LoadAnalyticsData()
        {
            try
            {
                // 1. Fetch Executive Analytics via CTEs
                DataTable dtStats = AnalyticsRepository.GetPlatformAnalytics();
                if (dtStats != null && dtStats.Rows.Count > 0)
                {
                    DataRow r = dtStats.Rows[0];

                    decimal totalGmv = r["TotalOrderValue"] != DBNull.Value ? Convert.ToDecimal(r["TotalOrderValue"]) : 0;
                    litTotalGmv.Text = "₹" + totalGmv.ToString("N0");

                    int totalOrders = r["TotalOrders"] != DBNull.Value ? Convert.ToInt32(r["TotalOrders"]) : 0;
                    int activeOrders = r["ActiveOrders"] != DBNull.Value ? Convert.ToInt32(r["ActiveOrders"]) : 0;
                    litTotalOrders.Text = totalOrders.ToString() + " Orders";
                    litActiveOrdersCount.Text = activeOrders.ToString() + " In-Flight Escrow";

                    int totalUsers = r["TotalUsers"] != DBNull.Value ? Convert.ToInt32(r["TotalUsers"]) : 0;
                    int farmers = r["FarmerCount"] != DBNull.Value ? Convert.ToInt32(r["FarmerCount"]) : 0;
                    int buyers = r["BuyerCount"] != DBNull.Value ? Convert.ToInt32(r["BuyerCount"]) : 0;
                    int operators = r["OperatorCount"] != DBNull.Value ? Convert.ToInt32(r["OperatorCount"]) : 0;

                    litTotalUsers.Text = totalUsers.ToString() + " Accounts";
                    litDemographicsTotal.Text = totalUsers.ToString();
                    litFarmerCountText.Text = farmers.ToString() + " (" + CalculatePct(farmers, totalUsers) + "%)";
                    litBuyerCountText.Text = buyers.ToString() + " (" + CalculatePct(buyers, totalUsers) + "%)";
                    litOperatorCountText.Text = operators.ToString() + " (" + CalculatePct(operators, totalUsers) + "%)";

                    // Progress bar HTML
                    int pctFarmer = totalUsers > 0 ? (farmers * 100) / totalUsers : 0;
                    int pctBuyer = totalUsers > 0 ? (buyers * 100) / totalUsers : 0;
                    int pctOperator = totalUsers > 0 ? (operators * 100) / totalUsers : 0;

                    litProgressBar.Text = string.Format(
                        "<div class='progress-bar bg-success' role='progressbar' style='width: {0}%;' title='Farmers: {0}%'></div>" +
                        "<div class='progress-bar bg-primary' role='progressbar' style='width: {1}%;' title='Buyers: {1}%'></div>" +
                        "<div class='progress-bar bg-warning' role='progressbar' style='width: {2}%;' title='Operators: {2}%'></div>",
                        pctFarmer, pctBuyer, pctOperator);

                    decimal totalRentals = r["TotalBookingRevenue"] != DBNull.Value ? Convert.ToDecimal(r["TotalBookingRevenue"]) : 0;
                    int totalBookings = r["TotalBookings"] != DBNull.Value ? Convert.ToInt32(r["TotalBookings"]) : 0;
                    litTotalRentals.Text = "₹" + totalRentals.ToString("N0");
                    litBookingsCount.Text = totalBookings.ToString() + " Machinery Bookings";
                }

                // 2. Fetch Category Breakdown with SQL Window Function
                DataTable dtCategories = AnalyticsRepository.GetCategoryBreakdown();
                if (dtCategories != null && dtCategories.Rows.Count > 0)
                {
                    rptCategoryBreakdown.DataSource = dtCategories;
                    rptCategoryBreakdown.DataBind();
                }

                // 3. Fetch Recent Platform Orders
                DataTable dtOrders = AnalyticsRepository.GetRecentPlatformOrders(5);
                if (dtOrders != null && dtOrders.Rows.Count > 0)
                {
                    rptRecentOrders.DataSource = dtOrders;
                    rptRecentOrders.DataBind();
                }
            }
            catch (Exception ex)
            {
                litTotalGmv.Text = "₹0";
                litTotalOrders.Text = "0 Orders";
                litTotalUsers.Text = "0 Accounts";
                litTotalRentals.Text = "₹0";
            }
        }

        private string CalculatePct(int part, int total)
        {
            if (total == 0) return "0";
            return ((part * 100) / total).ToString();
        }
    }
}
