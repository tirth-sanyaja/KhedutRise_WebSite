using System;
using System.Web.UI;

namespace KhedutRise
{
    // Sell Harvest - Authenticated Farmer Produce Listing
    public partial class SellHarvest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Enforce authentication
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx?error=auth_required&returnUrl=" + Server.UrlEncode("SellHarvest.aspx"));
                    return;
                }

                string role = Session["UserRole"] != null ? Session["UserRole"].ToString() : "";
                if (role != "Farmer" && role != "Admin")
                {
                    ShowError("Only registered Farmers can list crops for sale. Your active role is: " + role);
                    btnSaveCrop.Enabled = false;
                    return;
                }

                if (Session["UserName"] != null)
                {
                    txtFarmer.Text = Session["UserName"].ToString();
                    txtFarmer.ReadOnly = true;
                }
            }
        }

        // CREATE: Save new harvest listing with authenticated FarmerId
        protected void btnSaveCrop_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?error=auth_required&returnUrl=" + Server.UrlEncode("SellHarvest.aspx"));
                return;
            }

            int farmerId = Convert.ToInt32(Session["UserId"]);
            string farmerName = Session["UserName"] != null ? Session["UserName"].ToString() : txtFarmer.Text.Trim();

            string name = txtProdName.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string location = txtLocation.Text.Trim();
            string qtyStr = txtQty.Text.Trim();
            string priceStr = txtPrice.Text.Trim();
            string grade = ddlGrade.SelectedValue;

            // 1. Server-side validation
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                ShowError("Please enter a valid crop name (minimum 2 characters).");
                return;
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                ShowError("Please enter your farm village / district location.");
                return;
            }

            decimal qty;
            if (!decimal.TryParse(qtyStr, out qty) || qty <= 0)
            {
                ShowError("Please enter a valid positive harvest quantity in Quintals.");
                return;
            }

            decimal price;
            if (!decimal.TryParse(priceStr, out price) || price <= 0)
            {
                ShowError("Please enter a valid price per Quintal in INR.");
                return;
            }

            // Select appropriate image path by category
            string imagePath = "Images/groundnut_harvest.jpg";
            if (category == "Cotton") imagePath = "Images/green_rows.jpg";
            else if (category == "Wheat") imagePath = "Images/hero_farmer.jpg";
            else if (category == "Spices") imagePath = "Images/groundnut_harvest.jpg";

            // 2. Insert into database using ProductRepository
            try
            {
                int newProductId = ProductRepository.AddProduct(farmerId, farmerName, name, category, qty, price, location, grade, imagePath);

                if (newProductId > 0)
                {
                    lblMsg.Text = "🎉 Crop Listed Successfully! Your crop <b>" + Server.HtmlEncode(name) + "</b> is now active and live on the <a href='Marketplace.aspx' class='alert-link fw-bold text-decoration-underline'>Marketplace</a>.";
                    lblMsg.CssClass = "alert alert-success d-block p-3 text-center mb-3 fs-6";

                    // Reset form
                    txtProdName.Text = "";
                    txtQty.Text = "";
                    txtPrice.Text = "";
                }
                else
                {
                    ShowError("Failed to publish crop listing. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Error saving crop listing: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblMsg.Text = "❌ " + msg;
            lblMsg.CssClass = "alert alert-danger d-block p-3 text-center mb-3";
        }
    }
}
