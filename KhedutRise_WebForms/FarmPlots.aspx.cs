using System;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // My Farmland GPS & Plots - Manage farm land plots with validation
    public partial class FarmPlots : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPlots();
            }
        }

        // READ: Load farmland plots with SQL LEFT JOIN on Users table (via FarmerId Foreign Key)
        private void LoadPlots()
        {
            try
            {
                int farmerId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
                string sql = @"SELECT f.PlotId, f.PlotName, f.AreaAcres, f.Crop, f.Stage, f.Status, f.SoilType, f.FarmerId,
                                      ISNULL(u.FullName, 'Registered Farmer') AS FarmerName, 
                                      ISNULL(u.Location, 'Gujarat Farm') AS Village
                               FROM FarmPlots f
                               LEFT JOIN Users u ON f.FarmerId = u.UserId";

                DataTable dt;
                if (farmerId > 0)
                {
                    sql += " WHERE f.FarmerId = @FId ORDER BY f.PlotId ASC";
                    SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@FId", farmerId) };
                    dt = DbHelper.ExecuteQuery(sql, prms);
                }
                else
                {
                    sql += " ORDER BY f.PlotId ASC";
                    dt = DbHelper.ExecuteQuery(sql);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptPlots.DataSource = dt;
                    rptPlots.DataBind();
                }
            }
            catch { }
        }

        // CREATE: Add a new farmland plot to the database linked to authenticated FarmerId
        protected void btnAddPlot_Click(object sender, EventArgs e)
        {
            string plotName = txtPlotName.Text.Trim();
            string areaStr = txtArea.Text.Trim();
            string crop = txtCrop.Text.Trim();
            string stage = txtStage.Text.Trim();

            if (string.IsNullOrWhiteSpace(plotName)) return;

            decimal area;
            if (!decimal.TryParse(areaStr, out area) || area <= 0) return;

            if (string.IsNullOrWhiteSpace(crop) || string.IsNullOrWhiteSpace(stage)) return;

            int farmerId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 1001;

            try
            {
                FarmPlotRepository.AddPlot(farmerId, plotName, area, crop, stage, "Clayey Loam");

                txtPlotName.Text = "";
                txtArea.Text = "";
                txtCrop.Text = "";
                txtStage.Text = "";
            }
            catch { }

            LoadPlots();
        }
    }
}
