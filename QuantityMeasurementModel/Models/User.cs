using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Local application user for authentication.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// PBKDF2 password hash (Base64).
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Salt used for hashing (Base64).
        /// </summary>
        public string PasswordSalt { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
         /// <summary>
        /// Role of the user. Allowed values for now: "User", "Admin".
        /// </summary>
        public string Role { get; set; } = "User";
    }
}