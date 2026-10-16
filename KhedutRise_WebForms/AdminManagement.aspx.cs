using System;
using System.Data;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    //  Platform Management & Moderation - Real Approve/Reject/Delete workflow across all fields
    public partial class AdminManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

        // 1. Fetch Users
        private void LoadUsers(string roleCategory, string statusFilter)
        {
            try
            {
                string sql = "SELECT * FROM Users WHERE 1=1";
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

                DataTable dt = DbHelper.ExecuteQuery(sql);
                rptModeration.DataSource = dt;
                rptModeration.DataBind();

                // Compute counts
                string roleClause = "";
                if (roleCategory == "Farmer") roleClause = " WHERE UserRole = 'Farmer'";
                else if (roleCategory == "Buyer") roleClause = " WHERE UserRole = 'Buyer'";

                string wherePrefix = string.IsNullOrEmpty(roleClause) ? " WHERE " : roleClause + " AND ";

                DataTable dtTotal = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Users" + roleClause);
                DataTable dtApp = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Users" + wherePrefix + "(Status = 'Verified' OR Status = 'Approved')");
                DataTable dtPend = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Users" + wherePrefix + "Status = 'Pending'");
                DataTable dtRej = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Users" + wherePrefix + "Status = 'Rejected'");

                lblTotalCount.Text = dtTotal.Rows[0][0].ToString();
                lblApprovedCount.Text = dtApp.Rows[0][0].ToString();
                lblPendingCount.Text = dtPend.Rows[0][0].ToString();
                lblRejectedCount.Text = dtRej.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading users: " + ex.Message, false);
            }
        }

        // 2. Fetch Crop Products with SQL LEFT JOIN on Users (via FarmerId Foreign Key)
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

                DataTable dtTotal = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Products");
                DataTable dtApp = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Products WHERE IsApproved = 1");
                DataTable dtRej = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Products WHERE IsApproved = 0");

                lblTotalCount.Text = dtTotal.Rows[0][0].ToString();
                lblApprovedCount.Text = dtApp.Rows[0][0].ToString();
                lblPendingCount.Text = "0";
                lblRejectedCount.Text = dtRej.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                ShowAlert("Error loading products: " + ex.Message, false);
            }
        }

        // 3. Fetch Land Listings with SQL LEFT JOIN on Users (via SellerId Foreign Key)
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

                DataTable dtTotal = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM LandListings");
                DataTable dtApp = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM LandListings WHERE Status = 'Approved' OR Status = 'Verified'");
                DataTable dtPend = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM LandListings WHERE Status = 'Pending'");
                DataTable dtRej = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM LandListings WHERE Status = 'Rejected'");

                lblTotalCount.Text = dtTotal.Rows[0][0].ToString();
                lblApprovedCount.Text = dtApp.Rows[0][0].ToString();
                lblPendingCount.Text = dtPend.Rows[0][0].ToString();
                lblRejectedCount.Text = dtRej.Rows[0][0].ToString();
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
                DataTable dt = DbHelper.ExecuteQuery("SELECT FullName, UserRole FROM Users WHERE UserId = " + userId);
                if (dt.Rows.Count > 0)
                {
                    userName = dt.Rows[0]["FullName"].ToString() + " (" + dt.Rows[0]["UserRole"].ToString() + ")";
                }

                if (e.CommandName == "Approve")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Verified' WHERE UserId = " + userId);
                    ShowAlert(" APPROVED: Account for <b>" + userName + "</b> has been approved successfully!", true);
                }
                else if (e.CommandName == "Reject")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Rejected' WHERE UserId = " + userId);
                    ShowAlert(" REJECTED: Account for <b>" + userName + "</b> has been marked as Rejected.", false);
                }
                else if (e.CommandName == "DeleteUser")
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM Users WHERE UserId = " + userId);
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
                DataTable dt = DbHelper.ExecuteQuery("SELECT ProductName FROM Products WHERE ProductId = " + productId);
                if (dt.Rows.Count > 0) pName = dt.Rows[0]["ProductName"].ToString();

                if (e.CommandName == "ApproveProduct")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Products SET IsApproved = 1 WHERE ProductId = " + productId);
                    ShowAlert(" APPROVED: Crop listing <b>" + pName + "</b> is now approved and visible in the Marketplace!", true);
                }
                else if (e.CommandName == "RejectProduct")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Products SET IsApproved = 0 WHERE ProductId = " + productId);
                    ShowAlert(" REJECTED: Crop listing <b>" + pName + "</b> has been rejected.", false);
                }
                else if (e.CommandName == "DeleteProduct")
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM Products WHERE ProductId = " + productId);
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
                DataTable dt = DbHelper.ExecuteQuery("SELECT Title FROM LandListings WHERE LandId = " + landId);
                if (dt.Rows.Count > 0) title = dt.Rows[0]["Title"].ToString();

                if (e.CommandName == "ApproveLand")
                {
                    DbHelper.ExecuteNonQuery("UPDATE LandListings SET Status = 'Approved' WHERE LandId = " + landId);
                    ShowAlert(" APPROVED: Land plot <b>" + title + "</b> has been approved!", true);
                }
                else if (e.CommandName == "RejectLand")
                {
                    DbHelper.ExecuteNonQuery("UPDATE LandListings SET Status = 'Rejected' WHERE LandId = " + landId);
                    ShowAlert(" REJECTED: Land plot <b>" + title + "</b> has been rejected.", false);
                }
                else if (e.CommandName == "DeleteLand")
                {
                    DbHelper.ExecuteNonQuery("DELETE FROM LandListings WHERE LandId = " + landId);
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

        private void UpdatePillStyles(string cat)
        {
            string activeStyle = "btn btn-sm rounded-pill px-4 fw-bold text-white shadow-none";
            string inactiveStyle = "btn btn-sm rounded-pill px-4 fw-semibold shadow-none bg-white text-dark";

            btnFarmers.CssClass = (cat == "Farmer") ? activeStyle : inactiveStyle;
            btnFarmers.Style["background-color"] = (cat == "Farmer") ? "#0b6028" : "#ffffff";
            btnFarmers.Style["border"] = (cat == "Farmer") ? "1px solid #0b6028" : "1px solid #d1d5db";

            btnBuyers.CssClass = (cat == "Buyer") ? activeStyle : inactiveStyle;
            btnBuyers.Style["background-color"] = (cat == "Buyer") ? "#0b6028" : "#ffffff";
            btnBuyers.Style["border"] = (cat == "Buyer") ? "1px solid #0b6028" : "1px solid #d1d5db";

            btnModeration.CssClass = (cat == "Moderation") ? activeStyle : inactiveStyle;
            btnModeration.Style["background-color"] = (cat == "Moderation") ? "#0b6028" : "#ffffff";
            btnModeration.Style["border"] = (cat == "Moderation") ? "1px solid #0b6028" : "1px solid #d1d5db";

            btnProducts.CssClass = (cat == "Products") ? activeStyle : inactiveStyle;
            btnProducts.Style["background-color"] = (cat == "Products") ? "#0b6028" : "#ffffff";
            btnProducts.Style["border"] = (cat == "Products") ? "1px solid #0b6028" : "1px solid #d1d5db";

            btnLandListings.CssClass = (cat == "LandListings") ? activeStyle : inactiveStyle;
            btnLandListings.Style["background-color"] = (cat == "LandListings") ? "#0b6028" : "#ffffff";
            btnLandListings.Style["border"] = (cat == "LandListings") ? "1px solid #0b6028" : "1px solid #d1d5db";
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
