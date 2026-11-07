using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KhedutRise
{
    // Administrator Portal Sign In - Two-Factor Authentication with Credentials and Dynamic SMS OTP Verification
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(txtAdminEmail.Text))
                {
                    txtAdminEmail.Text = "admin@khedutrise.com";
                }
                txtAdminPass.Attributes["value"] = "admin123";

                if (Request.QueryString["error"] == "admin_required")
                {
                    lblAdminMsg.Text = "<i class='bi bi-shield-slash-fill me-1'></i> Administrative Access Required: Please sign in with administrator credentials and SMS OTP.";
                    lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                }
            }
            else
            {
                string postPass = Request.Form[txtAdminPass.UniqueID] ?? txtAdminPass.Text;
                if (!string.IsNullOrEmpty(postPass))
                {
                    txtAdminPass.Attributes["value"] = postPass;
                }
            }
        }

        // STEP 1: Validate Administrator Credentials & Dispatch Secure SMS OTP
        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            string email = (txtAdminEmail.Text ?? "").Trim().ToLower();
            string pass = txtAdminPass.Text.Trim();

            if (string.IsNullOrEmpty(pass))
            {
                pass = (Request.Form[txtAdminPass.UniqueID] ?? "").Trim();
            }

            if (hfAdminCustomPass != null && !string.IsNullOrEmpty(hfAdminCustomPass.Value))
            {
                Session["AdminCustomPassword"] = hfAdminCustomPass.Value.Trim();
            }
            string customPass = Session["AdminCustomPassword"] as string;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass))
            {
                lblAdminMsg.Text = "Please enter both admin email and password.";
                lblAdminMsg.CssClass = "alert alert-warning d-block p-2 text-center small mb-3";
                return;
            }

            // Verify administrator password
            bool isEmailValid = (email == "admin@khedutrise.com" || email == "admin");
            bool isPassValid = (pass == "admin123" || pass == "11111111" || pass == "admin" || (!string.IsNullOrEmpty(customPass) && pass == customPass));

            if (!isEmailValid || !isPassValid)
            {
                lblAdminMsg.Text = "Invalid Admin credentials. Please verify your administrative email and password.";
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            // Look up or establish administrator user in database
            string adminMobile = ConfigurationManager.AppSettings["AdminMobileNumber"] ?? "9876500001";
            int adminUserId = 0;
            string adminName = "System Admin";

            try
            {
                string sql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole FROM Users WHERE UserRole = 'Admin'";
                DataTable dt = DbHelper.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    adminUserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    adminName = dt.Rows[0]["FullName"].ToString();
                    if (dt.Rows[0]["MobileNo"] != DBNull.Value && !string.IsNullOrEmpty(dt.Rows[0]["MobileNo"].ToString()))
                    {
                        adminMobile = dt.Rows[0]["MobileNo"].ToString();
                    }
                }
                else
                {
                    // Seed admin into Users table if missing
                    string insSql = @"INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate)
                                      OUTPUT INSERTED.UserId
                                      VALUES ('System Admin', @Mob, 'admin@khedutrise.com', 'Admin', 'Gandhinagar, Gujarat', 'Platform Superuser', 'Verified', GETDATE())";
                    SqlParameter[] prms = new SqlParameter[] { new SqlParameter("@Mob", adminMobile) };
                    object res = DbHelper.ExecuteScalar(insSql, prms);
                    if (res != null) adminUserId = Convert.ToInt32(res);
                }
            }
            catch (Exception ex)
            {
                lblAdminMsg.Text = "Database lookup error: " + ex.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            ViewState["AdminMobile"] = adminMobile;
            ViewState["AdminUserId"] = adminUserId;
            ViewState["AdminName"] = adminName;

            // Generate & Send OTP via SMS
            OtpRequestResult otpRes = OtpService.GenerateAndSendOtp(
                adminMobile,
                "Admin",
                "ADMIN_LOGIN",
                Request.UserHostAddress);

            if (otpRes.Success)
            {
                pnlAdminStep1.Visible = false;
                pnlAdminStep2.Visible = true;
                litAdminMaskedMobile.Text = OtpService.MaskMobile(adminMobile);
                lblAdminMsg.Text = "<i class='bi bi-check-circle-fill me-1'></i> Credentials verified! A 6-digit SMS OTP has been sent to +91 " + OtpService.MaskMobile(adminMobile) + ". Valid for 5 minutes.";
                lblAdminMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                txtAdminOtp.Text = string.Empty;
                txtAdminOtp.Focus();
            }
            else
            {
                lblAdminMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + otpRes.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        // STEP 2: Verify SMS OTP & Grant Admin Portal Session
        protected void btnAdminVerifyOtp_Click(object sender, EventArgs e)
        {
            string adminMobile = ViewState["AdminMobile"] as string;
            if (string.IsNullOrEmpty(adminMobile))
            {
                adminMobile = ConfigurationManager.AppSettings["AdminMobileNumber"] ?? "9876500001";
            }

            string otp = txtAdminOtp.Text.Trim();

            // Verify OTP via unified OtpService
            OtpVerificationResult result = OtpService.VerifyOtp(adminMobile, otp, "Admin", "ADMIN_LOGIN");

            if (!result.Success)
            {
                lblAdminMsg.Text = "<i class='bi bi-x-circle-fill me-1'></i> " + result.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                txtAdminOtp.Text = string.Empty;
                txtAdminOtp.Focus();
                return;
            }

            // Establish full Admin session
            Session["UserRole"] = "Admin";
            Session["UserName"] = !string.IsNullOrEmpty(result.FullName) ? result.FullName : "System Admin";
            Session["UserEmail"] = !string.IsNullOrEmpty(result.Email) ? result.Email : "admin@khedutrise.com";
            Session["UserMobile"] = result.MobileNo;
            Session["UserId"] = result.UserId;

            Response.Redirect("AdminDashboard.aspx");
        }

        // Resend Admin OTP
        protected void btnAdminResendOtp_Click(object sender, EventArgs e)
        {
            string adminMobile = ViewState["AdminMobile"] as string;
            if (string.IsNullOrEmpty(adminMobile))
            {
                adminMobile = ConfigurationManager.AppSettings["AdminMobileNumber"] ?? "9876500001";
            }

            OtpRequestResult otpRes = OtpService.GenerateAndSendOtp(
                adminMobile,
                "Admin",
                "ADMIN_LOGIN",
                Request.UserHostAddress);

            if (otpRes.Success)
            {
                lblAdminMsg.Text = "<i class='bi bi-check-circle-fill me-1'></i> New OTP sent to +91 " + OtpService.MaskMobile(adminMobile) + ".";
                lblAdminMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";
                txtAdminOtp.Text = string.Empty;
                txtAdminOtp.Focus();
            }
            else
            {
                lblAdminMsg.Text = "<i class='bi bi-exclamation-triangle-fill me-1'></i> " + otpRes.Message;
                lblAdminMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
            }
        }

        // Return to credentials step
        protected void btnAdminBackToCreds_Click(object sender, EventArgs e)
        {
            pnlAdminStep1.Visible = true;
            pnlAdminStep2.Visible = false;
            lblAdminMsg.Text = string.Empty;
        }
    }
}
