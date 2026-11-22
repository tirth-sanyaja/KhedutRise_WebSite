using System;

namespace KhedutRise
{
    /// <summary>
    /// Holds validated user registration data temporarily in session
    /// while waiting for genuine Email OTP verification.
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
    }
}
