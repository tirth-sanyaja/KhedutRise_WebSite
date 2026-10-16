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

        // Load products from database with SQL LEFT JOIN on Users table
        private void LoadProducts(string category)
        {
            try
            {
                // Relational SQL JOIN between Products and Users (via Foreign Key FarmerId)
                string sql = @"SELECT p.ProductId, p.ProductName, p.Category, p.QuantityInQuintal, p.PricePerQuintal, 
                                      p.Grade, p.IsApproved, p.CreatedDate, p.FarmerId,
                                      ISNULL(u.FullName, p.FarmerName) AS FarmerName, 
                                      ISNULL(u.Location, p.Location) AS Location, 
                                      u.MobileNo AS FarmerContact 
                               FROM Products p 
                               LEFT JOIN Users u ON p.FarmerId = u.UserId 
                               WHERE p.IsApproved = 1";

                DataTable dt;
                if (!string.IsNullOrEmpty(category))
                {
                    sql += " AND p.Category = @Category";
                    System.Data.SqlClient.SqlParameter[] prms = new System.Data.SqlClient.SqlParameter[]
                    {
                        new System.Data.SqlClient.SqlParameter("@Category", category)
                    };
                    dt = DbHelper.ExecuteQuery(sql, prms);
                }
                else
                {
                    dt = DbHelper.ExecuteQuery(sql);
                }

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
