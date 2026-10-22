using System;
using System.Data;

namespace KhedutRise
{
    // My Crops - 100% Dynamic Database-Driven Crop Monitoring for Authenticated Farmer
    public partial class MyCrops : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Enforce authentication
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx?error=auth_required&returnUrl=" + Server.UrlEncode("MyCrops.aspx"));
                    return;
                }

                string role = Session["UserRole"] != null ? Session["UserRole"].ToString() : "";
                if (role != "Farmer" && role != "Admin")
                {
                    Response.Redirect("Default.aspx?msg=farmer_access_only");
                    return;
                }

                int farmerId = Convert.ToInt32(Session["UserId"]);
                LoadFarmerCrops(farmerId);
            }
        }

        // Fetch farmer's own farm plots and cultivated crops from SQL Server
        private void LoadFarmerCrops(int farmerId)
        {
            try
            {
                DataTable dt = FarmPlotRepository.GetFarmerPlots(farmerId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptFarmerCrops.Visible = true;
                    pnlNoCrops.Visible = false;
                    rptFarmerCrops.DataSource = dt;
                    rptFarmerCrops.DataBind();

                    // Calculate live statistics
                    int totalPlots = dt.Rows.Count;
                    decimal totalArea = 0;
                    System.Collections.Generic.HashSet<string> uniqueCrops = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["AreaAcres"] != DBNull.Value)
                        {
                            totalArea += Convert.ToDecimal(r["AreaAcres"]);
                        }
                        if (r["Crop"] != DBNull.Value)
                        {
                            uniqueCrops.Add(r["Crop"].ToString().Trim());
                        }
                    }

                    lblTotalCrops.Text = uniqueCrops.Count.ToString();
                    lblTotalArea.Text = totalArea.ToString("N1");
                    lblActivePlots.Text = totalPlots.ToString();
                    lblNextHarvest.Text = dt.Rows[0]["Crop"].ToString() + " (Active)";
                }
                else
                {
                    rptFarmerCrops.Visible = false;
                    pnlNoCrops.Visible = true;
                    lblTotalCrops.Text = "0";
                    lblTotalArea.Text = "0";
                    lblActivePlots.Text = "0";
                    lblNextHarvest.Text = "No Active Plots";
                }
            }
            catch (Exception ex)
            {
                rptFarmerCrops.Visible = false;
                pnlNoCrops.Visible = true;
            }
        }

        // Return real-world photograph matching the specific crop
        public string GetCropImage(object cropObj)
        {
            if (cropObj == null) return "Images/groundnut.jpg";
            string c = cropObj.ToString().ToLower();
            if (c.Contains("cotton") || c.Contains("કપાસ")) return "Images/cotton.jpg";
            if (c.Contains("wheat") || c.Contains("ઘઉં")) return "Images/wheat.jpg";
            if (c.Contains("tomato") || c.Contains("ટામેટા")) return "Images/tomato.jpg";
            if (c.Contains("onion") || c.Contains("ડુંગળી")) return "Images/onion.jpg";
            if (c.Contains("chilli") || c.Contains("મરચા") || c.Contains("vegetable") || c.Contains("શાકભાજી") || c.Contains("okra")) return "Images/green_chilli.jpg";
            if (c.Contains("cumin") || c.Contains("jeera") || c.Contains("જીરું") || c.Contains("spice")) return "Images/cumin_spices.jpg";
            return "Images/groundnut.jpg";
        }
    }
}
