using System;
using System.Collections.Generic;
using System.Data;

namespace KhedutRise
{
    // Page 12: Live APMC Market Rates - Real-time market rates and search filter
    public partial class ApmcRates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRates("");
            }
        }

        // Search commodity on text change
        protected void txtSearchCommodity_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchCommodity.Text.Trim();
            BindRates(keyword);
        }

        // Fetch APMC rates from database
        private void BindRates(string filter)
        {
            try
            {
                // 1. Generic Dictionary<TKey, TValue>: Live market rates lookup
                Dictionary<string, decimal> mandiRates = KhedutCollectionsService.GetLiveMandiRates();

                // 2. Non-Generic Hashtable: English to Gujarati translation
                string gujaratiName = KhedutCollectionsService.TranslateCropName(filter);

                string sql = "SELECT * FROM ApmcRates";
                System.Data.SqlClient.SqlParameter[] prms = null;
                if (!string.IsNullOrEmpty(filter))
                {
                    sql += " WHERE Commodity LIKE @Filter OR MarketYard LIKE @Filter";
                    prms = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@Filter", "%" + filter + "%") };
                }

                DataTable dt = DbHelper.ExecuteQuery(sql, prms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvApmc.DataSource = dt;
                    gvApmc.DataBind();
                }
                else
                {
                    gvApmc.DataSource = null;
                    gvApmc.DataBind();
                }
            }
            catch { }
        }
    }
}
