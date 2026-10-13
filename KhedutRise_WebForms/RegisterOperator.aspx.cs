using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Operator Account Creation 
    public partial class RegisterOperator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmitOp_Click(object sender, EventArgs e)
        {
            string fullName = txtName.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string model = txtModel.Text.Trim();
            string rate = txtRate.Text.Trim();
            string attachments = txtAttachments.Text.Trim();
            string radius = txtRadius.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // 1. Validation checks
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)
            {
                ShowError("Please enter your full name (minimum 2 characters).");
                return;
            }

            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10 || !long.TryParse(mobile, out _))
            {
                ShowError("Please enter a valid 10-digit Indian mobile number.");
                return;
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                ShowError("Please enter your machinery or tractor model.");
                return;
            }

            if (string.IsNullOrWhiteSpace(rate))
            {
                ShowError("Please enter your hourly rental rate.");
                return;
            }

            if (string.IsNullOrWhiteSpace(attachments))
            {
                ShowError("Please specify available attachments.");
                return;
            }

            if (string.IsNullOrWhiteSpace(radius))
            {
                ShowError("Please specify your service village radius.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ShowError("Password must be at least 6 characters long.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match. Please verify.");
                return;
            }

            // 2. Database Insertion
            try
            {
                string query = "INSERT INTO Users (FullName, MobileNo, UserRole, Location, SpecificField, Status) VALUES (@Name, @Mobile, 'Operator', 'Gujarat', @Field, 'Pending')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", fullName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Field", model + " | Rate: ₹" + rate + "/hr | Area: " + radius)
                };

                DbHelper.ExecuteNonQuery(query, prms);

                // 3. Establish active Operator session and redirect
                Session["UserRole"] = "Operator";
                Session["UserName"] = fullName;
                Session["UserMobile"] = mobile;
                lblOpMsg.Text = "Success! Machinery Operator registered successfully. Redirecting to rentals portal...";
                lblOpMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=TractorRentals.aspx");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE") || ex.Message.Contains("duplicate"))
                {
                    ShowError("This mobile number (+91 " + mobile + ") is already registered. Please sign in or use another number.");
                }
                else
                {
                    ShowError("Registration failed: " + ex.Message);
                }
            }
        }

        private void ShowError(string msg)
        {
            lblOpMsg.Text = "" + msg;
            lblOpMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
