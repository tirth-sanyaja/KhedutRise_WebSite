using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace KhedutRise
{
    // Native .NET SMTP Email Service for genuine OTP delivery
    public static class EmailService
    {
        // Sends genuine OTP email via standard .NET SmtpClient.
        // Returns true if accepted by the SMTP server, false with errorMessage if failed.
        public static bool SendOtpEmail(string toEmail, string otp, out string errorMessage, string expiryMinutes = "5")
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(toEmail) || !toEmail.Contains("@"))
            {
                errorMessage = "Invalid recipient email address.";
                return false;
            }

            string host     = ConfigurationManager.AppSettings["SmtpHost"]     ?? "smtp.gmail.com";
            string portStr  = ConfigurationManager.AppSettings["SmtpPort"]     ?? "587";
            string user     = ConfigurationManager.AppSettings["SmtpUser"]     ?? "";
            string password = ConfigurationManager.AppSettings["SmtpPassword"] ?? "";
            string fromAddr = ConfigurationManager.AppSettings["SmtpFrom"]     ?? user;
            string sslStr   = ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true";

            // Enforce real SMTP configuration
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password)
                || user.Contains("YOUR_") || password.Contains("YOUR_"))
            {
                errorMessage = "SMTP credentials are not configured in Web.config. Please configure your Gmail address in 'SmtpUser' and Google App Password (16 characters) in 'SmtpPassword' to deliver real emails.";
                System.Diagnostics.Trace.TraceWarning("[EmailService] " + errorMessage);
                return false;
            }

            int port = 587;
            if (!int.TryParse(portStr, out port)) { port = 587; }

            bool enableSsl = true;
            if (!bool.TryParse(sslStr, out enableSsl)) { enableSsl = true; }

            string subject = "KhedutRise - Your Verification OTP Code";
            string body    = BuildHtmlBody(otp, expiryMinutes);

            try
            {
                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(user, password);
                    smtp.EnableSsl   = enableSsl;
                    smtp.Timeout     = 15000;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    MailMessage mail = new MailMessage();
                    string cleanFrom = fromAddr.Contains("<") ? ExtractEmail(fromAddr) : fromAddr;
                    mail.From       = new MailAddress(cleanFrom, "KhedutRise");
                    mail.To.Add(new MailAddress(toEmail.Trim()));
                    mail.Subject    = subject;
                    mail.Body       = body;
                    mail.IsBodyHtml = true;

                    smtp.Send(mail);
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

        // Overload for backward compatibility
        public static bool SendOtpEmail(string toEmail, string otp, string expiryMinutes = "5")
        {
            string error;
            return SendOtpEmail(toEmail, otp, out error, expiryMinutes);
        }

        // Professional HTML email matching KhedutRise agricultural green theme
        private static string BuildHtmlBody(string otp, string expiryMinutes)
        {
            return string.Format(@"
<!DOCTYPE html>
<html>
<head><meta charset='utf-8'></head>
<body style='margin:0;padding:0;background-color:#f1f5f9;font-family:Segoe UI,Helvetica,Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f1f5f9;padding:30px 10px;'>
    <tr><td align='center'>
      <table width='560' cellpadding='0' cellspacing='0' style='background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 12px rgba(0,0,0,0.08);border:1px solid #e2e8f0;'>
        <!-- Header -->
        <tr><td style='background:#0b6028;padding:28px 32px;text-align:center;'>
          <h1 style='margin:0;color:#ffffff;font-size:26px;font-weight:bold;letter-spacing:0.5px;'>
            <span style='color:#a7f3d0;'>Khedut</span>Rise
          </h1>
          <p style='margin:6px 0 0;color:#d1fae5;font-size:13px;letter-spacing:0.5px;'>GUJARAT AGRI COMMERCE &amp; FARMER PORTAL</p>
        </td></tr>
        <!-- Main Content -->
        <tr><td style='padding:36px 32px;'>
          <h2 style='margin:0 0 16px;color:#1e293b;font-size:20px;font-weight:600;'>Email Verification Code</h2>
          <p style='margin:0 0 20px;color:#475569;font-size:15px;line-height:1.5;'>
            You requested a one-time verification code for your <strong>KhedutRise</strong> account. Use the 6-digit code below to complete your authentication:
          </p>
          
          <!-- OTP Box -->
          <div style='background:#f0fdf4;border:2px solid #16a34a;border-radius:10px;text-align:center;padding:22px;margin:24px 0;'>
            <p style='margin:0 0 6px;color:#166534;font-size:12px;font-weight:bold;text-transform:uppercase;letter-spacing:1px;'>One-Time Password (OTP)</p>
            <p style='margin:0;color:#0b6028;font-size:38px;font-weight:bold;letter-spacing:10px;font-family:monospace;'>{0}</p>
          </div>

          <!-- Expiry Notice -->
          <p style='margin:0 0 10px;color:#334155;font-size:14px;'>
            &#x23F0; This OTP is strictly valid for <strong>{1} minutes</strong> and can only be used once.
          </p>
          
          <!-- Security Warning -->
          <div style='background:#fef2f2;border-left:4px solid #ef4444;padding:12px 16px;border-radius:4px;margin:20px 0;'>
            <p style='margin:0;color:#991b1b;font-size:13px;font-weight:600;'>
              &#x26A0; Security Warning: Never share this OTP with anyone. KhedutRise team members or agents will NEVER call or ask for your OTP.
            </p>
          </div>

          <p style='margin:20px 0 0;color:#94a3b8;font-size:13px;'>
            If you did not request this OTP, please disregard this email. No changes will be made to your account.
          </p>
        </td></tr>
        <!-- Footer -->
        <tr><td style='background:#f8fafc;padding:20px 32px;text-align:center;border-top:1px solid #e2e8f0;'>
          <p style='margin:0 0 4px;color:#64748b;font-size:12px;'>
            &copy; {2} KhedutRise. Empowering Gujarat Agriculture.
          </p>
          <p style='margin:0;color:#94a3b8;font-size:11px;'>
            This is an automated system notification. Please do not reply directly to this email.
          </p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>", otp, expiryMinutes, DateTime.Now.Year);
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
