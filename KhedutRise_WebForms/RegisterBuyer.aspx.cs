using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Buyer Account Creation - Registers buyer with full server-side validation
    public partial class RegisterBuyer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmitBuyer_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();
            string bizType = ddlBizType.SelectedValue;
            string interests = txtInterests.Text.Trim();
            string gstin = txtGstin.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // 1. Validation checks
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 2)
            {
                ShowError("Please enter your full name or company representative name.");
                return;
            }

            long parsedMob;
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10 || !long.TryParse(mobile, out parsedMob))
            {
                ShowError("Please enter a valid 10-digit Indian mobile number.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
            {
                ShowError("Please enter a valid business email address.");
                return;
            }

            if (string.IsNullOrWhiteSpace(bizType))
            {
                ShowError("Please select your business type.");
                return;
            }

            if (string.IsNullOrWhiteSpace(gstin))
            {
                ShowError("Please provide your GSTIN or APMC trade license number.");
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
                string query = "INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status) VALUES (@Name, @Mobile, @Email, 'Buyer', 'Gujarat', @Field, 'Verified')";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", fullName),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Field", bizType + " | " + interests + " (GST: " + gstin + ")")
                };

                DbHelper.ExecuteNonQuery(query, prms);

                // 3. Establish active Buyer session and redirect
                Session["UserRole"] = "Buyer";
                Session["UserName"] = fullName;
                Session["UserMobile"] = mobile;
                lblBuyerMsg.Text = "Success! Buyer account registered successfully. Redirecting to your dashboard...";
                lblBuyerMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                Response.AddHeader("REFRESH", "1;URL=BuyerDashboard.aspx");
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
            lblBuyerMsg.Text = " " + msg;
            lblBuyerMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
