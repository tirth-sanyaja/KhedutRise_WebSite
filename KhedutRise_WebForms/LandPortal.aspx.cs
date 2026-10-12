using System;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Land Portal - List and browse agricultural land with validation
    public partial class LandPortal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLandListings();
            }
        }

        private void LoadLandListings()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT * FROM LandListings WHERE Status = 'Approved' OR Status = 'Verified' ORDER BY LandId DESC");
                if (dt.Rows.Count > 0)
                {
                    rptLandListings.DataSource = dt;
                    rptLandListings.DataBind();
                }
            }
            catch { }
        }

        protected void btnAddLand_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string areaStr = txtArea.Text.Trim();
            string price = txtPrice.Text.Trim();
            string location = txtLocation.Text.Trim();
            string soil = ddlSoil.SelectedValue;
            string water = ddlWater.SelectedValue;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(price))
            {
                lblLandMsg.Text = " Please fill out all required land details.";
                lblLandMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            decimal area;
            if (!decimal.TryParse(areaStr, out area) || area <= 0)
            {
                lblLandMsg.Text = " Please specify valid land area in acres.";
                lblLandMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            try
            {
                string sql = @"INSERT INTO LandListings (Title, AreaAcres, Location, WaterSource, Price, SoilType, Status) 
                               VALUES (@Title, @Area, @Loc, @Water, @Price, @Soil, 'Approved')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Area", area),
                    new SqlParameter("@Loc", location),
                    new SqlParameter("@Water", water),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@Soil", soil)
                };
                DbHelper.ExecuteNonQuery(sql, prms);

                lblLandMsg.Text = "Land listing published successfully!";
                lblLandMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";

                txtTitle.Text = "";
                txtArea.Text = "";
                txtPrice.Text = "";
                txtLocation.Text = "";
                LoadLandListings();
            }
            catch (Exception ex)
            {
                lblLandMsg.Text = "Error publishing land: " + ex.Message;
                lblLandMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
