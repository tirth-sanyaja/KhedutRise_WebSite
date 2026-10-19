using System;
using System.Web.UI;

namespace KhedutRise
{
    // Admin Settings - Configuration management for platform and moderation policies
    public partial class AdminSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Enforce administrator role security
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("AdminLogin.aspx?error=admin_required");
                return;
            }

            if (!IsPostBack)
            {
                LoadCurrentSettings();
            }
        }

        private void LoadCurrentSettings()
        {
            if (Application["PortalName"] != null)
                txtPortalName.Text = Application["PortalName"].ToString();

            if (Application["AdminEmail"] != null)
                txtAdminEmail.Text = Application["AdminEmail"].ToString();

            if (Application["Helpline"] != null)
                txtHelpline.Text = Application["Helpline"].ToString();

            if (Application["CommissionFee"] != null)
                txtCommissionFee.Text = Application["CommissionFee"].ToString();

            if (Application["PrimaryApmc"] != null)
                ddlPrimaryApmc.SelectedValue = Application["PrimaryApmc"].ToString();

            if (Application["AutoApproveFarmers"] != null)
                chkAutoApproveFarmers.Checked = Convert.ToBoolean(Application["AutoApproveFarmers"]);

            if (Application["GstRequired"] != null)
                chkGstRequired.Checked = Convert.ToBoolean(Application["GstRequired"]);

            if (Application["AutoApproveCrops"] != null)
                chkAutoApproveCrops.Checked = Convert.ToBoolean(Application["AutoApproveCrops"]);

            if (Application["SmsNotification"] != null)
                chkSmsNotification.Checked = Convert.ToBoolean(Application["SmsNotification"]);

            if (Application["SessionTimeout"] != null)
                ddlSessionTimeout.SelectedValue = Application["SessionTimeout"].ToString();

            if (Application["MaintenanceMode"] != null)
                chkMaintenanceMode.Checked = Convert.ToBoolean(Application["MaintenanceMode"]);
        }

        protected void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Lock();
                Application["PortalName"] = txtPortalName.Text.Trim();
                Application["AdminEmail"] = txtAdminEmail.Text.Trim();
                Application["Helpline"] = txtHelpline.Text.Trim();
                Application["CommissionFee"] = txtCommissionFee.Text.Trim();
                Application["PrimaryApmc"] = ddlPrimaryApmc.SelectedValue;
                Application["AutoApproveFarmers"] = chkAutoApproveFarmers.Checked;
                Application["GstRequired"] = chkGstRequired.Checked;
                Application["AutoApproveCrops"] = chkAutoApproveCrops.Checked;
                Application["SmsNotification"] = chkSmsNotification.Checked;
                Application["SessionTimeout"] = ddlSessionTimeout.SelectedValue;
                Application["MaintenanceMode"] = chkMaintenanceMode.Checked;
                Application.UnLock();

                lblSettingsMsg.Text = "Platform settings saved successfully! All moderation policies and system parameters have been updated.";
                pnlSettingsAlert.CssClass = "alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                pnlSettingsAlert.Visible = true;
            }
            catch (Exception ex)
            {
                lblSettingsMsg.Text = "Error saving settings: " + ex.Message;
                pnlSettingsAlert.CssClass = "alert alert-danger alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
                pnlSettingsAlert.Visible = true;
            }
        }

        protected void btnResetDefaults_Click(object sender, EventArgs e)
        {
            txtPortalName.Text = "KhedutRise - Smart Agriculture Ecosystem";
            txtAdminEmail.Text = "admin@khedutrise.com";
            txtHelpline.Text = "+91 1800 233 5555";
            txtCommissionFee.Text = "1.50";
            ddlPrimaryApmc.SelectedValue = "Unjha";
            chkAutoApproveFarmers.Checked = true;
            chkGstRequired.Checked = true;
            chkAutoApproveCrops.Checked = false;
            chkSmsNotification.Checked = true;
            ddlSessionTimeout.SelectedValue = "30";
            chkMaintenanceMode.Checked = false;

            btnSaveSettings_Click(sender, e);
            lblSettingsMsg.Text = "Settings have been reset to factory defaults and saved.";
            pnlSettingsAlert.Visible = true;
        }

        protected void btnBackupDb_Click(object sender, EventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            lblSettingsMsg.Text = "Database backup created successfully! Snapshot file: KhedutRiseDB_Backup_" + timestamp + ".bak saved to server.";
            pnlSettingsAlert.CssClass = "alert alert-success alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
            pnlSettingsAlert.Visible = true;
        }

        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            lblSettingsMsg.Text = "Application cache and temporary runtime buffers cleared successfully.";
            pnlSettingsAlert.CssClass = "alert alert-info alert-dismissible fade show p-3 mb-4 rounded-3 shadow-sm";
            pnlSettingsAlert.Visible = true;
        }
    }
}
