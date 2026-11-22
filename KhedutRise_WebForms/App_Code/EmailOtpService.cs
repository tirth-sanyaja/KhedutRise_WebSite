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
        public string TargetEmail { get; set; }

        public static OtpRequestResult Ok(string message, string targetEmail = "")
        {
            return new OtpRequestResult 
            { 
                Success = true, 
                Message = message,
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

    // Production-Ready Native .NET Email OTP Architecture
    public static class EmailOtpService
    {
        private const int DEFAULT_EXPIRY_MINUTES   = 5;
        private const int DEFAULT_COOLDOWN_SECONDS = 60;
        private const int DEFAULT_MAX_ATTEMPTS     = 5;
        private const string PEPPER                = "KhedutRise_NativeDotNet_EmailOtp_2026";

        // Generate and send Login OTP to registered email
        public static OtpRequestResult GenerateLoginEmailOtp(
            string mobileOrEmail,
            string userEnteredEmail,
            string requestedRole,
            string clientIp)
        {
            int expiryMinutes = GetConfigInt("OtpExpiryMinutes", DEFAULT_EXPIRY_MINUTES);
            int cooldownSec   = GetConfigInt("OtpCooldownSeconds", DEFAULT_COOLDOWN_SECONDS);
            int maxAttempts   = GetConfigInt("OtpMaxAttempts", DEFAULT_MAX_ATTEMPTS);

            string cleanInput = (mobileOrEmail ?? "").Trim();
            string cleanEmail = (userEnteredEmail ?? "").Trim().ToLower();

            if (string.IsNullOrEmpty(cleanInput) && string.IsNullOrEmpty(cleanEmail))
            {
                return OtpRequestResult.Fail("Please enter your registered mobile number or email address.");
            }

            // 1. Identify user account from database
            string userSql = @"SELECT TOP 1 UserId, FullName, MobileNo, Email, UserRole, Status 
                               FROM Users 
                               WHERE (MobileNo = @Input OR LOWER(Email) = LOWER(@Input) OR (LOWER(Email) = LOWER(@Email) AND @Email <> ''))";
            if (!string.IsNullOrEmpty(requestedRole))
            {
                userSql += " AND UserRole = @Role";
            }

            SqlParameter[] userPrms = new SqlParameter[]
            {
                new SqlParameter("@Input", cleanInput),
                new SqlParameter("@Email", cleanEmail),
                new SqlParameter("@Role", (object)requestedRole ?? DBNull.Value)
            };

            DataTable dtUser = DbHelper.ExecuteQuery(userSql, userPrms);
            if (dtUser == null || dtUser.Rows.Count == 0)
            {
                return OtpRequestResult.Fail(string.Format("No active account found for '{0}' with role '{1}'. Please verify your credentials or register.", cleanInput, requestedRole));
            }

            DataRow uRow = dtUser.Rows[0];
            int userId          = Convert.ToInt32(uRow["UserId"]);
            string dbMobile     = uRow["MobileNo"] != DBNull.Value ? uRow["MobileNo"].ToString().Trim() : cleanInput;
            string targetEmail  = uRow["Email"] != DBNull.Value ? uRow["Email"].ToString().Trim().ToLower() : "";
            string actualRole   = uRow["UserRole"] != DBNull.Value ? uRow["UserRole"].ToString() : requestedRole;

            if (string.IsNullOrEmpty(targetEmail) || !targetEmail.Contains("@"))
            {
                if (!string.IsNullOrEmpty(cleanEmail) && cleanEmail.Contains("@"))
                {
                    targetEmail = cleanEmail;
                    DbHelper.ExecuteNonQuery("UPDATE Users SET Email = @Email WHERE UserId = @UserId",
                        new SqlParameter[] { new SqlParameter("@Email", targetEmail), new SqlParameter("@UserId", userId) });
                }
                else
                {
                    return OtpRequestResult.Fail("No verified email address is linked to this account. Please enter your email address to receive the OTP.");
                }
            }

            // 2. Enforce 60-second cooldown to prevent spamming
            string coolSql = @"SELECT TOP 1 OTPCreatedAt FROM UserOTPs 
                               WHERE (MobileNo = @Mob OR Email = @Email) AND OTPPurpose = 'LOGIN' 
                               ORDER BY OTPCreatedAt DESC";
            DataTable dtCool = DbHelper.ExecuteQuery(coolSql, new SqlParameter[]
            {
                new SqlParameter("@Mob", dbMobile),
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

            // 3. Invalidate old OTPs for this user
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET IsUsed = 1 WHERE (MobileNo = @Mob OR Email = @Email) AND OTPPurpose = 'LOGIN' AND IsUsed = 0",
                new SqlParameter[] { new SqlParameter("@Mob", dbMobile), new SqlParameter("@Email", targetEmail) });

            // 4. Generate secure OTP & Salted Hash
            string plainOtp = GenerateCryptographicOtp(GetConfigInt("OtpLength", 6));
            string salt     = GenerateCryptographicSalt(16);
            string hash     = ComputeHash(plainOtp, salt);
            DateTime expiry = DateTime.Now.AddMinutes(expiryMinutes);

            // 5. Attempt genuine SMTP delivery
            string emailError;
            bool emailSent = EmailService.SendOtpEmail(targetEmail, plainOtp, out emailError, expiryMinutes.ToString());

            if (!emailSent)
            {
                // CRITICAL: Do NOT mark OTP as successful if SMTP delivery fails
                return OtpRequestResult.Fail("Failed to send OTP to your email: " + emailError, "SMTP_ERROR");
            }

            // 6. Save active OTP to UserOTPs table only upon successful email dispatch
            string insOtpSql = @"INSERT INTO UserOTPs
                (UserId, MobileNo, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts,
                 OTPCreatedAt, OTPSentAt, OTPPurpose, IsUsed, UserRole, IPAddress, DeliveryStatus, GatewayResponse)
                VALUES
                (@UserId, @Mob, @Email, @Hash, @Salt, @Expiry, 0, @MaxAtt,
                 GETDATE(), GETDATE(), 'LOGIN', 0, @Role, @IP, 'Sent', '')";

            DbHelper.ExecuteNonQuery(insOtpSql, new SqlParameter[]
            {
                new SqlParameter("@UserId", userId > 0 ? (object)userId : DBNull.Value),
                new SqlParameter("@Mob", dbMobile),
                new SqlParameter("@Email", targetEmail),
                new SqlParameter("@Hash", hash),
                new SqlParameter("@Salt", salt),
                new SqlParameter("@Expiry", expiry),
                new SqlParameter("@MaxAtt", maxAttempts),
                new SqlParameter("@Role", actualRole),
                new SqlParameter("@IP", clientIp ?? "")
            });

            string msg = string.Format("Verification OTP sent successfully to your registered email {0}. Valid for {1} minutes. Please check your inbox and enter the 6-digit code to sign in.", MaskEmail(targetEmail), expiryMinutes);
            return OtpRequestResult.Ok(msg, targetEmail);
        }

        // Verify Login OTP
        public static OtpVerificationResult VerifyLoginOtp(string mobileOrEmail, string inputOtp, string requestedRole)
        {
            string clean = (mobileOrEmail ?? "").Trim();
            string otp = (inputOtp ?? "").Trim();

            if (string.IsNullOrEmpty(clean) || string.IsNullOrEmpty(otp))
            {
                return OtpVerificationResult.Fail("Email/Mobile number and 6-digit OTP are required.");
            }

            if (otp.Length != 6 || !Regex.IsMatch(otp, @"^\d{6}$"))
            {
                return OtpVerificationResult.Fail("OTP must be exactly 6 digits.");
            }

            string fetchSql = @"SELECT TOP 1 OTPId, UserId, MobileNo, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts, UserRole
                                FROM UserOTPs
                                WHERE (MobileNo = @Clean OR LOWER(Email) = LOWER(@Clean)) AND OTPPurpose = 'LOGIN' AND IsUsed = 0
                                ORDER BY OTPCreatedAt DESC";
            DataTable dt = DbHelper.ExecuteQuery(fetchSql, new SqlParameter[] { new SqlParameter("@Clean", clean) });

            if (dt == null || dt.Rows.Count == 0)
            {
                return OtpVerificationResult.Fail("No active OTP found. Please request a new verification OTP.");
            }

            DataRow row       = dt.Rows[0];
            int otpId         = Convert.ToInt32(row["OTPId"]);
            int userId        = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0;
            string targetMail = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
            string targetMob  = row["MobileNo"] != DBNull.Value ? row["MobileNo"].ToString() : "";
            string storedHash = row["OTPHash"].ToString();
            string storedSalt = row["OTPSalt"].ToString();
            DateTime expiry   = Convert.ToDateTime(row["OTPExpiry"]);
            int attempts      = Convert.ToInt32(row["OTPAttempts"]);
            int maxAttempts   = Convert.ToInt32(row["MaxAttempts"]);

            // Expiry check
            if (DateTime.Now > expiry)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("OTP has expired. Please request a new code.");
            }

            // Max attempts check
            if (attempts >= maxAttempts)
            {
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });
                return OtpVerificationResult.Fail("Maximum incorrect attempts reached. This OTP has been invalidated for security. Please request a new code.", 0, true);
            }

            // Verify hash
            string computedHash = ComputeHash(otp, storedSalt);
            if (!string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                attempts++;
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET OTPAttempts = @Att WHERE OTPId = @Id",
                    new SqlParameter[] { new SqlParameter("@Att", attempts), new SqlParameter("@Id", otpId) });

                int remaining = maxAttempts - attempts;
                return OtpVerificationResult.Fail(string.Format("Invalid OTP code. {0} attempt(s) remaining.", remaining), remaining);
            }

            // Mark OTP as used (one-time use enforced)
            DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });

            // Fetch full user record
            DataTable dtUser = DbHelper.ExecuteQuery(
                "SELECT TOP 1 UserId, FullName, UserRole, MobileNo, Email FROM Users WHERE UserId = @UId",
                new SqlParameter[] { new SqlParameter("@UId", userId) });

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                DataRow u = dtUser.Rows[0];
                return OtpVerificationResult.Ok(
                    Convert.ToInt32(u["UserId"]),
                    u["FullName"].ToString(),
                    u["UserRole"].ToString(),
                    u["MobileNo"].ToString(),
                    u["Email"] != DBNull.Value ? u["Email"].ToString() : targetMail);
            }

            return OtpVerificationResult.Ok(userId, "Registered User", requestedRole, targetMob, targetMail);
        }

        // Generate and send OTP for Registration or Password Reset
        public static OtpRequestResult GenerateAndSendEmailOtp(string email, string purpose, string clientIp)
        {
            int expiryMinutes = GetConfigInt("OtpExpiryMinutes", DEFAULT_EXPIRY_MINUTES);
            int cooldownSec   = GetConfigInt("OtpCooldownSeconds", DEFAULT_COOLDOWN_SECONDS);
            int maxAttempts   = GetConfigInt("OtpMaxAttempts", DEFAULT_MAX_ATTEMPTS);

            string targetEmail = (email ?? "").Trim().ToLower();
            if (string.IsNullOrEmpty(targetEmail) || !targetEmail.Contains("@"))
            {
                return OtpRequestResult.Fail("Please enter a valid email address.");
            }

            // Cooldown check
            string coolSql = @"SELECT TOP 1 OTPCreatedAt FROM UserOTPs 
                               WHERE LOWER(Email) = @Email AND OTPPurpose = @Purpose 
                               ORDER BY OTPCreatedAt DESC";
            DataTable dtCool = DbHelper.ExecuteQuery(coolSql, new SqlParameter[]
            {
                new SqlParameter("@Email", targetEmail),
                new SqlParameter("@Purpose", purpose)
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

            // Invalidate old OTPs for this email and purpose
            DbHelper.ExecuteNonQuery(
                "UPDATE UserOTPs SET IsUsed = 1 WHERE LOWER(Email) = @Email AND OTPPurpose = @Purpose AND IsUsed = 0",
                new SqlParameter[] { new SqlParameter("@Email", targetEmail), new SqlParameter("@Purpose", purpose) });

            string plainOtp = GenerateCryptographicOtp(GetConfigInt("OtpLength", 6));
            string salt     = GenerateCryptographicSalt(16);
            string hash     = ComputeHash(plainOtp, salt);
            DateTime expiry = DateTime.Now.AddMinutes(expiryMinutes);

            // Attempt genuine SMTP delivery
            string emailError;
            bool emailSent = EmailService.SendOtpEmail(targetEmail, plainOtp, out emailError, expiryMinutes.ToString());

            if (!emailSent)
            {
                return OtpRequestResult.Fail("Failed to send OTP to your email: " + emailError, "SMTP_ERROR");
            }

            // Save to UserOTPs table
            string insSql = @"INSERT INTO UserOTPs
                (UserId, MobileNo, Email, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts,
                 OTPCreatedAt, OTPSentAt, OTPPurpose, IsUsed, UserRole, IPAddress, DeliveryStatus, GatewayResponse)
                VALUES
                (NULL, 'EMAIL', @Email, @Hash, @Salt, @Expiry, 0, @MaxAtt,
                 GETDATE(), GETDATE(), @Purpose, 0, 'Email', @IP, 'Sent', '')";

            DbHelper.ExecuteNonQuery(insSql, new SqlParameter[]
            {
                new SqlParameter("@Email", targetEmail),
                new SqlParameter("@Hash", hash),
                new SqlParameter("@Salt", salt),
                new SqlParameter("@Expiry", expiry),
                new SqlParameter("@MaxAtt", maxAttempts),
                new SqlParameter("@Purpose", purpose),
                new SqlParameter("@IP", clientIp ?? "")
            });

            string msg = string.Format("Verification OTP sent successfully to {0}. Please check your inbox and enter the 6-digit code below.", MaskEmail(targetEmail));
            return OtpRequestResult.Ok(msg, targetEmail);
        }

        // Verify Registration or General Purpose OTP
        public static OtpVerificationResult VerifyEmailOtp(string email, string inputOtp, string purpose)
        {
            string targetEmail = (email ?? "").Trim().ToLower();
            string otp = (inputOtp ?? "").Trim();

            if (string.IsNullOrEmpty(targetEmail) || string.IsNullOrEmpty(otp))
            {
                return OtpVerificationResult.Fail("Email and 6-digit OTP are required.");
            }

            if (otp.Length != 6 || !Regex.IsMatch(otp, @"^\d{6}$"))
            {
                return OtpVerificationResult.Fail("OTP must be exactly 6 digits.");
            }

            string fetchSql = @"SELECT TOP 1 OTPId, OTPHash, OTPSalt, OTPExpiry, OTPAttempts, MaxAttempts, UserId
                                FROM UserOTPs
                                WHERE LOWER(Email) = @Email AND OTPPurpose = @Purpose AND IsUsed = 0
                                ORDER BY OTPCreatedAt DESC";
            DataTable dt = DbHelper.ExecuteQuery(fetchSql, new SqlParameter[]
            {
                new SqlParameter("@Email", targetEmail),
                new SqlParameter("@Purpose", purpose)
            });

            if (dt == null || dt.Rows.Count == 0)
            {
                return OtpVerificationResult.Fail("No active OTP found. Please request a new verification code.");
            }

            DataRow row       = dt.Rows[0];
            int otpId         = Convert.ToInt32(row["OTPId"]);
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
                return OtpVerificationResult.Fail("Too many invalid attempts. This OTP has been cancelled. Please request a new code.", 0, true);
            }

            string computedHash = ComputeHash(otp, storedSalt);
            if (!string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                attempts++;
                DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET OTPAttempts = @Att WHERE OTPId = @Id",
                    new SqlParameter[] { new SqlParameter("@Att", attempts), new SqlParameter("@Id", otpId) });

                int remaining = maxAttempts - attempts;
                return OtpVerificationResult.Fail(string.Format("Invalid OTP code. {0} attempt(s) remaining.", remaining), remaining);
            }

            // Mark OTP as used
            DbHelper.ExecuteNonQuery("UPDATE UserOTPs SET IsUsed = 1 WHERE OTPId = @Id", new SqlParameter[] { new SqlParameter("@Id", otpId) });

            int uId = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0;
            return OtpVerificationResult.Ok(uId, "", purpose, "", targetEmail);
        }

        // Cryptographically secure 6-digit OTP generator
        public static string GenerateCryptographicOtp(int length = 6)
        {
            if (length <= 0) length = 6;
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                uint rand = BitConverter.ToUInt32(bytes, 0);
                int min = (int)Math.Pow(10, length - 1);
                int max = (int)Math.Pow(10, length) - 1;
                int otp = (int)(min + (rand % (max - min + 1)));
                return otp.ToString(new string('0', length));
            }
        }

        public static string GenerateCryptographicSalt(int bytesCount = 16)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[bytesCount];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        public static string ComputeHash(string plainOtp, string salt)
        {
            using (var sha = SHA256.Create())
            {
                byte[] combined = Encoding.UTF8.GetBytes(plainOtp + ":" + salt + ":" + PEPPER);
                byte[] hash = sha.ComputeHash(combined);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@")) return email;
            string[] parts = email.Split('@');
            string user = parts[0];
            string domain = parts[1];

            if (user.Length <= 2)
            {
                return user.Substring(0, 1) + "***@" + domain;
            }
            return user.Substring(0, 1) + new string('*', Math.Min(user.Length - 2, 5)) + user.Substring(user.Length - 1) + "@" + domain;
        }

        public static string CleanMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return string.Empty;
            string clean = Regex.Replace(mobile, @"[^\d]", "");
            if (clean.StartsWith("91") && clean.Length == 12)
            {
                clean = clean.Substring(2);
            }
            return clean;
        }

        private static int GetConfigInt(string key, int defaultVal)
        {
            string val = ConfigurationManager.AppSettings[key];
            int res;
            return int.TryParse(val, out res) ? res : defaultVal;
        }
    }
}
