using System;

namespace KhedutRise
{
    /// <summary>
    /// Holds validated user registration data temporarily in session and DB
    /// while waiting for genuine Email OTP verification.
    /// Supports compact serialization for durable persistence across session drops.
    /// </summary>
    [Serializable]
    public class PendingRegistration
    {
        public string Role { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string SpecificField { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }

        public PendingRegistration()
        {
            CreatedAt = DateTime.UtcNow;
            Location = "Gujarat";
        }

        /// <summary>
        /// Serializes registration data into a compact safe pipe-delimited string
        /// that easily fits within UserOTPs.GatewayResponse (nvarchar 500).
        /// </summary>
        public string Serialize()
        {
            string safeRole     = (Role ?? "Farmer").Trim().Replace("|", "");
            string safeName     = (FullName ?? "").Trim().Replace("|", "/");
            string safeMobile   = (MobileNo ?? "").Trim().Replace("|", "");
            string safeEmail    = (Email ?? "").Trim().ToLower().Replace("|", "");
            string safeLoc      = (Location ?? "Gujarat").Trim().Replace("|", "/");
            string safeSpecific = (SpecificField ?? "").Trim().Replace("|", "/");
            string safePassHash = (PasswordHash ?? "").Trim().Replace("|", "");

            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                safeRole, safeName, safeMobile, safeEmail, safeLoc, safeSpecific, safePassHash);
        }

        /// <summary>
        /// Reconstructs PendingRegistration from a serialized string.
        /// </summary>
        public static PendingRegistration Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return null;

            string[] parts = data.Split('|');
            if (parts.Length < 7) return null;

            return new PendingRegistration
            {
                Role          = parts[0],
                FullName      = parts[1],
                MobileNo      = parts[2],
                Email         = parts[3],
                Location      = string.IsNullOrEmpty(parts[4]) ? "Gujarat" : parts[4],
                SpecificField = parts[5],
                PasswordHash  = parts[6],
                CreatedAt     = DateTime.UtcNow
            };
        }
    }
}
