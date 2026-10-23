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
                ViewState["ActiveCategory"] = "";
                LoadProducts();
            }
        }

        // Handle category tab clicks (All, Groundnut, Cotton, Wheat,Vegetables)
        protected void FilterCategory_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string categoryName = btn.CommandArgument;
            ViewState["ActiveCategory"] = categoryName;

            // Highlight selected chip
            ResetCategoryStyles();
            btn.CssClass = "btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none";
            btn.Style["background-color"] = "#0b6028";
            btn.Style["border"] = "1px solid #0b6028";

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
            txtSearch.Text = "";
            txtLocationFilter.Text = "";
            ddlSort.SelectedIndex = 0;
            ViewState["ActiveCategory"] = "";
            ResetCategoryStyles();
            btnAll.CssClass = "btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none";
            btnAll.Style["background-color"] = "#0b6028";
            LoadProducts();
        }

        private void ResetCategoryStyles()
        {
            LinkButton[] buttons = new LinkButton[] { btnAll, btnGroundnut, btnCotton, btnWheat, btnVegetables };
            foreach (var b in buttons)
            {
                if (b != null)
                {
                    b.CssClass = "btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark";
                    b.Style["border"] = "1px solid #d1d5db";
                    b.Style["background-color"] = "#ffffff";
                }
            }
        }

        // Load products dynamically from database via ProductRepository
        private void LoadProducts()
        {
            try
            {
                string category = ViewState["ActiveCategory"] != null ? ViewState["ActiveCategory"].ToString() : "";
                string search = txtSearch.Text.Trim();
                string location = txtLocationFilter.Text.Trim();
                if (!string.IsNullOrEmpty(location))
                {
                    search = string.IsNullOrEmpty(search) ? location : (search + " " + location);
                }
                string sort = ddlSort.SelectedValue;

                DataTable dt = ProductRepository.GetMarketplaceProducts(category, search, sort);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptProducts.Visible = true;
                    pnlNoProducts.Visible = false;
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();
                }
                else
                {
                    rptProducts.Visible = false;
                    pnlNoProducts.Visible = true;
                }
            }
            catch (Exception ex)
            {
                rptProducts.Visible = false;
                pnlNoProducts.Visible = true;
            }
        }

        // Return exact fallback image matching product category and commodity
        public string GetFallbackImage(object categoryObj, object nameObj)
        {
            string cat = (categoryObj ?? "").ToString().Trim();
            string name = (nameObj ?? "").ToString().ToLower();

            if (cat == "Cotton") return "Images/cotton.jpg";
            if (cat == "Wheat") return "Images/wheat.jpg";
            if (cat == "Vegetables")
            {
                if (name.Contains("onion") || name.Contains("ડુંગળી")) return "Images/onion.jpg";
                if (name.Contains("chilli") || name.Contains("મરચા") || name.Contains("okra")) return "Images/green_chilli.jpg";
                return "Images/tomato.jpg";
            }
            if (cat == "Spices") return "Images/cumin_spices.jpg";
            return "Images/groundnut.jpg";
        }
    }
}
