using System;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Page 19: Sell Harvest - Product listing with complete validation
    public partial class SellHarvest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                {
                    txtFarmer.Text = Session["UserName"].ToString();
                }
            }
        }

        // CREATE & UPDATE: Save new harvest listing
        protected void btnSaveCrop_Click(object sender, EventArgs e)
        {
            string name = txtProdName.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string farmer = txtFarmer.Text.Trim();
            string location = txtLocation.Text.Trim();
            string qtyStr = txtQty.Text.Trim();
            string priceStr = txtPrice.Text.Trim();
            string grade = ddlGrade.SelectedValue;

            // 1. Validation checks
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                ShowError("Please enter a valid crop or variety name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(farmer))
            {
                ShowError("Please enter farmer or seller name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                ShowError("Please enter farm location.");
                return;
            }

            decimal qty;
            if (!decimal.TryParse(qtyStr, out qty) || qty <= 0)
            {
                ShowError("Please enter a valid positive quantity in quintals.");
                return;
            }

            decimal price;
            if (!decimal.TryParse(priceStr, out price) || price <= 0)
            {
                ShowError("Please enter a valid expected price per quintal.");
                return;
            }

            // 2. Database Insertion
            try
            {
                string query = @"INSERT INTO Products (ProductName, Category, QuantityInQuintal, PricePerQuintal, FarmerName, Location, Grade, IsApproved)
                                 VALUES (@Name, @Cat, @Qty, @Price, @Farmer, @Loc, @Grade, 1)";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Cat", category),
                    new SqlParameter("@Qty", qty),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@Farmer", farmer),
                    new SqlParameter("@Loc", location),
                    new SqlParameter("@Grade", grade)
                };
                DbHelper.ExecuteNonQuery(query, prms);

                lblMsg.Text = "✅ Success! Crop listing for <b>" + name + "</b> has been published to the Marketplace!";
                lblMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";

                // Clear fields
                txtProdName.Text = "";
                txtQty.Text = "";
                txtPrice.Text = "";
            }
            catch (Exception ex)
            {
                ShowError("Failed to publish crop: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblMsg.Text = "❌ " + msg;
            lblMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
