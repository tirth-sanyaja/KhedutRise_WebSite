using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace KhedutRise
{
    // Result of generating and dispatching an OTP
    public class OtpRequestResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public int CooldownRemainingSeconds { get; set; }
        public string PlainOtp { get; set; }

        public static OtpRequestResult Ok(string message, string plainOtp = "")
        {
            return new OtpRequestResult 
            { 
                Success = true, 
                Message = message,
                PlainOtp = plainOtp
            };
        }

        public static OtpRequestResult Fail(string message, string errorCode = "ERROR", int cooldownSec = 0)
        {
            return new OtpRequestResult
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                CooldownRemainingSeconds = cooldownSec,
                PlainOtp = string.Empty
            };
        }
    }

    // Result of verifying an OTP
    public class OtpVerificationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int RemainingAttempts { get; set; }
        public bool IsBlocked { get; set; }

        public static OtpVerificationResult Ok(int userId, string fullName, string userRole, string mobileNo, string email)
        {
            return new OtpVerificationResult
            {
                Success = true,
                Message = "Verification successful.",
                UserId = userId,
                FullName = fullName,
                UserRole = userRole,
                MobileNo = mobileNo,
                Email = email
            };
        }

        public static OtpVerificationResult Fail(string message, int remainingAttempts = 0, bool isBlocked = false)
        {
            return new OtpVerificationResult
            {
                Success = false,
                Message = message,
                RemainingAttempts = remainingAttempts,
                IsBlocked = isBlocked
            };
        }
    }

    // Manages the complete Email OTP lifecycle: generate, SMTP dispatch, verify, rate-limit.
    // Pure Email OTP architecture without external SMS dependencies.
    public static class EmailOtpService
    {
        private const int DEFAULT_EXPIRY_MINUTES   = 5;
        private const int DEFAULT_COOLDOWN_SECONDS = 60;
        private const int DEFAULT_MAX_ATTEMPTS     = 5;

        // Generate, store, and email a 6-digit OTP via SMTP
        // purpose = "LOGIN" | "ADMIN_LOGIN" | "REGISTER"
        public static OtpRequestResult GenerateAndSendEmailOtp(string email, string purpose, string clientIp, string requestedRole = null)
        {
            email   = (email ?? "").Trim().ToLower();
            purpose = string.IsNullOrEmpty(purpose) ? "LOGIN" : purpose.Trim().ToUpper();

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[\w\.\-]+@[\w\.\-]+\.\w+$"))
                return OtpRequestResult.Fail("Please enter a valid email address.", "INVALID_EMAIL");

            int expiryMinutes  = GetConfigInt("OtpExpiryMinutes",         DEFAULT_EXPIRY_MINUTES);
            int cooldownSec    = GetConfigInt("OtpResendCooldownSeconds", DEFAULT_COOLDOWN_SECONDS);
            int maxAttempts    = GetConfigInt("OtpMaxAttempts",            DEFAULT_MAX_ATTEMPTS);

            // 1. Check resend cooldown
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
                DateTime created  = Convert.ToDateTime(dtCool.Rows[0]["OTPCreatedAt"]);
                double elapsedSec = (DateTime.Now - created).TotalSeconds;
                if (elapsedSec < cooldownSec)
                {
                    int remaining = cooldownSec - (int)elapsedSec;
                    return OtpRequestResult.Fail(
                        string.Format("Please wait {0} seconds before requesting another OTP.", remaining),
                        "COOLDOWN", remaining);
                }
            }

            // 2. User existence & role validation for LOGIN flows
            int userId = 0;
            if (purpose == "ADMIN_LOGIN")
            {
                string adminSql = "SELECT TOP 1 UserId, FullName, Email, UserRole FROM Users WHERE LOWER(Email) = @Email";
                DataTable dtAdmin = DbHelper.ExecuteQuery(adminSql, new SqlParameter[] { new SqlParameter("@Email", email) });

                if (dtAdmin == null || dtAdmin.Rows.Count == 0)
                {
                    // Seed default admin account if not yet created in Users table
                    string insAdmin = @"INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate)
                                        OUTPUT INSERTED.UserId
                                        VALUES ('System Admin', '9876500001', @Email, 'Admin', 'Gandhinagar, Gujarat', 'Platform Superuser', 'Verified', GETDATE())";
                    object newId = DbHelper.ExecuteScalar(insAdmin, new SqlParameter[] { new SqlParameter("@Email", email) });
                    if (newId != null) userId = Convert.ToInt32(newId);
                }
                else
                {
                    if (!dtAdmin.Rows[0]["UserRole"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        return OtpRequestResult.Fail("Access Denied: This account does not possess administrator privileges.", "UNAUTHORIZED_ADMIN");
                    }
                    userId = Convert.ToInt32(dtAdmin.Rows[0]["UserId"]);
                }
            }
            else if (purpose == "LOGIN")
            {
                string userSql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole, Status FROM Users WHERE LOWER(Email) = @Email";
                DataTable dtUser = DbHelper.ExecuteQuery(userSql, new SqlParameter[] { new SqlParameter("@Email", email) });

                if (dtUser == null || dtUser.Rows.Count == 0)
                {
                    return OtpRequestResult.Fail("No registered account found with this email address. Please register first.", "NOT_FOUND");
                }

                string actualRole = dtUser.Rows[0]["UserRole"].ToString();
                if (!string.IsNullOrEmpty(requestedRole) && !actualRole.Equals(requestedRole, StringComparison.OrdinalIgnoreCase))
                {
                    return OtpRequestResult.Fail(string.Format("Role Mismatch: This email is registered as '{0}', not '{1}'. Please select '{0}'.", actualRole, requestedRole), "ROLE_MISMATCH");
                }

                userId = Convert.ToInt32(dtUser.Rows[0]["UserId"]);
            }

            // 3. Invalidate previous unused OTPs for this email + purpose
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET IsUsed = 1 WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0",
                new SqlParameter[] {
                    new SqlParameter("@Email",   email),
                    new SqlParameter("@Purpose", purpose)
                });

            // 4. Generate cryptographically random 6-digit OTP
            string plainOtp = GenerateCryptographicOtp(GetConfigInt("OtpLength", 6));
            string salt      = GenerateCryptographicSalt(16);
            string hash      = ComputeHash(plainOtp, salt);
            DateTime expiry  = DateTime.Now.AddMinutes(expiryMinutes);

            // 5. Store hashed OTP in UserOTPs
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

            // 6. Send OTP via SMTP
            bool sent = EmailService.SendOtpEmail(email, plainOtp, expiryMinutes.ToString());

            // 7. Update delivery status
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
            {
                return OtpRequestResult.Fail(
                    "OTP email could not be sent. Please verify SMTP settings in Web.config or try again.", "SMTP_FAIL");
            }

            return OtpRequestResult.Ok(
                string.Format("OTP sent successfully to {0}. It is valid for {1} minutes.", MaskEmail(email), expiryMinutes),
                plainOtp);
        }

        // Verify submitted OTP
        public static OtpVerificationResult VerifyEmailOtp(string email, string inputOtp, string purpose, string requestedRole = null)
        {
            email   = (email ?? "").Trim().ToLower();
            purpose = string.IsNullOrEmpty(purpose) ? "LOGIN" : purpose.Trim().ToUpper();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(inputOtp))
                return OtpVerificationResult.Fail("Email and OTP are required.");

            // Fetch the latest active OTP record for this email + purpose
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

            DataRow row       = dt.Rows[0];
            int otpId         = Convert.ToInt32(row["OTPId"]);
            int userId        = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0;
            string storedHash = row["OTPHash"].ToString();
            string storedSalt = row["OTPSalt"].ToString();
            DateTime expiry   = Convert.ToDateTime(row["OTPExpiry"]);
            int attempts      = Convert.ToInt32(row["OTPAttempts"]);
            int maxAttempts   = Convert.ToInt32(row["MaxAttempts"]);

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

            // Success — invalidate OTP immediately (one-time use)
            DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id",
                new SqlParameter[] { new SqlParameter("@Id", otpId) });

            // For LOGIN & ADMIN_LOGIN: retrieve user details from Users table
            if (purpose == "LOGIN" || purpose == "ADMIN_LOGIN")
            {
                string userSql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole FROM Users WHERE UserId = @Id";
                DataTable dtUser = DbHelper.ExecuteQuery(userSql,
                    new SqlParameter[] { new SqlParameter("@Id", userId) });

                if (dtUser == null || dtUser.Rows.Count == 0)
                    return OtpVerificationResult.Fail("User account not found. Please contact support.");

                DataRow u = dtUser.Rows[0];
                string actualRole = u["UserRole"].ToString();

                // Role guard for public login
                if (!string.IsNullOrEmpty(requestedRole) && !actualRole.Equals(requestedRole, StringComparison.OrdinalIgnoreCase))
                {
                    return OtpVerificationResult.Fail(string.Format("Role mismatch: Account is '{0}', cannot sign in as '{1}'.", actualRole, requestedRole));
                }

                return OtpVerificationResult.Ok(
                    Convert.ToInt32(u["UserId"]),
                    u["FullName"].ToString(),
                    actualRole,
                    u["MobileNo"] != DBNull.Value ? u["MobileNo"].ToString() : "",
                    u["Email"]    != DBNull.Value ? u["Email"].ToString()    : email);
            }

            // For REGISTER: confirm email verification
            return OtpVerificationResult.Ok(0, "", "", "", email);
        }

        // Mask email for safe UI display: user@domain.com → us****@domain.com
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@")) return email;
            string[] parts = email.Split('@');
            string   local = parts[0];
            string   shown = local.Length <= 2 ? local : local.Substring(0, 2) + new string('*', Math.Min(local.Length - 2, 4));
            return shown + "@" + parts[1];
        }

        // Cryptographically secure numeric OTP generator
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

        // Cryptographically secure salt generator
        private static string GenerateCryptographicSalt(int size)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[size];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        // SHA-256 hash calculation with salt and application pepper
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
