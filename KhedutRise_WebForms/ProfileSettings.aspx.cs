using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    //  Profile & Settings - Update profile details and password with validation
    public partial class ProfileSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string role = Session["UserRole"] != null ? Session["UserRole"].ToString() : "Farmer";
                string name = Session["UserName"] != null ? Session["UserName"].ToString() : "Sanyaja Tirth";

                litUserRole.Text = role;
                litProfileName.Text = name;
                txtProfileName.Text = name;

                if (name.Length >= 2)
                {
                    litAvatar.Text = name.Substring(0, 1).ToUpper() + (name.Contains(" ") ? name.Substring(name.IndexOf(" ") + 1, 1).ToUpper() : "");
                }
            }
        }

        // UPDATE: Save updated profile changes to the Users table
        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            string name = txtProfileName.Text.Trim();
            string mobile = txtProfileMobile.Text.Trim();
            string village = txtProfileVillage.Text.Trim();
            string email = txtProfileEmail.Text.Trim();

            // Validation
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                lblProfileMsg.Text = " Please enter a valid name (minimum 2 characters).";
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length < 10)
            {
                lblProfileMsg.Text = " Please enter a valid 10-digit mobile number.";
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(village))
            {
                lblProfileMsg.Text = " Please enter your village or district location.";
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            try
            {
                string sql = "UPDATE Users SET FullName = @Name, Location = @Loc WHERE MobileNo = @Mobile";
                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Loc", village),
                    new SqlParameter("@Mobile", mobile)
                };
                DbHelper.ExecuteNonQuery(sql, prms);

                Session["UserName"] = name;
                litProfileName.Text = name;
                lblProfileMsg.Text = " Profile details saved successfully!";
                lblProfileMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
            }
            catch (Exception ex)
            {
                lblProfileMsg.Text = " Error saving changes: " + ex.Message;
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        // Handle Change Password
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string curPass = txtCurrentPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();
            string confPass = txtConfirmNewPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(curPass))
            {
                lblPasswordMsg.Text = " Please enter your current password.";
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 6)
            {
                lblPasswordMsg.Text = " New password must be at least 6 characters long.";
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (newPass != confPass)
            {
                lblPasswordMsg.Text = " New passwords do not match. Please verify.";
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            lblPasswordMsg.Text = " Your password has been updated successfully!";
            lblPasswordMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmNewPassword.Text = "";
        }
    }
}
