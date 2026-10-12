using System;
using System.Data;
using System.Web.UI.WebControls;

namespace KhedutRise
{
    //  Admin Dashboard - Overview of platform statistics and user approvals/rejections
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["FilterStatus"] = "Pending";
                LoadRegistrations("Pending");
                UpdateKpis();
                UpdateFilterButtonStyles("Pending");
            }
        }

        // Fetch users based on selected status filter
        private void LoadRegistrations(string statusFilter)
        {
            try
            {
                string sql = "SELECT * FROM Users";
                if (statusFilter == "Pending")
                {
                    sql += " WHERE Status = 'Pending'";
                }
                else if (statusFilter == "Verified")
                {
                    sql += " WHERE Status = 'Verified' OR Status = 'Approved'";
                }
                else if (statusFilter == "Rejected")
                {
                    sql += " WHERE Status = 'Rejected'";
                }
                sql += " ORDER BY UserId DESC";

                DataTable dt = DbHelper.ExecuteQuery(sql);
                rptPending.DataSource = dt;
                rptPending.DataBind();
            }
            catch (Exception ex)
            {
                lblActionMsg.Text = "Error loading registrations: " + ex.Message;
                pnlActionAlert.Visible = true;
                pnlActionAlert.CssClass = "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
            }
        }

        // Calculate and display live counts
        private void UpdateKpis()
        {
            try
            {
                DataTable dtApproved = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Users WHERE Status = 'Verified' OR Status = 'Approved'");
                if (dtApproved.Rows.Count > 0)
                {
                    lblApprovedCount.Text = dtApproved.Rows[0][0].ToString();
                }

                DataTable dtPendingRejected = DbHelper.ExecuteQuery("SELECT COUNT(*) FROM Users WHERE Status = 'Pending' OR Status = 'Rejected'");
                if (dtPendingRejected.Rows.Count > 0)
                {
                    lblPendingRejectedCount.Text = dtPendingRejected.Rows[0][0].ToString();
                }
            }
            catch { }
        }

        // Handle Status Filter Tabs
        protected void FilterStatus_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string filter = btn.CommandArgument;
            ViewState["FilterStatus"] = filter;
            LoadRegistrations(filter);
            UpdateFilterButtonStyles(filter);
        }

        // Highlight selected filter button
        private void UpdateFilterButtonStyles(string currentFilter)
        {
            btnFilterAll.CssClass = (currentFilter == "ALL") ? "btn btn-dark fw-bold" : "btn btn-outline-secondary";
            btnFilterPending.CssClass = (currentFilter == "Pending") ? "btn btn-warning text-dark fw-bold" : "btn btn-outline-warning";
            btnFilterApproved.CssClass = (currentFilter == "Verified") ? "btn btn-success text-white fw-bold" : "btn btn-outline-success";
            btnFilterRejected.CssClass = (currentFilter == "Rejected") ? "btn btn-danger text-white fw-bold" : "btn btn-outline-danger";
        }

        // Handle Approve and Reject commands
        protected void rptPending_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            string filter = ViewState["FilterStatus"] != null ? ViewState["FilterStatus"].ToString() : "ALL";

            try
            {
                // Retrieve user name for clear feedback
                string userName = "User #" + userId;
                DataTable dtUser = DbHelper.ExecuteQuery("SELECT FullName, UserRole FROM Users WHERE UserId = " + userId);
                if (dtUser.Rows.Count > 0)
                {
                    userName = dtUser.Rows[0]["FullName"].ToString() + " (" + dtUser.Rows[0]["UserRole"].ToString() + ")";
                }

                if (e.CommandName == "Approve")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Verified' WHERE UserId = " + userId);
                    lblActionMsg.Text = " Success! " + userName + " has been APPROVED and granted full access.";
                    pnlActionAlert.CssClass = "alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                    pnlActionAlert.Visible = true;
                }
                else if (e.CommandName == "Reject")
                {
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Status = 'Rejected' WHERE UserId = " + userId);
                    lblActionMsg.Text = " Notice: " + userName + " has been REJECTED.";
                    pnlActionAlert.CssClass = "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                    pnlActionAlert.Visible = true;
                }

                LoadRegistrations(filter);
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
