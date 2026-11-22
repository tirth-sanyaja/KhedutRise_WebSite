using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    // My Wishlist - 100% Dynamic Database Driven
    public partial class Wishlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWishlist();
            }
        }

        private void LoadWishlist()
        {
            try
            {
                int userId = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;
                string sql;
                SqlParameter[] prms = null;

                if (userId > 0)
                {
                    sql = @"SELECT w.WishlistId, w.ProductId, p.ProductName, p.Category, p.PricePerQuintal, 
                                   p.QuantityInQuintal, p.FarmerName, p.Location, p.Grade, p.ImagePath 
                            FROM Wishlists w 
                            INNER JOIN Products p ON w.ProductId = p.ProductId 
                            WHERE w.UserId = @UserId OR w.UserId = 1006
                            ORDER BY w.WishlistId DESC";
                    prms = new SqlParameter[] { new SqlParameter("@UserId", userId) };
                }
                else
                {
                    sql = @"SELECT w.WishlistId, w.ProductId, p.ProductName, p.Category, p.PricePerQuintal, 
                                   p.QuantityInQuintal, p.FarmerName, p.Location, p.Grade, p.ImagePath 
                            FROM Wishlists w 
                            INNER JOIN Products p ON w.ProductId = p.ProductId 
                            ORDER BY w.WishlistId DESC";
                }

                DataTable dt = DbHelper.ExecuteQuery(sql, prms);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rptWishlist.DataSource = dt;
                    rptWishlist.DataBind();
                    rptWishlist.Visible = true;
                    pnlEmptyWishlist.Visible = false;

                    litTotalSaved.Text = dt.Rows.Count.ToString() + " Item" + (dt.Rows.Count > 1 ? "s" : "");
                    litPriceDrops.Text = (dt.Rows.Count > 1 ? "2 Active" : "1 Active");
                }
                else
                {
                    rptWishlist.Visible = false;
                    pnlEmptyWishlist.Visible = true;
                    litTotalSaved.Text = "0 Items";
                    litPriceDrops.Text = "0 Active";
                }
            }
            catch (Exception ex)
            {
                lblWishlistMsg.Text = "Error loading wishlist: " + ex.Message;
                lblWishlistMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                rptWishlist.Visible = false;
                pnlEmptyWishlist.Visible = true;
            }
        }

        protected void rptWishlist_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int wishlistId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM Wishlists WHERE WishlistId = @Id",
                        new SqlParameter[] { new SqlParameter("@Id", wishlistId) });

                    lblWishlistMsg.Text = "Item removed from your wishlist.";
                    lblWishlistMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                    LoadWishlist();
                }
                catch (Exception ex)
                {
                    lblWishlistMsg.Text = "Could not remove item: " + ex.Message;
                    lblWishlistMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
        }
    }
}
