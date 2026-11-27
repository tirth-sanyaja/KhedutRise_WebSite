using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Contact Us Page
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // CREATE: Insert inquiry message into ContactInquiries table
        protected void btnSubmitContact_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            string name = txtContactName.Text.Trim();
            string mobile = txtContactMobile.Text.Trim();
            string email = txtContactEmail.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string message = txtContactBody.Text.Trim();

            // 1. Validation checks
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                ShowError("Please enter your full name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length < 10)
            {
                ShowError("Please enter a valid 10-digit mobile number.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            if (string.IsNullOrWhiteSpace(message) || message.Length < 10)
            {
                ShowError("Please provide message details (minimum 10 characters).");
                return;
            }

            // 2. Database Insertion
            try
            {
                string sql = "INSERT INTO ContactInquiries (FullName, MobileNo, Message) VALUES (@Name, @Mobile, @Msg)";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Msg", "[" + category + " | " + email + "] " + message)
                };
                DbHelper.ExecuteNonQuery(sql, prms);

                // Dispatch confirmation notification email
                try
                {
                    string emailErr;
                    EmailService.SendNotificationEmail(
                        email,
                        name,
                        "KhedutRise - Support Inquiry Received",
                        "We Have Received Your Inquiry",
                        "Thank you for contacting KhedutRise Support. Your message regarding <strong>" + System.Web.HttpUtility.HtmlEncode(category) + "</strong> has been received and logged in our system. Our support team will review your inquiry and get in touch with you shortly.",
                        "Visit KhedutRise Portal",
                        "https://khedutrise.com",
                        out emailErr);
                }
                catch { }

                lblContactMsg.Text = "Thank you, " + name + "! Your inquiry has been submitted successfully. A confirmation has been sent to your email and our team will contact you shortly.";
                lblContactMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                txtContactName.Text = "";
                txtContactMobile.Text = "";
                txtContactEmail.Text = "";
                txtContactBody.Text = "";
            }
            catch (Exception ex)
            {
                ShowError("Failed to submit message: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            lblContactMsg.Text = " " + msg;
            lblContactMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
