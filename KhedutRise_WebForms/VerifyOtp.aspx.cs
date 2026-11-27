using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace KhedutRise
{
    // Dedicated, standalone Email OTP Verification page for Registration and 2FA Login
    public partial class VerifyOtp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void InitializePage()
        {
            string purpose = (Request.QueryString["purpose"] ?? "REGISTER").Trim().ToUpper();
            string email   = (Request.QueryString["email"] ?? "").Trim().ToLower();

            hfPurpose.Value = purpose;
            hfCooldownRemaining.Value = "60";

            if (purpose == "LOGIN")
            {
                string ident = Session["PendingLoginIdent"] != null 
                    ? Session["PendingLoginIdent"].ToString() 
                    : (Request.QueryString["ident"] ?? "").Trim();
                string role = Session["PendingLoginRole"] != null 
                    ? Session["PendingLoginRole"].ToString() 
                    : (Request.QueryString["role"] ?? "Farmer").Trim();
                string loginEmail = Session["PendingLoginEmail"] != null 
                    ? Session["PendingLoginEmail"].ToString() 
                    : email;

                if (string.IsNullOrEmpty(ident) || string.IsNullOrEmpty(loginEmail))
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                hfIdent.Value = ident;
                hfRole.Value  = role;
                hfEmail.Value = loginEmail;

                divTargetEmail.InnerText = EmailOtpService.MaskEmail(loginEmail);
                badgePurpose.InnerHtml = string.Format(
                    "<i class='bi bi-shield-lock-fill me-1 text-success'></i> Two-Step Sign In Verification ({0})", 
                    role);
                hlBack.NavigateUrl = "~/Login.aspx";
                hlBack.Text = "<i class='bi bi-arrow-left me-1'></i> Back to Sign In";
            }
            else // REGISTER
            {
                PendingRegistration pending = Session["PendingRegistration"] as PendingRegistration;

                if (pending == null && !string.IsNullOrEmpty(email))
                {
                    // Fallback recovery from UserOTPs.GatewayResponse if session was dropped
                    string fetchRegSql = @"SELECT TOP 1 GatewayResponse FROM UserOTPs 
                                           WHERE LOWER(Email) = @Email AND OTPPurpose = 'REGISTER' 
                                           ORDER BY OTPCreatedAt DESC";
                    object payloadObj = DbHelper.ExecuteScalar(fetchRegSql, new SqlParameter[] { new SqlParameter("@Email", email) });
                    if (payloadObj != null && payloadObj != DBNull.Value)
                    {
                        pending = PendingRegistration.Deserialize(payloadObj.ToString());
                        if (pending != null)
                        {
                            Session["PendingRegistration"] = pending;
                        }
                    }
                }

                if (pending == null && string.IsNullOrEmpty(email))
                {
                    // No pending registration in session or DB -> redirect to role selection
                    Response.Redirect("SelectRole.aspx");
                    return;
                }

                string regEmail = pending != null ? pending.Email : email;
                string regRole  = pending != null ? pending.Role : (Request.QueryString["role"] ?? "Farmer");

                hfEmail.Value = regEmail;
                hfRole.Value  = regRole;

                divTargetEmail.InnerText = EmailOtpService.MaskEmail(regEmail);
                badgePurpose.InnerHtml = string.Format(
                    "<i class='bi bi-person-check-fill me-1 text-success'></i> New {0} Account Verification", 
                    regRole);
                hlBack.NavigateUrl = GetRegisterUrl(regRole);
                hlBack.Text = string.Format("<i class='bi bi-arrow-left me-1'></i> Back to {0} Registration", regRole);
            }
        }

        private string GetRegisterUrl(string role)
        {
            switch (role)
            {
                case "Buyer":    return "~/RegisterBuyer.aspx";
                case "Operator": return "~/RegisterOperator.aspx";
                default:         return "~/RegisterFarmer.aspx";
            }
        }

        protected void btnConfirmOtp_Click(object sender, EventArgs e)
        {
            Page.Validate("OtpVerify");
            if (!Page.IsValid) return;

            string otp     = (txtOtpCode.Text ?? "").Trim();
            string purpose = hfPurpose.Value;

            if (purpose == "LOGIN")
            {
                string ident = hfIdent.Value;
                string role  = hfRole.Value;

                OtpVerificationResult result = EmailOtpService.VerifyLoginOtp(ident, otp, role);

                if (!result.Success)
                {
                    ShowError(result.Message);
                    txtOtpCode.Text = string.Empty;
                    txtOtpCode.Focus();
                    return;
                }

                // Create Authenticated Session
                Session["UserId"]     = result.UserId;
                Session["UserName"]   = result.FullName;
                Session["UserRole"]   = result.UserRole;
                Session["UserMobile"] = result.MobileNo;
                Session["UserEmail"]  = result.Email;

                // Clear temporary login session
                Session["PendingLoginIdent"] = null;
                Session["PendingLoginRole"]  = null;
                Session["PendingLoginEmail"] = null;

                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.Contains("Login.aspx") && !returnUrl.Contains("VerifyOtp.aspx"))
                {
                    Response.Redirect(returnUrl);
                    return;
                }

                RedirectToDashboard(result.UserRole);
            }
            else // REGISTER
            {
                string email = hfEmail.Value.Trim().ToLower();

                // Reconstruct pending registration from Session or durable UserOTPs.GatewayResponse
                PendingRegistration pending = Session["PendingRegistration"] as PendingRegistration;
                if (pending == null)
                {
                    string fetchRegSql = @"SELECT TOP 1 GatewayResponse FROM UserOTPs 
                                           WHERE LOWER(Email) = @Email AND OTPPurpose = 'REGISTER' 
                                           ORDER BY OTPCreatedAt DESC";
                    object payloadObj = DbHelper.ExecuteScalar(fetchRegSql, new SqlParameter[] { new SqlParameter("@Email", email) });
                    if (payloadObj != null && payloadObj != DBNull.Value)
                    {
                        pending = PendingRegistration.Deserialize(payloadObj.ToString());
                        if (pending != null)
                        {
                            Session["PendingRegistration"] = pending;
                        }
                    }
                }

                if (pending == null)
                {
                    string regRole = !string.IsNullOrEmpty(hfRole.Value) ? hfRole.Value : "Farmer";
                    ShowError(string.Format("Registration details not found or timed out. Please <a href='{0}' class='alert-link fw-bold'>fill the registration form again</a>.", ResolveUrl(GetRegisterUrl(regRole))));
                    return;
                }

                // Verify the OTP code entered by the user
                OtpVerificationResult result = EmailOtpService.VerifyEmailOtp(email, otp, "REGISTER");

                if (!result.Success)
                {
                    ShowError(result.Message);
                    txtOtpCode.Text = string.Empty;
                    txtOtpCode.Focus();
                    return;
                }

                // Clean mobile number
                string cleanMobile = EmailOtpService.CleanMobile(pending.MobileNo);

                // Check duplicate mobile number specifically
                var dtMob = DbHelper.ExecuteQuery(
                    "SELECT TOP 1 UserId FROM Users WHERE MobileNo = @Mob",
                    new SqlParameter[] { new SqlParameter("@Mob", cleanMobile) });

                if (dtMob != null && dtMob.Rows.Count > 0)
                {
                    ShowError(string.Format("The mobile number (+91 {0}) is already registered with an existing account. Please sign in or use a different number.", cleanMobile));
                    return;
                }

                // Check duplicate email specifically
                var dtEmail = DbHelper.ExecuteQuery(
                    "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = LOWER(@Email)",
                    new SqlParameter[] { new SqlParameter("@Email", pending.Email) });

                if (dtEmail != null && dtEmail.Rows.Count > 0)
                {
                    ShowError(string.Format("The email address '{0}' is already registered with an existing account. Please sign in.", pending.Email));
                    return;
                }

                try
                {
                    string insertSql = @"INSERT INTO Users 
                        (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, PasswordHash, CreatedDate) 
                        VALUES (@Name, @Mobile, @Email, @Role, @Loc, @Specific, 'Verified', @PassHash, GETDATE())";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@Name",     pending.FullName),
                        new SqlParameter("@Mobile",   cleanMobile),
                        new SqlParameter("@Email",    pending.Email),
                        new SqlParameter("@Role",     pending.Role),
                        new SqlParameter("@Loc",      pending.Location ?? "Gujarat"),
                        new SqlParameter("@Specific", pending.SpecificField ?? ""),
                        new SqlParameter("@PassHash", pending.PasswordHash)
                    };

                    DbHelper.ExecuteNonQuery(insertSql, prms);

                    // Retrieve generated UserId
                    object newId = DbHelper.ExecuteScalar(
                        "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = LOWER(@Email) ORDER BY UserId DESC",
                        new SqlParameter[] { new SqlParameter("@Email", pending.Email) });

                    int userId = newId != null ? Convert.ToInt32(newId) : 0;

                    // Establish Authenticated Session
                    Session["UserId"]     = userId;
                    Session["UserName"]   = pending.FullName;
                    Session["UserRole"]   = pending.Role;
                    Session["UserMobile"] = cleanMobile;
                    Session["UserEmail"]  = pending.Email;

                    // Clear pending registration
                    Session["PendingRegistration"] = null;

                    // Send official Welcome Confirmation Email
                    try
                    {
                        string welcomeErr;
                        EmailService.SendWelcomeEmail(pending.Email, pending.FullName, pending.Role, out welcomeErr);
                    }
                    catch { }

                    ShowSuccess("Registration Successful! Your email has been verified. Welcome to KhedutRise! Redirecting to your dashboard...");
                    RedirectToDashboard(pending.Role);
                }
                catch (Exception ex)
                {
                    ShowError("Failed to activate account: " + ex.Message);
                }
            }
        }

        protected void btnResendOtp_Click(object sender, EventArgs e)
        {
            string purpose = hfPurpose.Value;
            string email   = hfEmail.Value.Trim().ToLower();

            if (purpose == "LOGIN")
            {
                string ident = hfIdent.Value;
                string role  = hfRole.Value;

                OtpRequestResult req = EmailOtpService.GenerateLoginEmailOtp(
                    ident,
                    email,
                    role,
                    Request.UserHostAddress);

                if (req.Success)
                {
                    ShowSuccess("A new 6-digit OTP has been sent to " + EmailOtpService.MaskEmail(email));
                    hfCooldownRemaining.Value = "60";
                }
                else
                {
                    ShowError(req.Message);
                    if (req.CooldownRemainingSeconds > 0)
                    {
                        hfCooldownRemaining.Value = req.CooldownRemainingSeconds.ToString();
                    }
                }
            }
            else // REGISTER
            {
                PendingRegistration pending = Session["PendingRegistration"] as PendingRegistration;
                if (pending == null)
                {
                    string fetchRegSql = @"SELECT TOP 1 GatewayResponse FROM UserOTPs 
                                           WHERE LOWER(Email) = @Email AND OTPPurpose = 'REGISTER' 
                                           ORDER BY OTPCreatedAt DESC";
                    object payloadObj = DbHelper.ExecuteScalar(fetchRegSql, new SqlParameter[] { new SqlParameter("@Email", email) });
                    if (payloadObj != null && payloadObj != DBNull.Value)
                    {
                        pending = PendingRegistration.Deserialize(payloadObj.ToString());
                        if (pending != null) Session["PendingRegistration"] = pending;
                    }
                }

                string recipientName = pending != null ? pending.FullName : null;
                string payload       = pending != null ? pending.Serialize() : null;

                OtpRequestResult req = EmailOtpService.GenerateAndSendEmailOtp(
                    email,
                    "REGISTER",
                    Request.UserHostAddress,
                    recipientName,
                    payload);

                if (req.Success)
                {
                    ShowSuccess("A new 6-digit verification code has been dispatched to " + EmailOtpService.MaskEmail(email));
                    hfCooldownRemaining.Value = "60";
                }
                else
                {
                    ShowError(req.Message);
                    if (req.CooldownRemainingSeconds > 0)
                    {
                        hfCooldownRemaining.Value = req.CooldownRemainingSeconds.ToString();
                    }
                }
            }
        }

        private void RedirectToDashboard(string role)
        {
            string target;
            switch (role)
            {
                case "Farmer":   target = "FarmerDashboard.aspx"; break;
                case "Buyer":    target = "BuyerDashboard.aspx"; break;
                case "Operator": target = "TractorRentals.aspx"; break;
                default:         target = "Default.aspx"; break;
            }

            Response.AddHeader("REFRESH", "1.5;URL=" + target);
        }

        private void ShowError(string msg)
        {
            lblVerifyMsg.Text = string.Format(
                "<div class='alert alert-danger d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' role='alert'>" +
                "<i class='bi bi-exclamation-triangle-fill fs-5 me-2 flex-shrink-0'></i>" +
                "<div>{0}</div></div>",
                msg);
        }

        private void ShowSuccess(string msg)
        {
            lblVerifyMsg.Text = string.Format(
                "<div class='alert alert-success d-flex align-items-center mb-3 p-3 rounded-3 shadow-sm' role='alert' style='background-color: #ecfdf5; border-color: #10b981; color: #065f46;'>" +
                "<i class='bi bi-check-circle-fill fs-5 me-2 flex-shrink-0 text-success'></i>" +
                "<div>{0}</div></div>",
                msg);
        }
    }
}
