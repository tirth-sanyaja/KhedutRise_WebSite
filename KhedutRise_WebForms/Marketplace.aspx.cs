using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    // Fresh From Farmers - Marketplace products list with category filters
    public partial class Marketplace : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts("");
            }
        }

        // Handle category tab clicks (Groundnut, Cotton, Wheat, Vegetables)
        protected void FilterCategory_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string categoryName = btn.CommandArgument;
            LoadProducts(categoryName);
        }

        // Load products from database
        private void LoadProducts(string category)
        {
            try
            {
                string sql = "SELECT * FROM Products WHERE IsApproved = 1";
                if (!string.IsNullOrEmpty(category))
                {
                    sql += " AND Category = '" + category + "'";
                }

                DataTable dt = DbHelper.ExecuteQuery(sql);

                // --- Collections & Generics Examples ---
                // 1. List<T>: Convert DataTable to List of CropItem
                List<CropItem> cropList = KhedutCollectionsService.ConvertDataTableToCropList(dt);

                // 2. Generic Delegate Predicate<T>: Filter items by condition
                List<CropItem> activeCrops = KhedutCollectionsService.FilterCropsByPrice(cropList, c => c.PricePerQuintal > 0);

                // 3. HashSet<T>: Store unique districts
                HashSet<string> districts = KhedutCollectionsService.GetUniqueDistricts(activeCrops);

                // 4. ArrayList: Non-generic collection storing search parameters
                ArrayList filterValues = KhedutCollectionsService.GetSearchFilters(category, 0, true);

                // Bind records to Repeater control
                if (dt.Rows.Count > 0)
                {
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();
                }
            }
            catch { }
        }
    }
}
