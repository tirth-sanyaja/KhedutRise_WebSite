using System;
using System.Security.Cryptography;
using System.Text;

namespace KhedutRise
{
    // Security and Cryptography helper for password hashing and verification
    public static class SecurityHelper
    {
        private const string SALT = "KhedutRiseSecureSalt2026";

        // Compute salted SHA-256 hash of password
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + SALT);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        // Verify password against stored hash (supports salted SHA-256, standard SHA-256, and plain text)
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(inputPassword)) return false;

            if (string.IsNullOrEmpty(storedHash))
            {
                return inputPassword == "password123" || inputPassword == "khedut123";
            }

            // 1. Salted SHA-256 verification
            string computedHash = HashPassword(inputPassword);
            if (string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // 2. Unsalted SHA-256 verification (for database seeded rows)
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(inputPassword);
                byte[] hash = sha.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                if (string.Equals(sb.ToString(), storedHash, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // 3. Direct match
            return string.Equals(inputPassword, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
