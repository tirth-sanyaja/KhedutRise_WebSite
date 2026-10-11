using System;
using System.Data;

namespace KhedutRise
{
    // My Bookings & Service History - View past machinery and service logs
    public partial class MyBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLogs();
            }
        }

        // READ: Fetch all booking records from database
        private void LoadLogs()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT * FROM Bookings ORDER BY BookingDate DESC");
                if (dt.Rows.Count > 0)
                {
                    rptBookings.DataSource = dt;
                    rptBookings.DataBind();
                }
            }
            catch { }
        }
    }
}
