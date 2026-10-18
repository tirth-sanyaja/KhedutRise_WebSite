using System;
using System.Data;

namespace KhedutRise
{
    public partial class MyBookings : System.Web.UI.Page
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
                LoadUserBookings();
            }
        }

        private void LoadUserBookings()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                DataTable dt = EquipmentRepository.GetUserBookings(userId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Compute dynamic KPI metrics
                    object sumObj = dt.Compute("SUM(Amount)", "");
                    decimal totalSpent = sumObj != DBNull.Value ? Convert.ToDecimal(sumObj) : 0;
                    DataRow[] activeRows = dt.Select("Status = 'Confirmed'");
                    int activeCount = activeRows.Length;

                    litUpcomingCount.Text = activeCount.ToString() + " Scheduled";
                    litTotalSpent.Text = "₹" + totalSpent.ToString("N0");

                    // Populate top active booking card if an active booking exists
                    if (activeRows.Length > 0)
                    {
                        DataRow top = activeRows[0];
                        pnlActiveBooking.Visible = true;
                        litActiveTitle.Text = top["ServiceType"].ToString();
                        DateTime bDate = Convert.ToDateTime(top["BookingDate"]);
                        litActiveDetails.Text = "Scheduled Date: " + bDate.ToString("dd MMM yyyy") + " | Provider: " + top["ProviderName"].ToString();
                        litActiveStatus.Text = "Status: " + top["Status"].ToString() + " | Total: ₹" + Convert.ToDecimal(top["Amount"]).ToString("N0");
                        string img = top["ImagePath"] != DBNull.Value ? top["ImagePath"].ToString() : "Images/massey_tractor.jpg";
                        imgActiveBooking.ImageUrl = ResolveUrl(img);
                    }
                    else
                    {
                        pnlActiveBooking.Visible = false;
                    }

                    rptBookings.DataSource = dt;
                    rptBookings.DataBind();
                    divBookingsTable.Visible = true;
                    pnlNoBookings.Visible = false;
                }
                else
                {
                    litUpcomingCount.Text = "0 Scheduled";
                    litTotalSpent.Text = "₹0";
                    pnlActiveBooking.Visible = false;
                    divBookingsTable.Visible = false;
                    pnlNoBookings.Visible = true;
                }
            }
            catch (Exception)
            {
                litUpcomingCount.Text = "0 Scheduled";
                litTotalSpent.Text = "₹0";
                pnlActiveBooking.Visible = false;
                divBookingsTable.Visible = false;
                pnlNoBookings.Visible = true;
            }
        }
    }
}
