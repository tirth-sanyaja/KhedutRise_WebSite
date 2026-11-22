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
        public string TargetEmail { get; set; }

        public static OtpRequestResult Ok(string message, string plainOtp = "", string targetEmail = "")
        {
            return new OtpRequestResult 
            { 
                Success = true, 
                Message = message,
                PlainOtp = plainOtp,
                TargetEmail = targetEmail
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
                PlainOtp = string.Empty,
                TargetEmail = string.Empty
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

    // Pure Email OTP architecture with mobile number authentication
    public static class EmailOtpService
    {
        private const int DEFAULT_EXPIRY_MINUTES   = 5;
        private const int DEFAULT_COOLDOWN_SECONDS = 60;
        private const int DEFAULT_MAX_ATTEMPTS     = 5;

        // Clean Indian 10-digit mobile number
        public static string CleanMobile(string rawMobile)
        {
            if (string.IsNullOrEmpty(rawMobile)) return string.Empty;
            string digits = Regex.Replace(rawMobile, @"\D", "");
            if (digits.Length == 12 && digits.StartsWith("91"))
            {
                digits = digits.Substring(2);
            }
            if (digits.Length > 10)
            {
                digits = digits.Substring(digits.Length - 10);
            }
            return digits;
        }

        // Generate and dispatch Email OTP for LOGIN (requires mobile number + role)
        public static OtpRequestResult GenerateLoginEmailOtp(string rawMobile, string rawEmail, string requestedRole, string clientIp)
        {
            string mobile = CleanMobile(rawMobile);
            if (string.IsNullOrEmpty(mobile) || !Regex.IsMatch(mobile, @"^[6-9]\d{9}$"))
            {
                return OtpRequestResult.Fail("Please enter a valid 10-digit Indian mobile number.", "INVALID_MOBILE");
            }

            string email = (rawEmail ?? "").Trim().ToLower();
            string role = string.IsNullOrEmpty(requestedRole) ? "Farmer" : requestedRole.Trim();

            // 1. Verify user in database
            string userSql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole, Status FROM Users WHERE MobileNo = @Mob";
            SqlParameter[] userPrms = new SqlParameter[] { new SqlParameter("@Mob", mobile) };
            DataTable dtUser = DbHelper.ExecuteQuery(userSql, userPrms);

            int userId = 0;
            string actualRole = role;
            string registeredEmail = string.Empty;

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                DataRow row = dtUser.Rows[0];
                userId = Convert.ToInt32(row["UserId"]);
                actualRole = row["UserRole"].ToString();

                // Strict role check
                if (!string.IsNullOrEmpty(role) && !actualRole.Equals(role, StringComparison.OrdinalIgnoreCase))
                {
                    return OtpRequestResult.Fail(
                        string.Format("Role Mismatch: Mobile +91 {0} is registered as '{1}', not '{2}'. Please select '{1}'.", mobile, actualRole, role),
                        "ROLE_MISMATCH");
                }

                registeredEmail = row["Email"] != DBNull.Value ? row["Email"].ToString().Trim().ToLower() : "";
                if (string.IsNullOrEmpty(registeredEmail))
                {
                    registeredEmail = !string.IsNullOrEmpty(email) ? email : mobile + "@khedutrise.com";
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Email = @Email WHERE UserId = @Id",
                        new SqlParameter[] { new SqlParameter("@Email", registeredEmail), new SqlParameter("@Id", userId) });
                }
            }
            else
            {
                // Seamless provisioning for new mobile number
                registeredEmail = !string.IsNullOrEmpty(email) ? email : mobile + "@khedutrise.com";
                string defaultName = role + " User";
                string insSql = @"INSERT INTO Users (FullName, MobileNo, Email, UserRole, Location, SpecificField, Status, CreatedDate)
                                  OUTPUT INSERTED.UserId
                                  VALUES (@Name, @Mob, @Email, @Role, 'Gujarat, India', 'Portal User', 'Verified', GETDATE())";
                SqlParameter[] insPrms = new SqlParameter[]
                {
                    new SqlParameter("@Name", defaultName),
                    new SqlParameter("@Mob", mobile),
                    new SqlParameter("@Email", registeredEmail),
                    new SqlParameter("@Role", role)
                };
                object newId = DbHelper.ExecuteScalar(insSql, insPrms);
                if (newId != null) userId = Convert.ToInt32(newId);
            }

            // Target email is strictly the registered email
            string targetEmail = registeredEmail;

            // 2. Cooldown check
            int expiryMinutes = GetConfigInt("OtpExpiryMinutes", DEFAULT_EXPIRY_MINUTES);
            int cooldownSec   = GetConfigInt("OtpResendCooldownSeconds", DEFAULT_COOLDOWN_SECONDS);
            int maxAttempts   = GetConfigInt("OtpMaxAttempts", DEFAULT_MAX_ATTEMPTS);

            string coolSql = @"SELECT TOP 1 OTPCreatedAt FROM UserOTPs 
                              WHERE (MobileNo = @Mob OR Email = @Email) AND OTPPurpose = 'LOGIN' AND IsUsed = 0
                              ORDER BY OTPCreatedAt DESC";
            DataTable dtCool = DbHelper.ExecuteQuery(coolSql, new SqlParameter[]
            {
                new SqlParameter("@Mob", mobile),
                new SqlParameter("@Email", targetEmail)
            });

            if (dtCool != null && dtCool.Rows.Count > 0)
            {
                DateTime created = Convert.ToDateTime(dtCool.Rows[0]["OTPCreatedAt"]);
                double elapsed = (DateTime.Now - created).TotalSeconds;
                if (elapsed < cooldownSec)
                {
                    int rem = cooldownSec - (int)elapsed;
                    return OtpRequestResult.Fail(string.Format("Please wait {0} seconds before requesting a new OTP.", rem), "COOLDOWN", rem);
                }
            }

            // 3. Invalidate old OTPs
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET IsUsed = 1 WHERE (MobileNo = @Mob OR Email = @Email) AND OTPPurpose = 'LOGIN' AND IsUsed = 0",
                new SqlParameter[] { new SqlParameter("@Mob", mobile), new SqlParameter("@Email", targetEmail) });

            // 4. Generate OTP & Hash
            string plainOtp = GenerateCryptographicOtp(GetConfigInt("OtpLength", 6));
            string salt     = GenerateCryptographicSalt(16);
            string hash     = ComputeHash(plainOtp, salt);
            DateTime expiry = DateTime.Now.AddMinutes(expiryMinutes);

            // 5. Save to UserOTPs table
            string insOtpSql = @"INSERT INTO UserOTPs
                (UserId, MobileNo, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts,
                 OTPCreatedAt, OTPSentAt, OTPPurpose, IsUsed, UserRole, IPAddress, DeliveryStatus, GatewayResponse)
                VALUES
                (@UserId, @Mob, @Email, @Hash, @Salt, @Expiry, 0, @MaxAtt,
                 GETDATE(), GETDATE(), 'LOGIN', 0, @Role, @IP, 'Sent', '')";

            DbHelper.ExecuteNonQuery(insOtpSql, new SqlParameter[]
            {
                new SqlParameter("@UserId", userId > 0 ? (object)userId : DBNull.Value),
                new SqlParameter("@Mob", mobile),
                new SqlParameter("@Email", targetEmail),
                new SqlParameter("@Hash", hash),
                new SqlParameter("@Salt", salt),
                new SqlParameter("@Expiry", expiry),
                new SqlParameter("@MaxAtt", maxAttempts),
                new SqlParameter("@Role", actualRole),
                new SqlParameter("@IP", clientIp ?? "")
            });

            // 6. Attempt real email dispatch via SMTP
            EmailService.SendOtpEmail(targetEmail, plainOtp, expiryMinutes.ToString());

            string msg = string.Format("OTP sent successfully to your registered email {0}. Valid for {1} minutes.", MaskEmail(targetEmail), expiryMinutes);
            return OtpRequestResult.Ok(msg, plainOtp, targetEmail);
        }

        // Verify Login OTP by Mobile Number
        public static OtpVerificationResult VerifyLoginOtp(string rawMobile, string inputOtp, string requestedRole)
        {
            string mobile = CleanMobile(rawMobile);
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(inputOtp))
            {
                return OtpVerificationResult.Fail("Mobile number and OTP are required.");
            }

            string fetchSql = @"SELECT TOP 1 OTPId, UserId, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts, UserRole
                                FROM UserOTPs
                                WHERE MobileNo = @Mob AND OTPPurpose = 'LOGIN' AND IsUsed = 0
                                ORDER BY OTPCreatedAt DESC";
            DataTable dt = DbHelper.ExecuteQuery(fetchSql, new SqlParameter[] { new SqlParameter("@Mob", mobile) });

            if (dt == null || dt.Rows.Count == 0)
            {
                return OtpVerificationResult.Fail("No active OTP found. Please click 'Send OTP to Email' first.");
            }

            DataRow row       = dt.Rows[0];
            int otpId         = Convert.ToInt32(row["OTPId"]);
            int userId        = Convert.ToInt32(row["UserId"]);
            string storedHash = row["OTPHash"].ToString();
            string storedSalt = row["OTPSalt"].ToString();
            DateTime expiry   = Convert.ToDateTime(row["OTPExpiry"]);
            int attempts      = Convert.ToInt32(row["OTPAttempts"]);
            int maxAttempts   = Convert.ToInt32(row["MaxAttempts"]);

            // Expiry
            if (DateTime.Now > expiry)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("OTP has expired. Please request a new code.");
            }

            // Max attempts
            if (attempts >= maxAttempts)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("Maximum attempts exceeded. This OTP is invalid.", 0, true);
            }

            // Hash verification
            string computedHash = ComputeHash(inputOtp.Trim(), storedSalt);
            if (!string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                int newAttempts = attempts + 1;
                int remaining = maxAttempts - newAttempts;
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET OTPAttempts = @Att WHERE OTPId = @Id",
                    new SqlParameter[] { new SqlParameter("@Att", newAttempts), new SqlParameter("@Id", otpId) });

                if (remaining <= 0)
                {
                    DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                    return OtpVerificationResult.Fail("Incorrect OTP. Maximum attempts reached.", 0, true);
                }
                return OtpVerificationResult.Fail(string.Format("Incorrect OTP. {0} attempt(s) remaining.", remaining), remaining);
            }

            // Invalidate OTP (one-time use)
            DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });

            // Retrieve user details from Users table
            string uSql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole FROM Users WHERE UserId = @Id";
            DataTable dtU = DbHelper.ExecuteQuery(uSql, new SqlParameter[] { new SqlParameter("@Id", userId) });

            if (dtU == null || dtU.Rows.Count == 0)
            {
                return OtpVerificationResult.Fail("User account not found.");
            }

            DataRow u = dtU.Rows[0];
            string actualRole = u["UserRole"].ToString();

            if (!string.IsNullOrEmpty(requestedRole) && !actualRole.Equals(requestedRole, StringComparison.OrdinalIgnoreCase))
            {
                return OtpVerificationResult.Fail(string.Format("Role mismatch: Account is '{0}', not '{1}'.", actualRole, requestedRole));
            }

            return OtpVerificationResult.Ok(
                Convert.ToInt32(u["UserId"]),
                u["FullName"].ToString(),
                actualRole,
                u["MobileNo"].ToString(),
                u["Email"] != DBNull.Value ? u["Email"].ToString() : "");
        }

        // Generate and dispatch Email OTP for REGISTRATION or ADMIN_LOGIN
        public static OtpRequestResult GenerateAndSendEmailOtp(string email, string purpose, string clientIp, string requestedRole = null)
        {
            email = (email ?? "").Trim().ToLower();
            purpose = string.IsNullOrEmpty(purpose) ? "REGISTER" : purpose.Trim().ToUpper();

            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[\w\.\-]+@[\w\.\-]+\.\w+$"))
            {
                return OtpRequestResult.Fail("Please enter a valid email address.", "INVALID_EMAIL");
            }

            int expiryMinutes = GetConfigInt("OtpExpiryMinutes", DEFAULT_EXPIRY_MINUTES);
            int cooldownSec   = GetConfigInt("OtpResendCooldownSeconds", DEFAULT_COOLDOWN_SECONDS);
            int maxAttempts   = GetConfigInt("OtpMaxAttempts", DEFAULT_MAX_ATTEMPTS);

            // Cooldown check
            string coolSql = @"SELECT TOP 1 OTPCreatedAt FROM UserOTPs 
                               WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0
                               ORDER BY OTPCreatedAt DESC";
            DataTable dtCool = DbHelper.ExecuteQuery(coolSql, new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Purpose", purpose)
            });

            if (dtCool != null && dtCool.Rows.Count > 0)
            {
                DateTime created = Convert.ToDateTime(dtCool.Rows[0]["OTPCreatedAt"]);
                double elapsed = (DateTime.Now - created).TotalSeconds;
                if (elapsed < cooldownSec)
                {
                    int rem = cooldownSec - (int)elapsed;
                    return OtpRequestResult.Fail(string.Format("Please wait {0} seconds before requesting another OTP.", rem), "COOLDOWN", rem);
                }
            }

            int userId = 0;
            if (purpose == "ADMIN_LOGIN")
            {
                string adminSql = "SELECT TOP 1 UserId, FullName, Email, UserRole FROM Users WHERE LOWER(Email) = @Email";
                DataTable dtAdmin = DbHelper.ExecuteQuery(adminSql, new SqlParameter[] { new SqlParameter("@Email", email) });

                if (dtAdmin == null || dtAdmin.Rows.Count == 0)
                {
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
                        return OtpRequestResult.Fail("Access Denied: This email does not possess administrative privileges.", "UNAUTHORIZED_ADMIN");
                    }
                    userId = Convert.ToInt32(dtAdmin.Rows[0]["UserId"]);
                }
            }

            // Invalidate old OTPs
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET IsUsed = 1 WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0",
                new SqlParameter[] { new SqlParameter("@Email", email), new SqlParameter("@Purpose", purpose) });

            string plainOtp = GenerateCryptographicOtp(GetConfigInt("OtpLength", 6));
            string salt     = GenerateCryptographicSalt(16);
            string hash     = ComputeHash(plainOtp, salt);
            DateTime expiry = DateTime.Now.AddMinutes(expiryMinutes);

            string insSql = @"INSERT INTO UserOTPs
                (UserId, MobileNo, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts,
                 OTPCreatedAt, OTPSentAt, OTPPurpose, IsUsed, UserRole, IPAddress, DeliveryStatus, GatewayResponse)
                VALUES
                (@UserId, 'EMAIL', @Email, @Hash, @Salt, @Expiry, 0, @MaxAtt,
                 GETDATE(), GETDATE(), @Purpose, 0, 'Email', @IP, 'Sent', '')";

            DbHelper.ExecuteNonQuery(insSql, new SqlParameter[]
            {
                new SqlParameter("@UserId", userId > 0 ? (object)userId : DBNull.Value),
                new SqlParameter("@Email", email),
                new SqlParameter("@Hash", hash),
                new SqlParameter("@Salt", salt),
                new SqlParameter("@Expiry", expiry),
                new SqlParameter("@MaxAtt", maxAttempts),
                new SqlParameter("@Purpose", purpose),
                new SqlParameter("@IP", clientIp ?? "")
            });

            // Dispatch via SMTP
            EmailService.SendOtpEmail(email, plainOtp, expiryMinutes.ToString());

            string msg = string.Format("OTP sent successfully to {0}. It is valid for {1} minutes.", MaskEmail(email), expiryMinutes);
            return OtpRequestResult.Ok(msg, plainOtp, email);
        }

        // Verify Email OTP for Registration or Admin Login
        public static OtpVerificationResult VerifyEmailOtp(string email, string inputOtp, string purpose, string requestedRole = null)
        {
            email   = (email ?? "").Trim().ToLower();
            purpose = string.IsNullOrEmpty(purpose) ? "REGISTER" : purpose.Trim().ToUpper();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(inputOtp))
            {
                return OtpVerificationResult.Fail("Email and OTP are required.");
            }

            string fetchSql = @"SELECT TOP 1 OTPId, UserId, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts
                                 FROM UserOTPs
                                 WHERE Email = @Email AND OTPPurpose = @Purpose AND IsUsed = 0
                                 ORDER BY OTPCreatedAt DESC";
            DataTable dt = DbHelper.ExecuteQuery(fetchSql, new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Purpose", purpose)
            });

            if (dt == null || dt.Rows.Count == 0)
            {
                return OtpVerificationResult.Fail("No active OTP found. Please request a new code.");
            }

            DataRow row       = dt.Rows[0];
            int otpId         = Convert.ToInt32(row["OTPId"]);
            int userId        = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0;
            string storedHash = row["OTPHash"].ToString();
            string storedSalt = row["OTPSalt"].ToString();
            DateTime expiry   = Convert.ToDateTime(row["OTPExpiry"]);
            int attempts      = Convert.ToInt32(row["OTPAttempts"]);
            int maxAttempts   = Convert.ToInt32(row["MaxAttempts"]);

            if (DateTime.Now > expiry)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("OTP has expired. Please request a new code.");
            }

            if (attempts >= maxAttempts)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("Maximum attempts exceeded.", 0, true);
            }

            string computedHash = ComputeHash(inputOtp.Trim(), storedSalt);
            if (!string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                int newAttempts = attempts + 1;
                int remaining = maxAttempts - newAttempts;
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET OTPAttempts = @Att WHERE OTPId = @Id",
                    new SqlParameter[] { new SqlParameter("@Att", newAttempts), new SqlParameter("@Id", otpId) });
                if (remaining <= 0)
                {
                    DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                    return OtpVerificationResult.Fail("Incorrect OTP. Maximum attempts reached.", 0, true);
                }
                return OtpVerificationResult.Fail(string.Format("Incorrect OTP. {0} attempt(s) remaining.", remaining), remaining);
            }

            // Invalidate OTP immediately
            DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });

            if (purpose == "ADMIN_LOGIN")
            {
                string userSql = "SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole FROM Users WHERE UserId = @Id";
                DataTable dtUser = DbHelper.ExecuteQuery(userSql, new SqlParameter[] { new SqlParameter("@Id", userId) });
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    DataRow u = dtUser.Rows[0];
                    return OtpVerificationResult.Ok(
                        Convert.ToInt32(u["UserId"]),
                        u["FullName"].ToString(),
                        u["UserRole"].ToString(),
                        u["MobileNo"].ToString(),
                        u["Email"].ToString());
                }
            }

            return OtpVerificationResult.Ok(0, "", "", "", email);
        }

        // Mask email: user@gmail.com -> us***@gmail.com
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@")) return email;
            string[] parts = email.Split('@');
            string local = parts[0];
            string shown = local.Length <= 2 ? local : local.Substring(0, 2) + new string('*', Math.Min(local.Length - 2, 4));
            return shown + "@" + parts[1];
        }

        // Cryptographic OTP generator
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

        // Cryptographic Salt
        private static string GenerateCryptographicSalt(int size)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[size];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        // SHA-256 Hash
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
