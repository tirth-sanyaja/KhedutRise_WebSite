using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    // Admin Dashboard - Real-time telemetry, moderation workflow, and ecosystem control
    public partial class AdminDashboard : System.Web.UI.Page
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
                if (litCurrentDate != null)
                {
                    litCurrentDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy");
                }
                ViewState["FilterStatus"] = "ALL";
                LoadRegistrations("ALL");
                UpdateKpis();
                UpdateFilterButtonStyles("ALL");
                LoadRecentInquiries();
            }
        }

        // Fetch users based on selected status filter and optional search term
        private void LoadRegistrations(string statusFilter, string searchTerm = "")
        {
            try
            {
                string sql = "SELECT * FROM Users WHERE 1=1";
                List<SqlParameter> prms = new List<SqlParameter>();

                if (statusFilter == "Pending")
                {
                    sql += " AND Status = 'Pending'";
                }
                else if (statusFilter == "Verified")
                {
                    sql += " AND (Status = 'Verified' OR Status = 'Approved')";
                }
                else if (statusFilter == "Rejected")
                {
                    sql += " AND Status = 'Rejected'";
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    sql += " AND (FullName LIKE @Search OR MobileNo LIKE @Search OR Location LIKE @Search OR UserRole LIKE @Search)";
                    prms.Add(new SqlParameter("@Search", "%" + searchTerm.Trim() + "%"));
                }

                sql += " ORDER BY UserId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql, prms.Count > 0 ? prms.ToArray() : null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptPending.Visible = true;
                    pnlEmptyState.Visible = false;
                    rptPending.DataSource = dt;
                    rptPending.DataBind();
                }
                else
                {
                    rptPending.Visible = false;
                    pnlEmptyState.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblActionMsg.Text = "Error loading registrations: " + ex.Message;
                pnlActionAlert.Visible = true;
                pnlActionAlert.CssClass = "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
            }
        }

        // Calculate and display live counts across the platform using clean consolidated queries
        private void UpdateKpis()
        {
            try
            {
                // 1. Single query for all user status metrics
                string userMetricsSql = @"
                    SELECT 
                        COUNT(*) AS TotalUsers,
                        ISNULL(SUM(CASE WHEN Status IN ('Verified', 'Approved') THEN 1 ELSE 0 END), 0) AS ApprovedUsers,
                        ISNULL(SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END), 0) AS PendingUsers,
                        ISNULL(SUM(CASE WHEN Status = 'Rejected' THEN 1 ELSE 0 END), 0) AS RejectedUsers
                    FROM Users";
                DataTable dtUsers = DbHelper.ExecuteQuery(userMetricsSql);
                if (dtUsers != null && dtUsers.Rows.Count > 0)
                {
                    DataRow r = dtUsers.Rows[0];
                    int totalUsers = Convert.ToInt32(r["TotalUsers"]);
                    int approvedUsers = Convert.ToInt32(r["ApprovedUsers"]);
                    int pendingUsers = Convert.ToInt32(r["PendingUsers"]);
                    int rejectedUsers = Convert.ToInt32(r["RejectedUsers"]);

                    lblTotalUsers.Text = totalUsers.ToString();
                    litAllCount.Text = totalUsers.ToString();

                    lblApprovedCount.Text = approvedUsers.ToString();
                    litVerifiedCount.Text = approvedUsers.ToString();

                    lblPendingCount.Text = pendingUsers.ToString();
                    litPendingCount.Text = pendingUsers.ToString();

                    litRejectedCount.Text = rejectedUsers.ToString();

                    int verRate = totalUsers > 0 ? (int)Math.Round((double)approvedUsers / totalUsers * 100) : 100;
                    lblVerificationRate.Text = verRate + "% Verified";
                }

                // 2. Orders telemetry
                DataTable dtOrders = DbHelper.ExecuteQuery("SELECT COUNT(*), ISNULL(SUM(TotalPrice), 0) FROM Orders");
                if (dtOrders != null && dtOrders.Rows.Count > 0)
                {
                    litDashboardOrders.Text = dtOrders.Rows[0][0].ToString();
                    litDashboardGmv.Text = Convert.ToDecimal(dtOrders.Rows[0][1]).ToString("N0");
                }

                // 3. Consolidated counts for machinery, bookings, and inquiries
                string telemetrySql = @"
                    SELECT 
                        (SELECT COUNT(*) FROM Equipments) AS MachineryCount,
                        (SELECT COUNT(*) FROM Bookings) AS BookingsCount,
                        (SELECT COUNT(*) FROM ContactInquiries) AS InquiriesCount";
                DataTable dtTelemetry = DbHelper.ExecuteQuery(telemetrySql);
                if (dtTelemetry != null && dtTelemetry.Rows.Count > 0)
                {
                    litDashboardMachinery.Text = dtTelemetry.Rows[0]["MachineryCount"].ToString() + " Units";
                    litDashboardBookings.Text = dtTelemetry.Rows[0]["BookingsCount"].ToString();
                    lblInquiriesCount.Text = dtTelemetry.Rows[0]["InquiriesCount"].ToString();
                }
            }
            catch { }
        }

        // Load latest messages from ContactInquiries table
        private void LoadRecentInquiries()
        {
            try
            {
                string sql = "SELECT TOP 4 * FROM ContactInquiries ORDER BY InquiryId DESC";
                DataTable dt = DbHelper.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptInquiries.Visible = true;
                    pnlEmptyInquiries.Visible = false;
                    rptInquiries.DataSource = dt;
                    rptInquiries.DataBind();
                }
                else
                {
                    rptInquiries.Visible = false;
                    pnlEmptyInquiries.Visible = true;
                }
            }
            catch
            {
                rptInquiries.Visible = false;
                pnlEmptyInquiries.Visible = true;
            }
        }

        // Handle Status Filter Tabs
        protected void FilterStatus_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string filter = btn.CommandArgument;
            ViewState["FilterStatus"] = filter;
            string search = txtSearchUser != null ? txtSearchUser.Text.Trim() : "";
            LoadRegistrations(filter, search);
            UpdateFilterButtonStyles(filter);
        }

        // Search triggers
        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            string filter = ViewState["FilterStatus"] != null ? ViewState["FilterStatus"].ToString() : "ALL";
            string search = txtSearchUser.Text.Trim();
            LoadRegistrations(filter, search);
        }

        protected void txtSearchUser_TextChanged(object sender, EventArgs e)
        {
            string filter = ViewState["FilterStatus"] != null ? ViewState["FilterStatus"].ToString() : "ALL";
            string search = txtSearchUser.Text.Trim();
            LoadRegistrations(filter, search);
        }

        // Highlight selected filter button
        private void UpdateFilterButtonStyles(string currentFilter)
        {
            btnFilterAll.CssClass = (currentFilter == "ALL") 
                ? "btn btn-sm btn-dark fw-bold px-3 py-1 shadow-sm rounded-pill" 
                : "btn btn-sm btn-outline-secondary px-3 py-1 rounded-pill";

            btnFilterPending.CssClass = (currentFilter == "Pending") 
                ? "btn btn-sm btn-warning text-dark fw-bold px-3 py-1 shadow-sm rounded-pill" 
                : "btn btn-sm btn-outline-warning text-dark px-3 py-1 rounded-pill";

            btnFilterApproved.CssClass = (currentFilter == "Verified") 
                ? "btn btn-sm btn-success text-white fw-bold px-3 py-1 shadow-sm rounded-pill" 
                : "btn btn-sm btn-outline-success px-3 py-1 rounded-pill";

            btnFilterRejected.CssClass = (currentFilter == "Rejected") 
                ? "btn btn-sm btn-danger text-white fw-bold px-3 py-1 shadow-sm rounded-pill" 
                : "btn btn-sm btn-outline-danger px-3 py-1 rounded-pill";
        }

        // Helper to format 2-letter uppercase initials
        public string GetInitials(object nameObj)
        {
            if (nameObj == null) return "KR";
            string name = nameObj.ToString().Trim();
            if (string.IsNullOrEmpty(name)) return "KR";
            string[] parts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpper();
            return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
        }

        // Helper for user role badge styling
        public string GetRoleBadge(object roleObj)
        {
            string role = (roleObj ?? "").ToString().Trim();
            if (role.Equals("Farmer", StringComparison.OrdinalIgnoreCase))
                return "<span class=\"badge bg-success-subtle text-success border border-success-subtle px-2 py-1\"><i class=\"bi bi-person-fill me-1\"></i>Farmer</span>";
            if (role.Equals("Buyer", StringComparison.OrdinalIgnoreCase))
                return "<span class=\"badge bg-primary-subtle text-primary border border-primary-subtle px-2 py-1\"><i class=\"bi bi-shop me-1\"></i>Buyer</span>";
            if (role.Equals("Operator", StringComparison.OrdinalIgnoreCase))
                return "<span class=\"badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1\"><i class=\"bi bi-truck me-1\"></i>Operator</span>";
            return "<span class=\"badge bg-secondary-subtle text-secondary border border-secondary-subtle px-2 py-1\">" + role + "</span>";
        }

        // Helper for user verification status pill
        public string GetStatusBadge(object statusObj)
        {
            string status = (statusObj ?? "").ToString().Trim();
            if (status.Equals("Verified", StringComparison.OrdinalIgnoreCase) || status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                return "<span class=\"badge rounded-pill bg-success-subtle text-success border border-success-subtle px-2 py-1 fw-semibold\"><i class=\"bi bi-check-circle-fill me-1\"></i>Verified</span>";
            if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                return "<span class=\"badge rounded-pill bg-danger-subtle text-danger border border-danger-subtle px-2 py-1 fw-semibold\"><i class=\"bi bi-x-circle-fill me-1\"></i>Rejected</span>";
            return "<span class=\"badge rounded-pill bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1 fw-semibold\"><i class=\"bi bi-clock-history me-1\"></i>Pending Review</span>";
        }

        // Helper for document name
        public string GetDocName(object roleObj)
        {
            string role = (roleObj ?? "").ToString().Trim();
            if (role.Equals("Farmer", StringComparison.OrdinalIgnoreCase)) return "Land_Record_7_12.pdf";
            if (role.Equals("Buyer", StringComparison.OrdinalIgnoreCase)) return "GST_Trader_Certificate.pdf";
            return "Commercial_RC_Permit.pdf";
        }

        // Handle Approve and Reject commands
        protected void rptPending_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            string filter = ViewState["FilterStatus"] != null ? ViewState["FilterStatus"].ToString() : "ALL";
            string search = txtSearchUser != null ? txtSearchUser.Text.Trim() : "";

            try
            {
                string userName = "User #" + userId;
                SqlParameter[] selPrms = new SqlParameter[] { new SqlParameter("@Id", userId) };
                DataTable dtUser = DbHelper.ExecuteQuery("SELECT FullName, UserRole FROM Users WHERE UserId = @Id", selPrms);
                if (dtUser.Rows.Count > 0)
                {
                    userName = dtUser.Rows[0]["FullName"].ToString() + " (" + dtUser.Rows[0]["UserRole"].ToString() + ")";
                }

                SqlParameter[] updPrms = new SqlParameter[] { new SqlParameter("@Id", userId) };

                if (e.CommandName == "Approve")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Verified' WHERE UserId = @Id", updPrms);
                    lblActionMsg.Text = " <strong>Success!</strong> " + userName + " has been approved and granted active platform status.";
                    pnlActionAlert.CssClass = "alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                    pnlActionAlert.Visible = true;
                }
                else if (e.CommandName == "Reject")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Rejected' WHERE UserId = @Id", updPrms);
                    lblActionMsg.Text = " <strong>Notice:</strong> " + userName + " registration has been marked as Rejected.";
                    pnlActionAlert.CssClass = "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                    pnlActionAlert.Visible = true;
                }

                LoadRegistrations(filter, search);
                UpdateKpis();
                UpdateFilterButtonStyles(filter);
            }
            catch (Exception ex)
            {
                lblActionMsg.Text = "Action failed: " + ex.Message;
                pnlActionAlert.CssClass = "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                pnlActionAlert.Visible = true;
            }
        }
    }
}
