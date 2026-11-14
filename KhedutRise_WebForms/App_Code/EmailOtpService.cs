using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace KhedutRise
{
    // Manages the full Email OTP lifecycle: generate, send, verify, rate-limit.
    // Uses the same UserOTPs table as OtpService, keyed by Email column.
    public static class EmailOtpService
    {
        private const int DEFAULT_EXPIRY_MINUTES  = 5;
        private const int DEFAULT_COOLDOWN_SECONDS = 60;
        private const int DEFAULT_MAX_ATTEMPTS    = 5;

        // ── PUBLIC: Generate, store, and email a new OTP ─────────────────────
        // purpose = "LOGIN" | "REGISTER"
        public static OtpRequestResult GenerateAndSendEmailOtp(string email, string purpose, string clientIp)
        {
            email   = (email ?? "").Trim().ToLower();
            purpose = string.IsNullOrEmpty(purpose) ? "LOGIN" : purpose.Trim().ToUpper();

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[\w\.\-]+@[\w\.\-]+\.\w+$"))
                return OtpRequestResult.Fail("Please enter a valid email address.", "INVALID_EMAIL");

            int expiryMinutes  = GetConfigInt("OtpExpiryMinutes",        DEFAULT_EXPIRY_MINUTES);
            int cooldownSec    = GetConfigInt("OtpResendCooldownSeconds", DEFAULT_COOLDOWN_SECONDS);
            int maxAttempts    = GetConfigInt("OtpMaxAttempts",           DEFAULT_MAX_ATTEMPTS);

            // Check resend cooldown (look for any OTP for this email created within cooldown window)
            string cooldownSql = @"SELECT TOP 1 OTPCreatedAt FROM UserOTPs
                                   WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0
                                   ORDER BY OTPCreatedAt DESC";
            DataTable dtCool = DbHelper.ExecuteQuery(cooldownSql,
                new SqlParameter[] {
                    new SqlParameter("@Email",   email),
                    new SqlParameter("@Purpose", purpose)
                });

            if (dtCool != null && dtCool.Rows.Count > 0)
            {
                DateTime created    = Convert.ToDateTime(dtCool.Rows[0]["OTPCreatedAt"]);
                double elapsedSec   = (DateTime.Now - created).TotalSeconds;
                if (elapsedSec < cooldownSec)
                {
                    int remaining = cooldownSec - (int)elapsedSec;
                    return OtpRequestResult.Fail(
                        string.Format("Please wait {0} seconds before requesting another OTP.", remaining),
                        "COOLDOWN", remaining);
                }
            }

            // For LOGIN: email must be registered in Users table
            int userId = 0;
            if (purpose == "LOGIN")
            {
                string userSql = "SELECT TOP 1 UserId FROM Users WHERE LOWER(Email) = @Email AND Status = 'Verified'";
                DataTable dtUser = DbHelper.ExecuteQuery(userSql,
                    new SqlParameter[] { new SqlParameter("@Email", email) });

                if (dtUser == null || dtUser.Rows.Count == 0)
                    return OtpRequestResult.Fail(
                        "No verified account found with this email address.", "NOT_FOUND");

                userId = Convert.ToInt32(dtUser.Rows[0]["UserId"]);
            }

            // Invalidate all previous unused OTPs for this email + purpose
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET IsUsed = 1 WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0",
                new SqlParameter[] {
                    new SqlParameter("@Email",   email),
                    new SqlParameter("@Purpose", purpose)
                });

            // Generate OTP & hash
            string plainOtp = GenerateCryptographicOtp(GetConfigInt("OtpLength", 6));
            string salt      = GenerateCryptographicSalt(16);
            string hash      = ComputeHash(plainOtp, salt);
            DateTime expiry  = DateTime.Now.AddMinutes(expiryMinutes);

            // Persist to UserOTPs
            string insertSql = @"INSERT INTO UserOTPs
                (UserId, MobileNo, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts,
                 OTPCreatedAt, OTPSentAt, OTPPurpose, IsUsed, UserRole, IPAddress, DeliveryStatus, GatewayResponse)
                VALUES
                (@UserId, 'EMAIL', @Email, @Hash, @Salt, @Expiry, 0, @MaxAtt,
                 GETDATE(), GETDATE(), @Purpose, 0, 'Email', @IP, 'Pending', '')";

            DbHelper.ExecuteNonQuery(insertSql,
                new SqlParameter[] {
                    new SqlParameter("@UserId",  userId),
                    new SqlParameter("@Email",   email),
                    new SqlParameter("@Hash",    hash),
                    new SqlParameter("@Salt",    salt),
                    new SqlParameter("@Expiry",  expiry),
                    new SqlParameter("@MaxAtt",  maxAttempts),
                    new SqlParameter("@Purpose", purpose),
                    new SqlParameter("@IP",      clientIp ?? "")
                });

            // Send OTP via SMTP
            bool sent = EmailService.SendOtpEmail(email, plainOtp, expiryMinutes.ToString());

            // Update delivery status
            string deliveryStatus = sent ? "Sent" : "Failed";
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET DeliveryStatus = @Status WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0 AND OTPHash = @Hash",
                new SqlParameter[] {
                    new SqlParameter("@Status",  deliveryStatus),
                    new SqlParameter("@Email",   email),
                    new SqlParameter("@Purpose", purpose),
                    new SqlParameter("@Hash",    hash)
                });

            if (!sent)
                return OtpRequestResult.Fail(
                    "OTP email could not be sent. Please check SMTP configuration or try again.", "SMTP_FAIL");

            return OtpRequestResult.Ok(
                string.Format("OTP sent to {0}. It is valid for {1} minutes.", MaskEmail(email), expiryMinutes),
                plainOtp);  // plainOtp returned for dev-mode display only; never expose in production UI
        }

        // ── PUBLIC: Verify OTP submitted by user ─────────────────────────────
        // For REGISTER purpose, requestedRole is optional (not role-checked).
        public static OtpVerificationResult VerifyEmailOtp(string email, string inputOtp, string purpose)
        {
            email   = (email ?? "").Trim().ToLower();
            purpose = string.IsNullOrEmpty(purpose) ? "LOGIN" : purpose.Trim().ToUpper();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(inputOtp))
                return OtpVerificationResult.Fail("Email and OTP are required.");

            // Fetch the latest active OTP record for this email+purpose
            string fetchSql = @"SELECT TOP 1 OTPId, UserId, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts
                                 FROM UserOTPs
                                 WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0
                                 ORDER BY OTPCreatedAt DESC";
            DataTable dt = DbHelper.ExecuteQuery(fetchSql,
                new SqlParameter[] {
                    new SqlParameter("@Email",   email),
                    new SqlParameter("@Purpose", purpose)
                });

            if (dt == null || dt.Rows.Count == 0)
                return OtpVerificationResult.Fail("No active OTP found. Please request a new code.");

            DataRow row     = dt.Rows[0];
            int otpId       = Convert.ToInt32(row["OTPId"]);
            int userId      = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0;
            string storedHash = row["OTPHash"].ToString();
            string storedSalt = row["OTPSalt"].ToString();
            DateTime expiry = Convert.ToDateTime(row["OTPExpiry"]);
            int attempts    = Convert.ToInt32(row["OTPAttempts"]);
            int maxAttempts = Convert.ToInt32(row["MaxAttempts"]);

            // Expiry check
            if (DateTime.Now > expiry)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id",
                    new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("OTP has expired. Please request a new code.");
            }

            // Max attempts check
            if (attempts >= maxAttempts)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id",
                    new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("Maximum attempts reached. This OTP is invalid. Please request a new code.", 0, true);
            }

            // Hash verification
            string computedHash = ComputeHash(inputOtp.Trim(), storedSalt);
            if (!string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                int newAttempts = attempts + 1;
                int remaining   = maxAttempts - newAttempts;
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET OTPAttempts = @Att WHERE OTPId = @Id",
                    new SqlParameter[] {
                        new SqlParameter("@Att", newAttempts),
                        new SqlParameter("@Id",  otpId)
                    });
                if (remaining <= 0)
                {
                    DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id",
                        new SqlParameter[] { new SqlParameter("@Id", otpId) });
                    return OtpVerificationResult.Fail("Incorrect OTP. Maximum attempts reached. This OTP is now invalid.", 0, true);
                }
                return OtpVerificationResult.Fail(
                    string.Format("Incorrect OTP. You have {0} attempt(s) remaining.", remaining), remaining);
            }

            // Success — invalidate OTP (one-time use)
            DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id",
                new SqlParameter[] { new SqlParameter("@Id", otpId) });

            // For LOGIN: load user details from Users table
            if (purpose == "LOGIN")
            {
                string userSql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole FROM Users WHERE UserId = @Id";
                DataTable dtUser = DbHelper.ExecuteQuery(userSql,
                    new SqlParameter[] { new SqlParameter("@Id", userId) });

                if (dtUser == null || dtUser.Rows.Count == 0)
                    return OtpVerificationResult.Fail("User account not found. Please contact support.");

                DataRow u = dtUser.Rows[0];
                return OtpVerificationResult.Ok(
                    Convert.ToInt32(u["UserId"]),
                    u["FullName"].ToString(),
                    u["UserRole"].ToString(),
                    u["MobileNo"] != DBNull.Value ? u["MobileNo"].ToString() : "",
                    u["Email"]    != DBNull.Value ? u["Email"].ToString()    : email);
            }

            // For REGISTER: just confirm success (caller saves email to Users table)
            return OtpVerificationResult.Ok(0, "", "", "", email);
        }

        // ── Mask email for display: user@domain.com → us**@domain.com ────────
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@")) return email;
            string[] parts = email.Split('@');
            string   local = parts[0];
            string   shown = local.Length <= 2 ? local : local.Substring(0, 2) + new string('*', Math.Min(local.Length - 2, 4));
            return shown + "@" + parts[1];
        }

        // ── Cryptographic helpers (same pattern as OtpService) ─────────────
        private static string GenerateCryptographicOtp(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                uint val = BitConverter.ToUInt32(bytes, 0);
                int min  = (int)Math.Pow(10, length - 1);
                int max  = (int)Math.Pow(10, length) - 1;
                return ((int)(val % (max - min + 1) + min)).ToString();
            }
        }

        private static string GenerateCryptographicSalt(int size)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[size];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        private static string ComputeHash(string otp, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] input = Encoding.UTF8.GetBytes(otp + ":" + salt + ":KhedutRiseOTP2026");
                byte[] hash  = sha.ComputeHash(input);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private static int GetConfigInt(string key, int fallback)
        {
            string val = ConfigurationManager.AppSettings[key];
            int parsed;
            return (!string.IsNullOrEmpty(val) && int.TryParse(val, out parsed)) ? parsed : fallback;
        }
    }
}
