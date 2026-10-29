using System;
using System.Data;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    // Fresh From Farmers - 100% Dynamic Database Marketplace
    public partial class Marketplace : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string initialCat = Request.QueryString["cat"] != null ? Request.QueryString["cat"].Trim() : "";
                ViewState["ActiveCategory"] = initialCat;
                UpdateCategoryChipsStyle(initialCat);
                LoadProducts();
            }
        }

        // Handle category tab clicks (All, Groundnut, Cotton, Wheat, Vegetables, Spices)
        protected void FilterCategory_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string categoryName = btn.CommandArgument != null ? btn.CommandArgument.Trim() : "";
            ViewState["ActiveCategory"] = categoryName;

            UpdateCategoryChipsStyle(categoryName);
            LoadProducts();
        }

        // Search & Filter handlers
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtLocationFilter.Text = "";
            ddlSort.SelectedIndex = 0;
            ViewState["ActiveCategory"] = "";
            UpdateCategoryChipsStyle("");
            LoadProducts();
        }

        private void UpdateCategoryChipsStyle(string activeCategory)
        {
            LinkButton[] buttons = new LinkButton[] { btnAll, btnGroundnut, btnCotton, btnWheat, btnVegetables, btnSpices };
            foreach (var b in buttons)
            {
                if (b != null)
                {
                    bool isActive = (string.IsNullOrEmpty(activeCategory) && string.IsNullOrEmpty(b.CommandArgument)) ||
                                    (!string.IsNullOrEmpty(activeCategory) && string.Equals(b.CommandArgument, activeCategory, StringComparison.OrdinalIgnoreCase));

                    if (isActive)
                    {
                        b.CssClass = "category-chip fw-bold text-white shadow-sm";
                        b.Style["background-color"] = "#0b6028";
                        b.Style["border"] = "1px solid #0b6028";
                        b.Style["color"] = "#ffffff";
                    }
                    else
                    {
                        b.CssClass = "category-chip fw-semibold bg-white text-dark";
                        b.Style["border"] = "1px solid #d1d5db";
                        b.Style["background-color"] = "#ffffff";
                        b.Style["color"] = "#212529";
                    }
                }
            }
        }

        // Load products dynamically from database via ProductRepository
        private void LoadProducts()
        {
            try
            {
                string category = ViewState["ActiveCategory"] != null ? ViewState["ActiveCategory"].ToString() : "";
                string location = txtLocationFilter.Text.Trim();
                string sort = ddlSort.SelectedValue;

                // Sync category buttons state
                UpdateCategoryChipsStyle(category);

                DataTable dt = ProductRepository.GetMarketplaceProducts(category, location, sort);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptProducts.Visible = true;
                    pnlNoProducts.Visible = false;
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();

                    lblProductCount.Text = string.Format("<i class='bi bi-box-seam me-1'></i> {0} Available Crop{1}", 
                        dt.Rows.Count, dt.Rows.Count > 1 ? "s" : "");
                    lblProductCount.CssClass = "badge bg-success-subtle text-success border border-success-subtle px-3 py-2 fw-bold rounded-pill";
                }
                else
                {
                    rptProducts.Visible = false;
                    pnlNoProducts.Visible = true;
                    lblProductCount.Text = "<i class='bi bi-inbox me-1'></i> 0 Crops Found";
                    lblProductCount.CssClass = "badge bg-secondary-subtle text-secondary border px-3 py-2 fw-bold rounded-pill";
                }
            }
            catch (Exception)
            {
                rptProducts.Visible = false;
                pnlNoProducts.Visible = true;
                lblProductCount.Text = "<i class='bi bi-exclamation-circle me-1'></i> Error Loading Catalog";
                lblProductCount.CssClass = "badge bg-danger-subtle text-danger border px-3 py-2 fw-bold rounded-pill";
            }
        }

        // Return exact fallback image matching product category and commodity
        public string GetFallbackImage(object categoryObj, object nameObj)
        {
            string cat = (categoryObj ?? "").ToString().Trim();
            string name = (nameObj ?? "").ToString().ToLower();

            if (cat == "Cotton") return "Images/cotton.jpg";
            if (cat == "Wheat") return "Images/wheat.jpg";
            if (cat == "Groundnut") return "Images/groundnut.jpg";
            if (cat == "Vegetables")
            {
                if (name.Contains("onion") || name.Contains("ડુંગળી")) return "Images/onion.jpg";
                if (name.Contains("chilli") || name.Contains("chili") || name.Contains("મરચા") || name.Contains("okra") || name.Contains("bhindi")) return "Images/green_chilli.jpg";
                return "Images/tomato.jpg";
            }
            if (cat == "Spices") return "Images/cumin_spices.jpg";
            return "Images/groundnut.jpg";
        }

        // Return distinct badge style per category
        public string GetCategoryBadgeClass(object categoryObj)
        {
            string cat = (categoryObj ?? "").ToString().Trim();
            switch (cat)
            {
                case "Cotton":
                    return "bg-info-subtle text-info-emphasis border border-info-subtle";
                case "Wheat":
                    return "bg-warning-subtle text-warning-emphasis border border-warning-subtle";
                case "Groundnut":
                    return "bg-primary-subtle text-primary border border-primary-subtle";
                case "Vegetables":
                    return "bg-success-subtle text-success border border-success-subtle";
                case "Spices":
                    return "bg-danger-subtle text-danger border border-danger-subtle";
                default:
                    return "bg-secondary-subtle text-secondary border border-secondary-subtle";
            }
        }
    }
}
