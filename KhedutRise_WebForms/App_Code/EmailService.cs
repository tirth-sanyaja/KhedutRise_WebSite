using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace KhedutRise
{
    /// <summary>
    /// Premium Branded .NET Email Service for KhedutRise
    /// Delivers enterprise-grade, beautifully branded emails with official logo,
    /// agricultural green aesthetics, plain-text alternatives, and strict security notices.
    /// </summary>
    public static class EmailService
    {
        private const string PrimaryColor   = "#0b6028"; // KhedutRise Forest Green
        private const string PrimaryHover   = "#166534";
        private const string AccentColor    = "#10b981"; // Vibrant Emerald Green
        private const string LightBgColor   = "#f0fdf4"; // Soft Green Card Background
        private const string TextDark       = "#0f172a";
        private const string TextMuted      = "#64748b";
        private const string BorderColor    = "#e2e8f0";
        private const string Tagline        = "Revolutionizing Indian Agriculture Through Transparent Technology.";
        private const string FooterTagline  = "Growing Together, Building a Smarter Agricultural Future.";
        private const string SupportEmail   = "support@khedutrise.com";
        private const string SupportPhone   = "+91 98250 12345";
        private const string WebsiteUrl     = "https://khedutrise.com";

        // =====================================================================
        // 1. PRIMARY OTP VERIFICATION EMAIL
        // =====================================================================
        public static bool SendOtpEmail(
            string toEmail,
            string otp,
            out string errorMessage,
            string expiryMinutes = "5",
            string recipientName = null,
            string purpose = "VERIFICATION")
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(toEmail) || !toEmail.Contains("@"))
            {
                errorMessage = "Invalid recipient email address.";
                return false;
            }

            string safeName = string.IsNullOrWhiteSpace(recipientName) ? "Valued Agriculturist" : recipientName.Trim();
            string subject  = "KhedutRise - Your Verification OTP Code";

            // Plain-text alternative
            string plainText = string.Format(
@"KhedutRise - {0}
============================================================

Hello {1},

Welcome to KhedutRise!
We're excited to have you as part of our growing agricultural technology community.

To verify your email address and continue securely, please use the One-Time Password (OTP) provided below:

YOUR VERIFICATION CODE: {2}

This code is strictly valid for {3} minutes and can only be used once.

SECURITY ADVISORY:
- For your security, never share this OTP with anyone.
- KhedutRise will never ask you to disclose your OTP over a phone call or message.
- If you did not request this verification code, you can safely ignore this email.

Growing Together, Building a Smarter Agricultural Future.
Website: {4}
Support: {5} | {6}
Copyright (C) {7} KhedutRise. All Rights Reserved.
", Tagline, safeName, otp, expiryMinutes, WebsiteUrl, SupportEmail, SupportPhone, DateTime.Now.Year);

            // HTML Body
            string htmlContent = BuildOtpEmailContent(safeName, otp, expiryMinutes, purpose);
            string fullHtml     = WrapInBrandedLayout("Verify Your Email Address", htmlContent);

            return SendBrandedEmail(toEmail, subject, plainText, fullHtml, out errorMessage);
        }

        // Backward compatibility overload
        public static bool SendOtpEmail(string toEmail, string otp, string expiryMinutes = "5")
        {
            string error;
            return SendOtpEmail(toEmail, otp, out error, expiryMinutes);
        }

        // =====================================================================
        // 2. WELCOME / REGISTRATION CONFIRMATION EMAIL
        // =====================================================================
        public static bool SendWelcomeEmail(string toEmail, string fullName, string userRole, out string errorMessage)
        {
            string safeName = string.IsNullOrWhiteSpace(fullName) ? "Valued Member" : fullName.Trim();
            string subject  = string.Format("Welcome to KhedutRise, {0}! Your {1} Account is Active", safeName, userRole);

            string plainText = string.Format(
@"Welcome to KhedutRise!
============================================================

Hello {0},

Congratulations! Your KhedutRise {1} account has been successfully verified and activated.

You now have full access to:
- Direct Mandi & APMC Price Transparency
- Direct Buyer-Farmer Harvest Trade
- Peer-to-Peer Tractor & Agricultural Equipment Rentals
- Escrow Payment Protection

Access your dashboard: {2}

Support: {3} | {4}
Copyright (C) {5} KhedutRise. All Rights Reserved.
", safeName, userRole, WebsiteUrl, SupportEmail, SupportPhone, DateTime.Now.Year);

            string content = string.Format(@"
<h2 style='margin:0 0 16px;color:#0f172a;font-size:22px;font-weight:700;'>Welcome to KhedutRise!</h2>
<p style='margin:0 0 16px;color:#334155;font-size:15px;line-height:1.6;'>
  Hello <strong>{0}</strong>,
</p>
<p style='margin:0 0 20px;color:#334155;font-size:15px;line-height:1.6;'>
  Congratulations! Your <strong>{1}</strong> account has been successfully verified and is now fully active. We are thrilled to welcome you to Gujarat's leading agricultural commerce platform.
</p>

<!-- Feature Box -->
<div style='background:#f0fdf4;border:1.5px solid #86efac;border-radius:10px;padding:20px;margin:24px 0;'>
  <h3 style='margin:0 0 12px;color:#0b6028;font-size:16px;font-weight:700;'>What You Can Do Next:</h3>
  <ul style='margin:0;padding-left:20px;color:#1e293b;font-size:14px;line-height:1.8;'>
    <li><strong>Explore Live Mandi Rates:</strong> Real-time APMC commodity price updates across 33 districts.</li>
    <li><strong>Direct Harvest Trade:</strong> Connect directly with verified buyers without middlemen cuts.</li>
    <li><strong>Machinery & Tractor Rentals:</strong> Rent equipment on-demand at fair hourly rates.</li>
    <li><strong>Secure Escrow Checkout:</strong> Guaranteed safe transactions for both farmers and buyers.</li>
  </ul>
</div>

<!-- CTA Button -->
<div style='text-align:center;margin:30px 0;'>
  <a href='{2}' style='display:inline-block;background-color:#0b6028;color:#ffffff;text-decoration:none;font-weight:bold;font-size:15px;padding:14px 36px;border-radius:8px;box-shadow:0 3px 6px rgba(11,96,40,0.25);'>
    Access Your Dashboard &rarr;
  </a>
</div>
", HttpUtility.HtmlEncode(safeName), HttpUtility.HtmlEncode(userRole), WebsiteUrl);

            string fullHtml = WrapInBrandedLayout("Account Activated", content);
            return SendBrandedEmail(toEmail, subject, plainText, fullHtml, out errorMessage);
        }

        // =====================================================================
        // 3. PASSWORD RESET EMAIL
        // =====================================================================
        public static bool SendPasswordResetEmail(string toEmail, string fullName, string resetOtp, out string errorMessage, string expiryMinutes = "15")
        {
            string safeName = string.IsNullOrWhiteSpace(fullName) ? "Valued Member" : fullName.Trim();
            string subject  = "KhedutRise - Password Reset Verification Code";

            string plainText = string.Format(
@"KhedutRise - Password Reset
============================================================

Hello {0},

We received a request to reset the password for your KhedutRise account.

YOUR PASSWORD RESET CODE: {1}

This code is valid for {2} minutes.

If you did not request a password reset, please ignore this email or contact support immediately if you suspect unauthorized access.

Support: {3} | {4}
Copyright (C) {5} KhedutRise. All Rights Reserved.
", safeName, resetOtp, expiryMinutes, SupportEmail, SupportPhone, DateTime.Now.Year);

            string content = string.Format(@"
<h2 style='margin:0 0 16px;color:#0f172a;font-size:22px;font-weight:700;'>Password Reset Request</h2>
<p style='margin:0 0 16px;color:#334155;font-size:15px;line-height:1.6;'>
  Hello <strong>{0}</strong>,
</p>
<p style='margin:0 0 20px;color:#334155;font-size:15px;line-height:1.6;'>
  We received a request to reset your password. Use the verification code below to proceed securely with setting a new password:
</p>

<!-- OTP Box -->
<div style='background:#f0fdf4;border:2px dashed #16a34a;border-radius:12px;text-align:center;padding:24px 20px;margin:24px 0;'>
  <p style='margin:0 0 6px;color:#166534;font-size:12px;font-weight:bold;text-transform:uppercase;letter-spacing:1.5px;'>PASSWORD RESET CODE</p>
  <p style='margin:0;color:#0b6028;font-size:36px;font-weight:800;letter-spacing:12px;font-family:""Courier New"",Courier,monospace;'>{1}</p>
  <p style='margin:10px 0 0;color:#475569;font-size:13px;'>
    &#x23F0; Valid for <strong>{2} minutes</strong> &bull; Single-use only
  </p>
</div>

<!-- Security Alert -->
<div style='background:#fffbeb;border-left:4px solid #f59e0b;padding:14px 18px;border-radius:4px;margin:24px 0;'>
  <p style='margin:0;color:#92400e;font-size:13px;line-height:1.5;'>
    <strong>Security Notice:</strong> If you did not initiate this password reset, someone may be attempting to access your account. You can safely ignore this email, or contact support at <a href='mailto:{3}' style='color:#92400e;font-weight:bold;'>{3}</a>.
  </p>
</div>
", HttpUtility.HtmlEncode(safeName), resetOtp, expiryMinutes, SupportEmail);

            string fullHtml = WrapInBrandedLayout("Password Reset", content);
            return SendBrandedEmail(toEmail, subject, plainText, fullHtml, out errorMessage);
        }

        // =====================================================================
        // 4. PASSWORD CHANGED CONFIRMATION EMAIL
        // =====================================================================
        public static bool SendPasswordChangedEmail(string toEmail, string fullName, out string errorMessage)
        {
            string safeName = string.IsNullOrWhiteSpace(fullName) ? "Valued Member" : fullName.Trim();
            string subject  = "KhedutRise - Your Account Password Was Changed";

            string plainText = string.Format(
@"KhedutRise Security Alert
============================================================

Hello {0},

The password for your KhedutRise account ({1}) was successfully changed on {2}.

If you made this change, you can safely disregard this email.
If you did NOT make this change, please contact KhedutRise security immediately at {3}.

Support: {3} | {4}
Copyright (C) {5} KhedutRise. All Rights Reserved.
", safeName, toEmail, DateTime.Now.ToString("dd MMM yyyy, hh:mm tt"), SupportEmail, SupportPhone, DateTime.Now.Year);

            string content = string.Format(@"
<h2 style='margin:0 0 16px;color:#0f172a;font-size:22px;font-weight:700;'>Password Changed Successfully</h2>
<p style='margin:0 0 16px;color:#334155;font-size:15px;line-height:1.6;'>
  Hello <strong>{0}</strong>,
</p>
<p style='margin:0 0 20px;color:#334155;font-size:15px;line-height:1.6;'>
  This is a security confirmation that the password for your KhedutRise account (<code>{1}</code>) was updated on <strong>{2}</strong>.
</p>

<div style='background:#fef2f2;border-left:4px solid #ef4444;padding:14px 18px;border-radius:4px;margin:24px 0;'>
  <p style='margin:0;color:#991b1b;font-size:13px;line-height:1.5;'>
    <strong>Didn't make this change?</strong> Please contact our emergency support immediately at <a href='mailto:{3}' style='color:#991b1b;font-weight:bold;'>{3}</a> or call {4} to secure your account.
  </p>
</div>
", HttpUtility.HtmlEncode(safeName), HttpUtility.HtmlEncode(toEmail), DateTime.Now.ToString("dd MMM yyyy, hh:mm tt"), SupportEmail, SupportPhone);

            string fullHtml = WrapInBrandedLayout("Security Notice", content);
            return SendBrandedEmail(toEmail, subject, plainText, fullHtml, out errorMessage);
        }

        // =====================================================================
        // 5. GENERAL ACCOUNT / TRANSACTION NOTIFICATION EMAIL
        // =====================================================================
        public static bool SendNotificationEmail(
            string toEmail,
            string fullName,
            string subject,
            string messageTitle,
            string messageBody,
            string actionBtnText,
            string actionBtnUrl,
            out string errorMessage)
        {
            string safeName = string.IsNullOrWhiteSpace(fullName) ? "Valued Member" : fullName.Trim();

            string plainText = string.Format(
@"KhedutRise - {0}
============================================================

Hello {1},

{2}

{3}

Website: {4}
Support: {5}
Copyright (C) {6} KhedutRise. All Rights Reserved.
", messageTitle, safeName, messageBody, actionBtnUrl, WebsiteUrl, SupportEmail, DateTime.Now.Year);

            string btnHtml = string.IsNullOrEmpty(actionBtnText) || string.IsNullOrEmpty(actionBtnUrl)
                ? ""
                : string.Format(@"
<div style='text-align:center;margin:28px 0;'>
  <a href='{0}' style='display:inline-block;background-color:#0b6028;color:#ffffff;text-decoration:none;font-weight:bold;font-size:15px;padding:12px 32px;border-radius:8px;'>
    {1} &rarr;
  </a>
</div>", actionBtnUrl, HttpUtility.HtmlEncode(actionBtnText));

            string content = string.Format(@"
<h2 style='margin:0 0 16px;color:#0f172a;font-size:22px;font-weight:700;'>{0}</h2>
<p style='margin:0 0 16px;color:#334155;font-size:15px;line-height:1.6;'>
  Hello <strong>{1}</strong>,
</p>
<div style='color:#334155;font-size:15px;line-height:1.7;margin:16px 0;'>
  {2}
</div>
{3}
", HttpUtility.HtmlEncode(messageTitle), HttpUtility.HtmlEncode(safeName), messageBody, btnHtml);

            string fullHtml = WrapInBrandedLayout(messageTitle, content);
            return SendBrandedEmail(toEmail, subject, plainText, fullHtml, out errorMessage);
        }

        // =====================================================================
        // CORE EMAIL DISPATCH ENGINE WITH EMBEDDED LOGO (CID)
        // =====================================================================
        private static bool SendBrandedEmail(
            string toEmail,
            string subject,
            string plainTextBody,
            string htmlBody,
            out string errorMessage)
        {
            errorMessage = string.Empty;

            string host     = ConfigurationManager.AppSettings["SmtpHost"]     ?? "smtp.gmail.com";
            string portStr  = ConfigurationManager.AppSettings["SmtpPort"]     ?? "587";
            string user     = ConfigurationManager.AppSettings["SmtpUser"]     ?? "";
            string password = ConfigurationManager.AppSettings["SmtpPassword"] ?? "";
            string fromAddr = ConfigurationManager.AppSettings["SmtpFrom"]     ?? user;
            string sslStr   = ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true";

            // Enforce real SMTP credentials
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password)
                || user.Contains("YOUR_") || password.Contains("YOUR_"))
            {
                errorMessage = "SMTP credentials are not configured in Web.config. Please set your Gmail in 'SmtpUser' and 16-character App Password in 'SmtpPassword'.";
                System.Diagnostics.Trace.TraceWarning("[EmailService] " + errorMessage);
                return false;
            }

            int port = 587;
            if (!int.TryParse(portStr, out port)) { port = 587; }

            bool enableSsl = true;
            if (!bool.TryParse(sslStr, out enableSsl)) { enableSsl = true; }

            try
            {
                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials    = new NetworkCredential(user.Trim(), password.Trim());
                    smtp.EnableSsl      = enableSsl;
                    smtp.Timeout        = 20000;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    using (MailMessage mail = new MailMessage())
                    {
                        string cleanFrom = fromAddr.Contains("<") ? ExtractEmail(fromAddr) : fromAddr.Trim();

                        // Official Sender Branding
                        mail.From = new MailAddress(cleanFrom, "KhedutRise");
                        mail.To.Add(new MailAddress(toEmail.Trim()));
                        mail.Subject = subject;

                        // Add Reply-To and Sender headers
                        try { mail.ReplyToList.Add(new MailAddress(SupportEmail, "KhedutRise Support")); } catch { }
                        mail.Headers.Add("X-Mailer", "KhedutRise .NET MailEngine");
                        mail.Headers.Add("X-Entity-Ref-ID", Guid.NewGuid().ToString());

                        // 1. Plain Text Alternative View
                        AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainTextBody, null, MediaTypeNames.Text.Plain);
                        mail.AlternateViews.Add(plainView);

                        // 2. HTML Alternative View with Embedded Logo (CID)
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                        // Resolve local logo file
                        string logoPath = ResolveLogoPath();
                        if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                        {
                            try
                            {
                                LinkedResource logoRes = new LinkedResource(logoPath, "image/png");
                                logoRes.ContentId = "khedutrise_logo";
                                logoRes.TransferEncoding = TransferEncoding.Base64;
                                htmlView.LinkedResources.Add(logoRes);
                            }
                            catch (Exception resEx)
                            {
                                System.Diagnostics.Trace.TraceWarning("[EmailService] Could not embed logo as LinkedResource: " + resEx.Message);
                            }
                        }

                        mail.AlternateViews.Add(htmlView);

                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (SmtpException smtpEx)
            {
                errorMessage = "SMTP delivery to " + toEmail + " failed: " + smtpEx.Message;
                System.Diagnostics.Trace.TraceError("[EmailService] " + errorMessage);
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Email delivery error: " + ex.Message;
                System.Diagnostics.Trace.TraceError("[EmailService] " + errorMessage);
                return false;
            }
        }

        // =====================================================================
        // HTML BUILDERS & PREMIUM TEMPLATES
        // =====================================================================
        private static string BuildOtpEmailContent(string recipientName, string otp, string expiryMinutes, string purpose)
        {
            string purposeLabel = purpose == "LOGIN" ? "Sign In Verification" : "Email Verification";

            return string.Format(@"
<h2 style='margin:0 0 16px;color:#0f172a;font-size:22px;font-weight:700;'>Verify Your Email Address</h2>

<p style='margin:0 0 16px;color:#334155;font-size:15px;line-height:1.6;'>
  Hello <strong>{0}</strong>,
</p>

<p style='margin:0 0 14px;color:#334155;font-size:15px;line-height:1.6;'>
  Welcome to <strong>KhedutRise</strong>!
</p>

<p style='margin:0 0 20px;color:#334155;font-size:15px;line-height:1.6;'>
  We're excited to have you as part of our growing agricultural technology community. To verify your email address and continue securely, please use the One-Time Password (OTP) provided below:
</p>

<!-- OTP Display Section -->
<div style='background-color:#f0fdf4;border:2px dashed #16a34a;border-radius:12px;text-align:center;padding:26px 20px;margin:26px 0;box-shadow:inset 0 1px 3px rgba(0,0,0,0.02);'>
  <p style='margin:0 0 8px;color:#166534;font-size:12px;font-weight:800;letter-spacing:1.8px;text-transform:uppercase;'>
    YOUR VERIFICATION CODE
  </p>
  <div style='margin:0;color:#0b6028;font-size:40px;font-weight:800;letter-spacing:14px;font-family:""Courier New"",Courier,monospace;line-height:1.2;user-select:all;'>
    {1}
  </div>
  <p style='margin:12px 0 0;color:#475569;font-size:13px;font-weight:500;'>
    &#x23F0; This OTP is strictly valid for <strong>{2} minutes</strong> &bull; Single-use only
  </p>
</div>

<!-- Security Section -->
<div style='background-color:#fffbeb;border-left:4px solid #f59e0b;padding:16px 20px;border-radius:6px;margin:26px 0;'>
  <p style='margin:0 0 6px;color:#92400e;font-size:13px;font-weight:700;'>
    &#x1F6E1;&#xFE0F; Security Information:
  </p>
  <ul style='margin:0;padding-left:18px;color:#78350f;font-size:13px;line-height:1.7;'>
    <li><strong>For your security, never share this OTP with anyone.</strong></li>
    <li>KhedutRise will never ask you to disclose your OTP over a phone call or message.</li>
    <li>If you did not request this verification code, you can safely ignore this email.</li>
  </ul>
</div>
", HttpUtility.HtmlEncode(recipientName), otp, expiryMinutes);
        }

        /// <summary>
        /// Wraps any email content in the official KhedutRise agricultural-themed layout
        /// </summary>
        private static string WrapInBrandedLayout(string pageTitle, string innerContentHtml)
        {
            return string.Format(@"<!DOCTYPE html>
<html lang='en'>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>{0} - KhedutRise</title>
</head>
<body style='margin:0;padding:0;background-color:#f1f5f9;font-family:-apple-system,BlinkMacSystemFont,""Segoe UI"",Roboto,""Helvetica Neue"",Arial,sans-serif;-webkit-font-smoothing:antialiased;'>
  <!-- Outer Wrapper Table -->
  <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='background-color:#f1f5f9;padding:32px 12px;'>
    <tr>
      <td align='center'>
        <!-- Main Email Container (Max 600px) -->
        <table role='presentation' width='100%' cellpadding='0' cellspacing='0' border='0' style='max-width:600px;background-color:#ffffff;border-radius:14px;overflow:hidden;box-shadow:0 10px 25px -5px rgba(0,0,0,0.06),0 8px 10px -6px rgba(0,0,0,0.04);border:1px solid #e2e8f0;'>
          
          <!-- ================= HEADER SECTION ================= -->
          <tr>
            <td style='background:linear-gradient(135deg, #0b6028 0%, #15803d 100%);padding:32px 28px;text-align:center;'>
              <!-- Embedded Logo -->
              <table role='presentation' align='center' cellpadding='0' cellspacing='0' border='0' style='margin:0 auto 12px;'>
                <tr>
                  <td align='center' style='width:64px;height:64px;background-color:#ffffff;border-radius:50%;border:2.5px solid #a7f3d0;box-shadow:0 4px 10px rgba(0,0,0,0.15);overflow:hidden;'>
                    <img src='cid:khedutrise_logo' alt='KhedutRise Logo' width='60' height='60' style='display:block;border-radius:50%;object-fit:cover;' />
                  </td>
                </tr>
              </table>

              <!-- Brand Name -->
              <h1 style='margin:0;color:#ffffff;font-size:27px;font-weight:800;letter-spacing:0.5px;'>
                <span style='color:#86efac;'>Khedut</span>Rise
              </h1>

              <!-- Official Tagline -->
              <p style='margin:8px auto 0;max-width:440px;color:#d1fae5;font-size:13px;line-height:1.5;font-weight:500;'>
                {1}
              </p>
            </td>
          </tr>

          <!-- Green Decorative Accent Bar -->
          <tr>
            <td style='height:4px;background:linear-gradient(90deg, #10b981 0%, #34d399 50%, #10b981 100%);font-size:0;line-height:0;'>&nbsp;</td>
          </tr>

          <!-- ================= MAIN CONTENT SECTION ================= -->
          <tr>
            <td style='padding:38px 34px 30px;background-color:#ffffff;'>
              {2}
            </td>
          </tr>

          <!-- ================= FOOTER SECTION ================= -->
          <tr>
            <td style='background-color:#f8fafc;padding:26px 30px;text-align:center;border-top:1px solid #e2e8f0;'>
              <!-- Footer Brand & Tagline -->
              <p style='margin:0 0 6px;color:#0b6028;font-size:15px;font-weight:700;'>
                KhedutRise
              </p>
              <p style='margin:0 0 14px;color:#64748b;font-size:13px;font-style:italic;'>
                ""{3}""
              </p>

              <!-- Links & Contact -->
              <p style='margin:0 0 10px;color:#475569;font-size:12px;line-height:1.6;'>
                <a href='{4}' style='color:#0b6028;text-decoration:none;font-weight:600;'>Visit Official Website</a> &bull;
                <a href='mailto:{5}' style='color:#0b6028;text-decoration:none;font-weight:600;'>{5}</a> &bull;
                <span style='color:#64748b;'>{6}</span>
              </p>

              <!-- Copyright -->
              <p style='margin:12px 0 0;color:#94a3b8;font-size:11px;line-height:1.5;'>
                Copyright &copy; {7} KhedutRise. All Rights Reserved.<br />
                Gujarat Agri-Commerce &amp; Technology Platform &bull; Empowering Indian Agriculture
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>",
                HttpUtility.HtmlEncode(pageTitle),
                Tagline,
                innerContentHtml,
                FooterTagline,
                WebsiteUrl,
                SupportEmail,
                SupportPhone,
                DateTime.Now.Year);
        }

        private static string ResolveLogoPath()
        {
            string[] candidateFiles = new string[] { "logo_email.png", "logo.png" };

            foreach (string file in candidateFiles)
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Server != null)
                    {
                        string mapped = HttpContext.Current.Server.MapPath("~/Images/" + file);
                        if (File.Exists(mapped)) return mapped;
                    }
                }
                catch { }

                try
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string local = Path.Combine(baseDir, "Images", file);
                    if (File.Exists(local)) return local;

                    string direct = Path.Combine(@"C:\Users\a\Desktop\KhedutRise\KhedutRise_WebForms\Images", file);
                    if (File.Exists(direct)) return direct;
                }
                catch { }
            }

            return null;
        }

        private static string ExtractEmail(string from)
        {
            int start = from.IndexOf('<');
            int end   = from.IndexOf('>');
            if (start >= 0 && end > start)
                return from.Substring(start + 1, end - start - 1).Trim();
            return from.Trim();
        }
    }
}
