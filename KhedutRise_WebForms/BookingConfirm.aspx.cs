using System;
using System.Data;

namespace KhedutRise
{
    public partial class BookingConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                string returnUrl = Request.RawUrl;
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(returnUrl));
                return;
            }

            if (!IsPostBack)
            {
                txtBookingDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                int eqId = 1;
                if (!string.IsNullOrEmpty(Request.QueryString["eqId"]))
                {
                    int.TryParse(Request.QueryString["eqId"], out eqId);
                }

                LoadEquipmentDetails(eqId);
            }
        }

        private void LoadEquipmentDetails(int eqId)
        {
            try
            {
                DataTable dt = EquipmentRepository.GetEquipmentById(eqId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hfEquipmentId.Value = row["EquipmentId"].ToString();
                    litModelName.Text = row["ModelName"].ToString();
                    litOwnerName.Text = row["ResolvedOwnerName"].ToString();
                    litLocation.Text = row["ResolvedLocation"].ToString();
                    litHorsepower.Text = row["Horsepower"].ToString();

                    decimal hourlyRate = Convert.ToDecimal(row["HourlyRate"]);
                    hfHourlyRate.Value = hourlyRate.ToString("0.##");
                    litHourlyRate.Text = hourlyRate.ToString("N0");

                    string imgPath = row["ImagePath"] != DBNull.Value ? row["ImagePath"].ToString() : "Images/massey_tractor.jpg";
                    imgEquipment.ImageUrl = ResolveUrl(imgPath);

                    RecalculatePricing();
                }
                else
                {
                    lblBookingMsg.Text = "Requested agricultural machinery was not found.";
                    lblBookingMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                    btnConfirmBooking.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblBookingMsg.Text = "Error loading machinery details: " + ex.Message;
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        protected void txtDuration_TextChanged(object sender, EventArgs e)
        {
            RecalculatePricing();
        }

        private void RecalculatePricing()
        {
            int hours = 6;
            int.TryParse(txtDuration.Text.Trim(), out hours);
            if (hours <= 0) hours = 1;

            decimal rate = 800;
            decimal.TryParse(hfHourlyRate.Value, out rate);

            decimal subtotal = hours * rate;
            decimal transport = 500;
            decimal total = subtotal + transport;

            litBaseRateCalc.Text = "₹" + rate.ToString("N0") + "/hr";
            litSubtotal.Text = subtotal.ToString("N0");
            litTotal.Text = total.ToString("N0");
        }

        protected void btnConfirmBooking_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string customerName = Session["UserName"] != null ? Session["UserName"].ToString() : "Verified Farmer";

            string date = txtBookingDate.Text.Trim();
            string durationStr = txtDuration.Text.Trim();
            string address = txtAddress.Text.Trim();
            string paymentMode = ddlPaymentMode.SelectedValue;

            if (string.IsNullOrWhiteSpace(date))
            {
                lblBookingMsg.Text = "Please select a valid booking date.";
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            int hours;
            if (!int.TryParse(durationStr, out hours) || hours <= 0)
            {
                lblBookingMsg.Text = "Please enter valid rental hours (minimum 1 hour).";
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                lblBookingMsg.Text = "Please specify your farm delivery address.";
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            int eqId;
            if (!int.TryParse(hfEquipmentId.Value, out eqId))
            {
                eqId = 1;
            }

            try
            {
                decimal hourlyRate = 800;
                decimal.TryParse(hfHourlyRate.Value, out hourlyRate);
                decimal total = (hours * hourlyRate) + 500;

                string serviceType = litModelName.Text + " (" + hours + " hrs on " + date + ") [" + paymentMode + "]";
                string provider = litOwnerName.Text;

                string bookingId = EquipmentRepository.CreateBooking(userId, eqId, serviceType, provider, customerName, total);

                lblBookingMsg.Text = "Booking Confirmed! Booking ID: " + bookingId + ". Total: ₹" + total.ToString("N0") + ". Redirecting to My Bookings...";
                lblBookingMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                btnConfirmBooking.Enabled = false;

                Response.AddHeader("REFRESH", "2;URL=MyBookings.aspx");
            }
            catch (Exception ex)
            {
                lblBookingMsg.Text = "Booking submission failed: " + ex.Message;
                lblBookingMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
