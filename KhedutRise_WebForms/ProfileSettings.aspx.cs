using System;
using System.Data;
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

                    // Store current verified email in ViewState to detect changes
                    ViewState["CurrentEmail"] = (u.Email ?? "").Trim().ToLower();

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

            int userId       = Convert.ToInt32(Session["UserId"]);
            string name      = txtProfileName.Text.Trim();
            string mobile    = txtProfileMobile.Text.Trim();
            string village   = txtProfileVillage.Text.Trim();
            string newEmail  = txtProfileEmail.Text.Trim().ToLower();
            string currEmail = (ViewState["CurrentEmail"] != null ? ViewState["CurrentEmail"].ToString() : "").Trim().ToLower();

            // Server-side validation
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                ShowProfileMessage("Please enter a valid full name (minimum 2 characters).", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 10)
            {
                ShowProfileMessage("Please enter a valid 10-digit mobile number.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(village))
            {
                ShowProfileMessage("Please specify your village or district location.", false);
                return;
            }

            // Check if email address is being changed
            bool isEmailChanged = !string.IsNullOrEmpty(newEmail) && !newEmail.Equals(currEmail, StringComparison.OrdinalIgnoreCase);

            if (isEmailChanged)
            {
                // Validate new email format
                if (!newEmail.Contains("@") || !newEmail.Contains("."))
                {
                    ShowProfileMessage("Please enter a valid email address.", false);
                    return;
                }

                // Check if new email is already registered to another account
                string dupSql = "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = LOWER(@Email) AND UserId <> @UserId";
                var dtDup = DbHelper.ExecuteQuery(dupSql, new SqlParameter[]
                {
                    new SqlParameter("@Email", newEmail),
                    new SqlParameter("@UserId", userId)
                });

                if (dtDup != null && dtDup.Rows.Count > 0)
                {
                    ShowProfileMessage("This email address is already registered to another account. Please use another email.", false);
                    return;
                }

                // Dispatch OTP to the new email address
                OtpRequestResult otpRes = EmailOtpService.GenerateAndSendEmailOtp(
                    newEmail,
                    "CHANGE_EMAIL",
                    Request.UserHostAddress,
                    name);

                if (!otpRes.Success)
                {
                    ShowProfileMessage("Failed to send verification code to new email: " + otpRes.Message, false);
                    return;
                }

                // Store pending changes in session until verified
                Session["PendingNewEmail"] = newEmail;
                Session["PendingName"]     = name;
                Session["PendingMobile"]   = mobile;
                Session["PendingVillage"]  = village;

                // Show OTP panel
                pnlVerifyNewEmail.Visible = true;
                lblEmailOtpNotice.Text = string.Format(
                    "A 6-digit verification code has been dispatched to <b>{0}</b>. Please enter the code below to confirm this email change. Your existing email ({1}) remains active until verified.",
                    EmailOtpService.MaskEmail(newEmail),
                    currEmail);
                txtNewEmailOtp.Text = string.Empty;
                txtNewEmailOtp.Focus();

                ShowProfileMessage("Verification code sent to " + EmailOtpService.MaskEmail(newEmail) + ". Please enter the 6-digit OTP below.", true);
                return;
            }

            // Normal profile save without email change
            try
            {
                bool success = UserRepository.UpdateProfile(userId, name, currEmail, village, "");
                if (success)
                {
                    Session["UserName"]   = name;
                    Session["UserMobile"] = mobile;
                    litProfileName.Text   = name;
                    ShowProfileMessage("Profile details updated successfully!", true);
                }
                else
                {
                    ShowProfileMessage("Unable to update profile record.", false);
                }
            }
            catch (Exception ex)
            {
                ShowProfileMessage("Error saving profile: " + ex.Message, false);
            }
        }

        // Confirm New Email via OTP Verification
        protected void btnConfirmNewEmail_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId       = Convert.ToInt32(Session["UserId"]);
            string newEmail  = Session["PendingNewEmail"] != null ? Session["PendingNewEmail"].ToString() : "";
            string name      = Session["PendingName"] != null ? Session["PendingName"].ToString() : txtProfileName.Text.Trim();
            string village   = Session["PendingVillage"] != null ? Session["PendingVillage"].ToString() : txtProfileVillage.Text.Trim();
            string otp       = (txtNewEmailOtp.Text ?? "").Trim();

            if (string.IsNullOrEmpty(newEmail))
            {
                ShowProfileMessage("Email change request timed out. Please enter your new email again.", false);
                pnlVerifyNewEmail.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(otp) || otp.Length != 6)
            {
                ShowProfileMessage("Please enter the 6-digit verification OTP sent to your new email.", false);
                return;
            }

            // Verify OTP
            OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(newEmail, otp, "CHANGE_EMAIL");
            if (!result.Success)
            {
                ShowProfileMessage(result.Message + " Your existing email address remains unchanged.", false);
                txtNewEmailOtp.Text = string.Empty;
                txtNewEmailOtp.Focus();
                return;
            }

            // OTP verified -> Update database with new verified email
            try
            {
                bool updated = UserRepository.UpdateProfile(userId, name, newEmail, village, "");
                if (updated)
                {
                    Session["UserEmail"]      = newEmail;
                    Session["UserName"]       = name;
                    ViewState["CurrentEmail"] = newEmail;
                    txtProfileEmail.Text      = newEmail;

                    // Send Security Notification Email to the new email
                    try
                    {
                        string notifErr;
                        EmailService.SendNotificationEmail(
                            newEmail,
                            name,
                            "KhedutRise - Email Address Updated",
                            "Email Address Successfully Verified",
                            "Your KhedutRise account email address was successfully updated and verified to <strong>" + newEmail + "</strong>.",
                            "Visit Portal",
                            "https://khedutrise.com",
                            out notifErr);
                    }
                    catch { }

                    // Clear pending session
                    Session["PendingNewEmail"] = null;
                    Session["PendingName"]     = null;
                    Session["PendingMobile"]   = null;
                    Session["PendingVillage"]  = null;

                    pnlVerifyNewEmail.Visible = false;
                    ShowProfileMessage("Your email address has been successfully verified and updated to " + newEmail + "!", true);
                }
                else
                {
                    ShowProfileMessage("Failed to update database with new email.", false);
                }
            }
            catch (Exception ex)
            {
                ShowProfileMessage("Error updating email: " + ex.Message, false);
            }
        }

        // Cancel Email Change
        protected void btnCancelEmailChange_Click(object sender, EventArgs e)
        {
            string currEmail = ViewState["CurrentEmail"] != null ? ViewState["CurrentEmail"].ToString() : "";
            txtProfileEmail.Text      = currEmail;
            pnlVerifyNewEmail.Visible = false;
            Session["PendingNewEmail"] = null;
            ShowProfileMessage("Email change cancelled. Your registered email remains " + currEmail, true);
        }

        // Change Password with Email Confirmation Alert
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int userId      = Convert.ToInt32(Session["UserId"]);
            string curPass  = txtCurrentPassword.Text.Trim();
            string newPass  = txtNewPassword.Text.Trim();
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
                    lblPasswordMsg.Text = "Your password has been securely updated!";
                    lblPasswordMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                    txtCurrentPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtConfirmNewPassword.Text = "";

                    // Dispatch Password Changed Email Notification
                    string userEmail = Session["UserEmail"] != null ? Session["UserEmail"].ToString() : "";
                    string userName  = Session["UserName"] != null ? Session["UserName"].ToString() : "Member";
                    if (!string.IsNullOrEmpty(userEmail) && userEmail.Contains("@"))
                    {
                        try
                        {
                            string mailErr;
                            EmailService.SendPasswordChangedEmail(userEmail, userName, out mailErr);
                        }
                        catch { }
                    }
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

        private void ShowProfileMessage(string message, bool isSuccess)
        {
            lblProfileMsg.Text = message;
            lblProfileMsg.CssClass = isSuccess 
                ? "alert alert-success d-block p-2 text-center small mb-3" 
                : "alert alert-danger d-block p-2 text-center small mb-3";
        }
    }
}
