using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    //  Farmer Account Creation - Registers farmer with complete server-side validation
    public partial class RegisterFarmer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmitFarmer_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string crop = txtCrop.Text.Trim();
            string area = txtArea.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // 1. Validation checks
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)
            {
                ShowError("Please enter your full name (minimum 2 characters).");
                return;
            }

            long parsedMob;
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10 || !long.TryParse(mobile, out parsedMob))
            {
                ShowError("Please enter a valid 10-digit Indian mobile number.");
                return;
            }

            if (string.IsNullOrWhiteSpace(crop))
            {
                ShowError("Please enter your primary sowing crop.");
                return;
            }

            if (string.IsNullOrWhiteSpace(area))
            {
                ShowError("Please specify your farm area in acres.");
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
                string query = "INSERT INTO Users (FullName, MobileNo, UserRole, Location, SpecificField, Status) VALUES (@Name, @Mobile, 'Farmer', 'Gujarat', @Crop, 'Verified')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", fullName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Crop", crop + " (" + area + " Acres)")
                };

                DbHelper.ExecuteNonQuery(query, prms);

                // 3. Establish active Farmer session and navigate to dashboard
                Session["UserRole"] = "Farmer";
                Session["UserName"] = fullName;
                Session["UserMobile"] = mobile;
                lblRegMsg.Text = "Success! Farmer account registered successfully. Redirecting to your dashboard...";
                lblRegMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=FarmerDashboard.aspx");
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
            lblRegMsg.Text = " " + msg;
            lblRegMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
