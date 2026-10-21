using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    // Platform Management & Moderation - Clean approve, reject, and delete workflows
    public partial class AdminManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce administrator role security
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("AdminLogin.aspx?error=admin_required");
                return;
            }

            if (!IsPostBack)
            {
                string tabQuery = Request.QueryString["tab"];
                if (string.Equals(tabQuery, "Farmers", StringComparison.OrdinalIgnoreCase))
                {
                    ViewState["ActiveCategory"] = "Farmer";
                }
                else if (string.Equals(tabQuery, "Buyers", StringComparison.OrdinalIgnoreCase))
                {
                    ViewState["ActiveCategory"] = "Buyer";
                }
                else if (string.Equals(tabQuery, "Products", StringComparison.OrdinalIgnoreCase))
                {
                    ViewState["ActiveCategory"] = "Products";
                }
                else if (string.Equals(tabQuery, "LandListings", StringComparison.OrdinalIgnoreCase))
                {
                    ViewState["ActiveCategory"] = "LandListings";
                }
                else
                {
                    ViewState["ActiveCategory"] = "Moderation";
                }

                ViewState["ActiveStatus"] = "ALL";
                LoadCurrentCategory();
            }
        }

        // Sidebar Navigation
        protected void NavSidebar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string target = btn.CommandArgument;
            if (target == "Farmers")
            {
                ViewState["ActiveCategory"] = "Farmer";
            }
            else if (target == "Buyers")
            {
                ViewState["ActiveCategory"] = "Buyer";
            }
            else
            {
                ViewState["ActiveCategory"] = "Moderation";
            }

            ViewState["ActiveStatus"] = "ALL";
            LoadCurrentCategory();
        }

        // Top Filter Category Pills
        protected void TabFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["ActiveCategory"] = btn.CommandArgument;
            ViewState["ActiveStatus"] = "ALL";
            LoadCurrentCategory();
        }

        // Status Filter Buttons (All, Approved, Pending, Rejected)
        protected void StatusFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ViewState["ActiveStatus"] = btn.CommandArgument;
            LoadCurrentCategory();
        }

        // Central loader based on current category and status
        private void LoadCurrentCategory()
        {
            string category = ViewState["ActiveCategory"] != null ? ViewState["ActiveCategory"].ToString() : "Moderation";
            string status = ViewState["ActiveStatus"] != null ? ViewState["ActiveStatus"].ToString() : "ALL";

            UpdatePillStyles(category);
            UpdateStatusButtonStyles(status);
            UpdateSidebarStyles(category);

            if (category == "Products")
            {
                pnlUsersView.Visible = false;
                pnlLandView.Visible = false;
                pnlProductsView.Visible = true;
                litBreadcrumb.Text = "Crops Moderation";
                litPageTitle.Text = "Crop Listings Moderation";
                LoadProducts(status);
            }
            else if (category == "LandListings")
            {
                pnlUsersView.Visible = false;
                pnlProductsView.Visible = false;
                pnlLandView.Visible = true;
                litBreadcrumb.Text = "Land Listings";
                litPageTitle.Text = "Land Listings Moderation";
                LoadLandListings(status);
            }
            else
            {
                pnlProductsView.Visible = false;
                pnlLandView.Visible = false;
                pnlUsersView.Visible = true;

                if (category == "Farmer")
                {
                    litBreadcrumb.Text = "Farmers Management";
                    litPageTitle.Text = "Registered Farmers Directory & Moderation";
                    litTableHeading.Text = "Farmers Directory";
                }
                else if (category == "Buyer")
                {
                    litBreadcrumb.Text = "Buyers Management";
                    litPageTitle.Text = "Registered Buyers Directory & Moderation";
                    litTableHeading.Text = "Buyers Directory";
                }
                else
                {
                    litBreadcrumb.Text = "Platform Moderation";
                    litPageTitle.Text = "Account Approvals & Moderation";
                    litTableHeading.Text = "Pending & Active Accounts";
                }

                LoadUsers(category, status);
            }
        }

        // 1. Fetch Users & Counts in single queries
        private void LoadUsers(string roleCategory, string statusFilter)
        {
            try
            {
                string sql = "SELECT * FROM Users WHERE 1=1";
                List<SqlParameter> prms = new List<SqlParameter>();

                if (roleCategory == "Farmer")
                {
                    sql += " AND UserRole = 'Farmer'";
                }
                else if (roleCategory == "Buyer")
                {
                    sql += " AND UserRole = 'Buyer'";
                }

                if (statusFilter == "Approved")
                {
                    sql += " AND (Status = 'Verified' OR Status = 'Approved')";
                }
                else if (statusFilter == "Pending")
                {
                    sql += " AND Status = 'Pending'";
                }
                else if (statusFilter == "Rejected")
                {
                    sql += " AND Status = 'Rejected'";
                }
                sql += " ORDER BY UserId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql, prms.Count > 0 ? prms.ToArray() : null);
                rptModeration.DataSource = dt;
                rptModeration.DataBind();

                // Compute counts in a single efficient query
                string roleFilterClause = "";
                if (roleCategory == "Farmer") roleFilterClause = " WHERE UserRole = 'Farmer'";
                else if (roleCategory == "Buyer") roleFilterClause = " WHERE UserRole = 'Buyer'";

                string countSql = string.Format(@"
                    SELECT 
                        COUNT(*) AS TotalCount,
                        ISNULL(SUM(CASE WHEN Status IN ('Verified', 'Approved') THEN 1 ELSE 0 END), 0) AS ApprovedCount,
                        ISNULL(SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END), 0) AS PendingCount,
                        ISNULL(SUM(CASE WHEN Status = 'Rejected' THEN 1 ELSE 0 END), 0) AS RejectedCount
                    FROM Users {0}", roleFilterClause);

                DataTable dtCounts = DbHelper.ExecuteQuery(countSql);
                if (dtCounts != null && dtCounts.Rows.Count > 0)
                {
                    DataRow r = dtCounts.Rows[0];
                    lblTotalCount.Text = r["TotalCount"].ToString();
                    lblApprovedCount.Text = r["ApprovedCount"].ToString();
                    lblPendingCount.Text = r["PendingCount"].ToString();
                    lblRejectedCount.Text = r["RejectedCount"].ToString();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading users: " + ex.Message, false);
            }
        }

        // 2. Fetch Crop Products with SQL LEFT JOIN on Users
        private void LoadProducts(string statusFilter)
        {
            try
            {
                string sql = @"SELECT p.ProductId, p.ProductName, p.Category, p.QuantityInQuintal, p.PricePerQuintal, 
                                      p.Grade, p.IsApproved, p.CreatedDate, p.FarmerId,
                                      ISNULL(u.FullName, p.FarmerName) AS FarmerName, 
                                      ISNULL(u.Location, p.Location) AS Location,
                                      u.MobileNo AS FarmerContact 
                               FROM Products p 
                               LEFT JOIN Users u ON p.FarmerId = u.UserId 
                               WHERE 1=1";
                if (statusFilter == "Approved")
                {
                    sql += " AND p.IsApproved = 1";
                }
                else if (statusFilter == "Rejected" || statusFilter == "Pending")
                {
                    sql += " AND p.IsApproved = 0";
                }
                sql += " ORDER BY p.ProductId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql);
                rptProducts.DataSource = dt;
                rptProducts.DataBind();

                // Compute product counts in a single query
                string countSql = @"
                    SELECT 
                        COUNT(*) AS TotalCount,
                        ISNULL(SUM(CASE WHEN IsApproved = 1 THEN 1 ELSE 0 END), 0) AS ApprovedCount,
                        ISNULL(SUM(CASE WHEN IsApproved = 0 THEN 1 ELSE 0 END), 0) AS RejectedCount
                    FROM Products";

                DataTable dtCounts = DbHelper.ExecuteQuery(countSql);
                if (dtCounts != null && dtCounts.Rows.Count > 0)
                {
                    DataRow r = dtCounts.Rows[0];
                    lblTotalCount.Text = r["TotalCount"].ToString();
                    lblApprovedCount.Text = r["ApprovedCount"].ToString();
                    lblPendingCount.Text = "0";
                    lblRejectedCount.Text = r["RejectedCount"].ToString();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading products: " + ex.Message, false);
            }
        }

        // 3. Fetch Land Listings with SQL LEFT JOIN on Users
        private void LoadLandListings(string statusFilter)
        {
            try
            {
                string sql = @"SELECT l.LandId, l.Title, l.AreaAcres, l.Location, l.WaterSource, l.Price, l.SoilType, l.Status, l.SellerId,
                                      ISNULL(u.FullName, 'Farmer Seller') AS SellerName, 
                                      u.MobileNo AS SellerContact 
                               FROM LandListings l 
                               LEFT JOIN Users u ON l.SellerId = u.UserId 
                               WHERE 1=1";
                if (statusFilter == "Approved")
                {
                    sql += " AND (l.Status = 'Approved' OR l.Status = 'Verified')";
                }
                else if (statusFilter == "Pending")
                {
                    sql += " AND l.Status = 'Pending'";
                }
                else if (statusFilter == "Rejected")
                {
                    sql += " AND l.Status = 'Rejected'";
                }
                sql += " ORDER BY l.LandId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql);
                rptLand.DataSource = dt;
                rptLand.DataBind();

                // Compute land listing counts in a single query
                string countSql = @"
                    SELECT 
                        COUNT(*) AS TotalCount,
                        ISNULL(SUM(CASE WHEN Status IN ('Approved', 'Verified') THEN 1 ELSE 0 END), 0) AS ApprovedCount,
                        ISNULL(SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END), 0) AS PendingCount,
                        ISNULL(SUM(CASE WHEN Status = 'Rejected' THEN 1 ELSE 0 END), 0) AS RejectedCount
                    FROM LandListings";

                DataTable dtCounts = DbHelper.ExecuteQuery(countSql);
                if (dtCounts != null && dtCounts.Rows.Count > 0)
                {
                    DataRow r = dtCounts.Rows[0];
                    lblTotalCount.Text = r["TotalCount"].ToString();
                    lblApprovedCount.Text = r["ApprovedCount"].ToString();
                    lblPendingCount.Text = r["PendingCount"].ToString();
                    lblRejectedCount.Text = r["RejectedCount"].ToString();
                }
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading land listings: " + ex.Message, false);
            }
        }

        // Item Commands for Users
        protected void rptModeration_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            try
            {
                string userName = "User #" + userId;
                SqlParameter[] selPrms = new SqlParameter[] { new SqlParameter("@Id", userId) };
                DataTable dt = DbHelper.ExecuteQuery("SELECT FullName, UserRole FROM Users WHERE UserId = @Id", selPrms);
                if (dt.Rows.Count > 0)
                {
                    userName = dt.Rows[0]["FullName"].ToString() + " (" + dt.Rows[0]["UserRole"].ToString() + ")";
                }

                SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Id", userId) };

                if (e.CommandName == "Approve")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Verified' WHERE UserId = @Id", prms);
                    ShowAlert(" APPROVED: Account for <b>" + userName + "</b> has been approved successfully!", true);
                }
                else if (e.CommandName == "Reject")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Rejected' WHERE UserId = @Id", prms);
                    ShowAlert(" REJECTED: Account for <b>" + userName + "</b> has been marked as Rejected.", false);
                }
                else if (e.CommandName == "DeleteUser")
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM Users WHERE UserId = @Id", prms);
                    ShowAlert(" DELETED: Account for <b>" + userName + "</b> was permanently removed.", false);
                }

                LoadCurrentCategory();
            }
            catch (Exception ex)
            {
                ShowAlert("Operation error: " + ex.Message, false);
            }
        }

        // Item Commands for Products
        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);
            try
            {
                string pName = "Product #" + productId;
                SqlParameter[] selPrms = new SqlParameter[] { new SqlParameter("@Id", productId) };
                DataTable dt = DbHelper.ExecuteQuery("SELECT ProductName FROM Products WHERE ProductId = @Id", selPrms);
                if (dt.Rows.Count > 0) pName = dt.Rows[0]["ProductName"].ToString();

                SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Id", productId) };

                if (e.CommandName == "ApproveProduct")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Products SET IsApproved = 1 WHERE ProductId = @Id", prms);
                    ShowAlert(" APPROVED: Crop listing <b>" + pName + "</b> is now approved and visible in the Marketplace!", true);
                }
                else if (e.CommandName == "RejectProduct")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Products SET IsApproved = 0 WHERE ProductId = @Id", prms);
                    ShowAlert(" REJECTED: Crop listing <b>" + pName + "</b> has been rejected.", false);
                }
                else if (e.CommandName == "DeleteProduct")
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM Products WHERE ProductId = @Id", prms);
                    ShowAlert(" DELETED: Crop listing <b>" + pName + "</b> was removed.", false);
                }

                LoadCurrentCategory();
            }
            catch (Exception ex)
            {
                ShowAlert("Operation error: " + ex.Message, false);
            }
        }

        // Item Commands for Land Listings
        protected void rptLand_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int landId = Convert.ToInt32(e.CommandArgument);
            try
            {
                string title = "Land #" + landId;
                SqlParameter[] selPrms = new SqlParameter[] { new SqlParameter("@Id", landId) };
                DataTable dt = DbHelper.ExecuteQuery("SELECT Title FROM LandListings WHERE LandId = @Id", selPrms);
                if (dt.Rows.Count > 0) title = dt.Rows[0]["Title"].ToString();

                SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Id", landId) };

                if (e.CommandName == "ApproveLand")
                {
                    DbHelper.ExecuteNonQuery("UPDATE LandListings SET Status = 'Approved' WHERE LandId = @Id", prms);
                    ShowAlert(" APPROVED: Land plot <b>" + title + "</b> has been approved!", true);
                }
                else if (e.CommandName == "RejectLand")
                {
                    DbHelper.ExecuteNonQuery("UPDATE LandListings SET Status = 'Rejected' WHERE LandId = @Id", prms);
                    ShowAlert(" REJECTED: Land plot <b>" + title + "</b> has been rejected.", false);
                }
                else if (e.CommandName == "DeleteLand")
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM LandListings WHERE LandId = @Id", prms);
                    ShowAlert(" DELETED: Land plot <b>" + title + "</b> was removed.", false);
                }

                LoadCurrentCategory();
            }
            catch (Exception ex)
            {
                ShowAlert("Operation error: " + ex.Message, false);
            }
        }

        private void ShowAlert(string message, bool isSuccess)
        {
            lblAlertMsg.Text = message;
            pnlAlert.CssClass = isSuccess 
                ? "alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm"
                : "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
            pnlAlert.Visible = true;
        }

        private void SetPillStyle(LinkButton btn, bool isActive)
        {
            if (btn == null) return;
            btn.CssClass = isActive 
                ? "btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none" 
                : "btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark";
            btn.Style["background-color"] = isActive ? "#0b6028" : "#ffffff";
            btn.Style["border"] = isActive ? "1px solid #0b6028" : "1px solid #d1d5db";
        }

        private void UpdatePillStyles(string cat)
        {
            SetPillStyle(btnFarmers, cat == "Farmer");
            SetPillStyle(btnBuyers, cat == "Buyer");
            SetPillStyle(btnModeration, cat == "Moderation");
            SetPillStyle(btnProducts, cat == "Products");
            SetPillStyle(btnLandListings, cat == "LandListings");
        }

        private void UpdateStatusButtonStyles(string status)
        {
            btnStatusAll.CssClass = (status == "ALL") ? "btn btn-dark fw-bold" : "btn btn-outline-secondary";
            btnStatusApproved.CssClass = (status == "Approved") ? "btn btn-success text-white fw-bold" : "btn btn-outline-success";
            btnStatusPending.CssClass = (status == "Pending") ? "btn btn-warning text-dark fw-bold" : "btn btn-outline-warning text-dark";
            btnStatusRejected.CssClass = (status == "Rejected") ? "btn btn-danger text-white fw-bold" : "btn btn-outline-danger";
        }

        private void UpdateSidebarStyles(string cat)
        {
            lnkNavFarmers.CssClass = (cat == "Farmer") ? "text-decoration-none fw-bold d-block" : "text-decoration-none text-white d-block opacity-90";
            lnkNavFarmers.Style["color"] = (cat == "Farmer") ? "#22c55e" : "#ffffff";

            lnkNavBuyers.CssClass = (cat == "Buyer") ? "text-decoration-none fw-bold d-block" : "text-decoration-none text-white d-block opacity-90";
            lnkNavBuyers.Style["color"] = (cat == "Buyer") ? "#22c55e" : "#ffffff";

            lnkNavModeration.CssClass = (cat == "Moderation") ? "text-decoration-none fw-bold d-block" : "text-decoration-none text-white d-block opacity-90";
            lnkNavModeration.Style["color"] = (cat == "Moderation") ? "#22c55e" : "#ffffff";
        }
    }
}
