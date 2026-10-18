using System;
using System.Data.SqlClient;

namespace KhedutRise
{
    public partial class ProfileSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                User u = UserRepository.GetUserById(userId);

                if (u != null)
                {
                    litUserRole.Text = u.UserRole;
                    litProfileName.Text = u.FullName;
                    txtProfileName.Text = u.FullName;
                    txtProfileMobile.Text = u.MobileNo;
                    txtProfileVillage.Text = u.Location;
                    txtProfileEmail.Text = u.Email;

                    if (!string.IsNullOrEmpty(u.FullName))
                    {
                        string[] parts = u.FullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            litAvatar.Text = (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
                        }
                        else if (parts.Length == 1 && parts[0].Length > 0)
                        {
                            litAvatar.Text = parts[0][0].ToString().ToUpper();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblProfileMsg.Text = "Failed to load user profile: " + ex.Message;
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string name = txtProfileName.Text.Trim();
            string mobile = txtProfileMobile.Text.Trim();
            string village = txtProfileVillage.Text.Trim();
            string email = txtProfileEmail.Text.Trim();

            // Server-side validation
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                lblProfileMsg.Text = "Please enter a valid full name (minimum 2 characters).";
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length < 10)
            {
                lblProfileMsg.Text = "Please enter a valid 10-digit mobile number.";
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(village))
            {
                lblProfileMsg.Text = "Please specify your village or district location.";
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            try
            {
                bool success = UserRepository.UpdateProfile(userId, name, email, village, "");
                if (success)
                {
                    Session["UserName"] = name;
                    litProfileName.Text = name;
                    lblProfileMsg.Text = "Profile details saved successfully in SQL Server!";
                    lblProfileMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                }
                else
                {
                    lblProfileMsg.Text = "Unable to update profile record.";
                    lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
            catch (Exception ex)
            {
                lblProfileMsg.Text = "Error saving profile: " + ex.Message;
                lblProfileMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string curPass = txtCurrentPassword.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();
            string confPass = txtConfirmNewPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(curPass))
            {
                lblPasswordMsg.Text = "Please enter your current password.";
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 6)
            {
                lblPasswordMsg.Text = "New password must be at least 6 characters long.";
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (newPass != confPass)
            {
                lblPasswordMsg.Text = "New password and confirmation do not match.";
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            try
            {
                string error;
                bool updated = UserRepository.ChangePassword(userId, curPass, newPass, out error);
                if (updated)
                {
                    lblPasswordMsg.Text = "Your password has been securely updated in SQL Server!";
                    lblPasswordMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                    txtCurrentPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmNewPassword.Text = "";
                }
                else
                {
                    lblPasswordMsg.Text = "Password update failed: " + error;
                    lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
            catch (Exception ex)
            {
                lblPasswordMsg.Text = "Error updating password: " + ex.Message;
                lblPasswordMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }
    }
}
