using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    //  Agricultural Services - Request and book farming services
    public partial class AgriServices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Pre-fill customer name from session if logged in
                if (Session["UserName"] != null)
                {
                    txtCustomerName.Text = Session["UserName"].ToString();
                }
            }
        }

        protected void btnConfirmService_Click(object sender, EventArgs e)
        {
            // Step 1: Read values from form
            string service = ddlService.SelectedValue;
            string customer = txtCustomerName.Text.Trim();
            string mobile = txtCustomerMobile.Text.Trim();
            string dateLoc = txtDateTime.Text.Trim();

            // Step 2: Validate inputs
            if (string.IsNullOrWhiteSpace(customer) || customer.Length < 2)
            {
                lblServiceMsg.Text = "Please enter your contact name.";
                lblServiceMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length < 10)
            {
                lblServiceMsg.Text = "Please enter a valid 10-digit mobile number.";
                lblServiceMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(dateLoc))
            {
                lblServiceMsg.Text = "Please provide preferred date and farm location.";
                lblServiceMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            try
            {
                // Generate a booking ID
                string bookingId = "#KR-BK-" + new Random().Next(410, 999);

                // Calculate service fee
                decimal fee = 450;
                if (service.Contains("Survey")) fee = 1200;
                else if (service.Contains("Irrigation")) fee = 800;

                // Step 3: Insert booking into database
                string sql = "INSERT INTO Bookings (BookingId, ServiceType, ProviderName, CustomerName, Amount, Status) VALUES (@Id, @Service, 'Rajkot Agri Labs', @Cust, @Fee, 'Confirmed')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Id", bookingId),
                    new SqlParameter("@Service", service + " (" + dateLoc + ")"),
                    new SqlParameter("@Cust", customer),
                    new SqlParameter("@Fee", fee)
                };
                DbHelper.ExecuteNonQuery(sql, prms);

                // --- Collections Demonstrations ---
                // 1. Generic Queue<T>: Add to soil test queue
                SoilTestRequest request = new SoilTestRequest();
                request.RequestId = new Random().Next(100, 999);
                request.FarmerName = customer;
                request.ServiceName = service;
                request.RequestDate = DateTime.Now;
                KhedutCollectionsService.EnqueueSoilTest(request);

                // 2. Generic Delegate Action<T>: Log message
                KhedutCollectionsService.LogMessage("Service booked for " + customer, msg => { });

                // 3. Generic Class ApiResponse<T>: Standard response wrapper
                ApiResponse<string> apiResult = new ApiResponse<string>(true, "Booking confirmed", bookingId);

                // Step 4: Show success message
                lblServiceMsg.Text = "Service Request Confirmed! Assigned Booking ID: " + bookingId;
                lblServiceMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=MyBookings.aspx");
            }
            catch (Exception ex)
            {
                lblServiceMsg.Text = "Booking failed: " + ex.Message;
                lblServiceMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
