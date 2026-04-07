using System;
using System.Security.Cryptography;

namespace QuantityMeasurementApi.Security
{
    /// <summary>
    /// PBKDF2-based password hasher with random salt.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;    // 128 bits
        private const int KeySize = 32;     // 256 bits
        private const int Iterations = 100_000;

        public static (string hash, string salt) HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[SaltSize];
            rng.GetBytes(saltBytes);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256);

            byte[] hashBytes = pbkdf2.GetBytes(KeySize);

            string hash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (string.IsNullOrWhiteSpace(salt)) return false;
            if (string.IsNullOrWhiteSpace(hash)) return false;

            byte[] saltBytes;
            byte[] hashBytes;

            try
            {
                saltBytes = Convert.FromBase64String(salt);
                hashBytes = Convert.FromBase64String(hash);
            }
            catch
            {
                return false;
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256);

            byte[] computedHash = pbkdf2.GetBytes(KeySize);
            string computedHashBase64 = Convert.ToBase64String(computedHash);

            return SlowEquals(hash, computedHashBase64);
        }

        private static bool SlowEquals(string a, string b)
        {
            if (a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }

            return diff == 0;
        }
    }
}