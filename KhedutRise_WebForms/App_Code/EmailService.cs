using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace KhedutRise
{
    // Sends OTP emails via SMTP. All credentials are read from Web.config.
    public static class EmailService
    {
        // Returns true if email was accepted by the SMTP server.
        public static bool SendOtpEmail(string toEmail, string otp, string expiryMinutes = "5")
        {
            string host     = ConfigurationManager.AppSettings["SmtpHost"]     ?? "smtp.gmail.com";
            string portStr  = ConfigurationManager.AppSettings["SmtpPort"]     ?? "587";
            string user     = ConfigurationManager.AppSettings["SmtpUser"]     ?? "";
            string password = ConfigurationManager.AppSettings["SmtpPassword"] ?? "";
            string fromAddr = ConfigurationManager.AppSettings["SmtpFrom"]     ?? user;
            string sslStr   = ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true";

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password)
                || user.Contains("YOUR_") || password.Contains("YOUR_"))
            {
                // SMTP not configured — log and return false so caller shows an error.
                System.Diagnostics.Trace.TraceWarning("[EmailService] SMTP credentials not configured in Web.config.");
                return false;
            }

            int port = 587;
            int.TryParse(portStr, out port);
            bool enableSsl = true;
            bool.TryParse(sslStr, out enableSsl);

            string subject = "Your KhedutRise Verification OTP";
            string body    = BuildHtmlBody(otp, expiryMinutes);

            try
            {
                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(user, password);
                    smtp.EnableSsl   = enableSsl;
                    smtp.Timeout     = 15000;

                    MailMessage mail = new MailMessage();
                    mail.From       = new MailAddress(fromAddr.Contains("<") ? ExtractEmail(fromAddr) : fromAddr,
                                                       "KhedutRise");
                    mail.To.Add(toEmail);
                    mail.Subject    = subject;
                    mail.Body       = body;
                    mail.IsBodyHtml = true;

                    smtp.Send(mail);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[EmailService] SMTP send failed: " + ex.Message);
                return false;
            }
        }

        // Professional HTML email matching KhedutRise green theme.
        private static string BuildHtmlBody(string otp, string expiryMinutes)
        {
            return string.Format(@"
<!DOCTYPE html>
<html>
<head><meta charset='utf-8'></head>
<body style='margin:0;padding:0;background:#f1f5f9;font-family:Arial,sans-serif;'>
  <table width='100%' cellpadding='0' cellspacing='0'>
    <tr><td align='center' style='padding:32px 16px;'>
      <table width='540' cellpadding='0' cellspacing='0' style='background:#ffffff;border-radius:10px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,0.08);'>
        <!-- Header -->
        <tr><td style='background:#0b6028;padding:28px 32px;text-align:center;'>
          <h2 style='margin:0;color:#ffffff;font-size:24px;letter-spacing:1px;'>
            <span style='color:#a7f3d0;'>Khedut</span>Rise
          </h2>
          <p style='margin:4px 0 0;color:#d1fae5;font-size:13px;'>Farmer &amp; Agri Commerce Portal</p>
        </td></tr>
        <!-- Body -->
        <tr><td style='padding:36px 32px;'>
          <p style='margin:0 0 16px;color:#374151;font-size:16px;'>Hello,</p>
          <p style='margin:0 0 24px;color:#374151;font-size:15px;'>
            Use the one-time verification code below to complete your sign-in or registration on <strong>KhedutRise</strong>.
          </p>
          <!-- OTP Box -->
          <div style='background:#f0fdf4;border:2px solid #0b6028;border-radius:8px;text-align:center;padding:24px;margin:0 0 24px;'>
            <p style='margin:0 0 8px;color:#6b7280;font-size:13px;text-transform:uppercase;letter-spacing:1px;'>Your Verification OTP</p>
            <p style='margin:0;color:#0b6028;font-size:40px;font-weight:bold;letter-spacing:12px;font-family:monospace;'>{0}</p>
          </div>
          <p style='margin:0 0 12px;color:#374151;font-size:14px;'>
            &#x23F0; This code is valid for <strong>{1} minutes</strong> from the time of this email.
          </p>
          <p style='margin:0 0 24px;color:#dc2626;font-size:14px;font-weight:bold;'>
            &#x26A0; Do not share this OTP with anyone. KhedutRise will never ask for your OTP.
          </p>
          <p style='margin:0;color:#9ca3af;font-size:13px;'>
            If you did not request this code, please ignore this email. Your account remains secure.
          </p>
        </td></tr>
        <!-- Footer -->
        <tr><td style='background:#f8fafc;padding:18px 32px;text-align:center;border-top:1px solid #e5e7eb;'>
          <p style='margin:0;color:#9ca3af;font-size:12px;'>
            &copy; {2} KhedutRise &mdash; Gujarat Agri Commerce Platform
          </p>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>", otp, expiryMinutes, DateTime.Now.Year);
        }

        // Extract plain email from "Display Name <email@example.com>" format.
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
