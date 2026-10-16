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
                string sql = @"SELECT f.PlotId, f.PlotName, f.AreaAcres, f.Crop, f.Stage, f.Status, f.SoilType, f.FarmerId,
                                      ISNULL(u.FullName, 'Registered Farmer') AS FarmerName, 
                                      ISNULL(u.Location, 'Gujarat Farm') AS Village
                               FROM FarmPlots f
                               LEFT JOIN Users u ON f.FarmerId = u.UserId
                               ORDER BY f.PlotId ASC";

                DataTable dt = DbHelper.ExecuteQuery(sql);
                if (dt.Rows.Count > 0)
                {
                    rptPlots.DataSource = dt;
                    rptPlots.DataBind();
                }
            }
            catch { }
        }

        // CREATE: Add a new farmland plot to the database
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

            try
            {
                string sql = "INSERT INTO FarmPlots (PlotName, AreaAcres, Crop, Stage, Status) VALUES (@Name, @Area, @Crop, @Stage, 'Healthy')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", plotName),
                    new SqlParameter("@Area", area),
                    new SqlParameter("@Crop", crop),
                    new SqlParameter("@Stage", stage)
                };
                DbHelper.ExecuteNonQuery(sql, prms);

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
