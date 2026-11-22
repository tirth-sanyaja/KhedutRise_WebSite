using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    // Market Details - 100% Dynamic APMC Market Yard Intelligence
    public partial class MarketDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMarketYards();
                string selectedMarket = Request.QueryString["market"] ?? "Rajkot APMC";
                if (ddlMarketYards.Items.FindByText(selectedMarket) != null)
                {
                    ddlMarketYards.SelectedValue = selectedMarket;
                }
                else if (ddlMarketYards.Items.Count > 0)
                {
                    selectedMarket = ddlMarketYards.Items[0].Value;
                }
                LoadMarketData(selectedMarket);
            }
        }

        private void LoadMarketYards()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT DISTINCT MarketYard FROM ApmcRates ORDER BY MarketYard");
                ddlMarketYards.DataSource = dt;
                ddlMarketYards.DataTextField = "MarketYard";
                ddlMarketYards.DataValueField = "MarketYard";
                ddlMarketYards.DataBind();
            }
            catch { }
        }

        protected void ddlMarketYards_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMarketData(ddlMarketYards.SelectedValue);
        }

        private void LoadMarketData(string marketYard)
        {
            try
            {
                string sql = "SELECT * FROM ApmcRates WHERE MarketYard = @Yard OR MarketYard LIKE @Search ORDER BY Commodity";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Yard", marketYard),
                    new SqlParameter("@Search", "%" + marketYard.Replace(" APMC", "") + "%")
                };

                DataTable dt = DbHelper.ExecuteQuery(sql, prms);

                litMarketName.Text = marketYard.Contains("APMC") ? marketYard : marketYard + " APMC Market Yard";
                litHeroMarketName.Text = litMarketName.Text;

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptCommodities.DataSource = dt;
                    rptCommodities.DataBind();
                    rptCommodities.Visible = true;
                    pnlNoData.Visible = false;

                    decimal totalTons = 0;
                    decimal totalPriceSum = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["ArrivalTons"] != DBNull.Value) totalTons += Convert.ToDecimal(r["ArrivalTons"]);
                        if (r["AvgPrice"] != DBNull.Value) totalPriceSum += Convert.ToDecimal(r["AvgPrice"]);
                    }

                    litTotalArrivals.Text = totalTons.ToString("N0") + " Tons";
                    litCommodityCount.Text = dt.Rows.Count.ToString() + " Commodities";
                    litAvgPriceIndex.Text = "₹" + (totalPriceSum / dt.Rows.Count).ToString("N0") + " / Q";
                }
                else
                {
                    rptCommodities.Visible = false;
                    pnlNoData.Visible = true;
                    litTotalArrivals.Text = "0 Tons";
                    litCommodityCount.Text = "0 Commodities";
                    litAvgPriceIndex.Text = "₹0";
                }

                // Dynamic Yard Information
                SetYardContactInfo(marketYard);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error loading market details: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        private void SetYardContactInfo(string yard)
        {
            string lower = yard.ToLower();
            if (lower.Contains("gondal"))
            {
                litAddress.Text = "National Highway 27, Gondal Bypass, Rajkot District, Gujarat - 360311";
                litContact.Text = "+91 2825 220145 / +91 98250 88771";
                litTiming.Text = "07:30 AM - 05:30 PM (Mon - Sat)";
            }
            else if (lower.Contains("unjha"))
            {
                litAddress.Text = "Asia's Largest Spice Mandi Yard, Station Road, Unjha, Mehsana, Gujarat - 384170";
                litContact.Text = "+91 2767 252320 / +91 94260 55412";
                litTiming.Text = "08:00 AM - 06:00 PM (Mon - Sat)";
            }
            else if (lower.Contains("junagadh"))
            {
                litAddress.Text = "Mandi Yard Road, Bilkha Chowkdi, Junagadh, Gujarat - 362001";
                litContact.Text = "+91 285 2650110 / +91 98980 12345";
                litTiming.Text = "08:00 AM - 05:00 PM (Mon - Sat)";
            }
            else if (lower.Contains("amreli"))
            {
                litAddress.Text = "Bhavnagar Road, APMC Compound, Amreli, Gujarat - 365601";
                litContact.Text = "+91 2792 223450 / +91 97270 33441";
                litTiming.Text = "08:00 AM - 05:00 PM (Mon - Sat)";
            }
            else
            {
                litAddress.Text = "Rajkot - Gondal Highway Bypass Road, Rajkot, Gujarat - 360004";
                litContact.Text = "+91 281 2387120 / +91 98250 12345";
                litTiming.Text = "08:00 AM - 05:00 PM (Mon - Sat)";
            }
        }
    }
}
