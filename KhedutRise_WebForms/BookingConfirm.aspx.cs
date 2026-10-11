using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Booking Confirmation - Machinery rental booking
    public partial class BookingConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default booking date as tomorrow
                txtBookingDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
        }

        protected void btnConfirmBooking_Click(object sender, EventArgs e)
        {
            // Step 1: Read values from form fields
            string date = txtBookingDate.Text.Trim();
            string durationStr = txtDuration.Text.Trim();
            string address = txtAddress.Text.Trim();
            string paymentMode = ddlPaymentMode.SelectedValue;

            // Step 2: Form validation
            if (string.IsNullOrWhiteSpace(date))
            {
                lblBookingMsg.Text = " Please select a valid booking date.";
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            int hours;
            if (!int.TryParse(durationStr, out hours) || hours <= 0)
            {
                lblBookingMsg.Text = " Please enter valid rental hours.";
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                lblBookingMsg.Text = " Please enter delivery address.";
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            try
            {
                // Generate a booking ID
                string bookingId = "#KR-TR-" + new Random().Next(500, 999);

                // Get farmer name from session
                string customerName = Session["UserName"] != null ? Session["UserName"].ToString() : "Sanyaja Tirth";

                // Calculate total amount using Func Generic Delegate (hours * 800 + 500 delivery)
                decimal total = KhedutCollectionsService.CalculateRent(hours, 800, baseRent => baseRent + 500);

                // Step 3: Insert booking into Bookings table
                string query = "INSERT INTO Bookings (BookingId, ServiceType, ProviderName, CustomerName, Amount, Status) VALUES (@Id, @Service, 'Dilipbhai Parmar', @Cust, @Amt, 'Confirmed')";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", bookingId),
                    new SqlParameter("@Service", "Massey Ferguson 241DI (" + hours + " hrs on " + date + ") [" + paymentMode + "]"),
                    new SqlParameter("@Cust", customerName),
                    new SqlParameter("@Amt", total)
                };
                DbHelper.ExecuteNonQuery(query, parameters);

                // Add to rental booking queue (Queue collection example)
                KhedutCollectionsService.EnqueueRentalBooking(bookingId + " - " + customerName + " (" + hours + " hrs)");

                // Step 4: Show success message and redirect
                lblBookingMsg.Text = " Booking Confirmed! ID: " + bookingId + ". Total amount: ₹" + total.ToString("N0") + ". Redirecting to My Bookings...";
                lblBookingMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=MyBookings.aspx");
            }
            catch (Exception ex)
            {
                lblBookingMsg.Text = " Booking failed: " + ex.Message;
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
