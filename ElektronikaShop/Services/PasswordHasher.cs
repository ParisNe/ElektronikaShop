using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElektronikaShop.Services
{
    public static class PasswordHasher
    {
        private const int Iterations = 120000;

        public static string Hash(string password)
        {
            byte[] salt = new byte[16];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            byte[] hash;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32);
            }

            return string.Format(
                "PBKDF2$ {0}$ {1}$ {2}",
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash))
                .Replace("$ ", "$");
        }

        public static bool Verify(
            string password,
            string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split('$');

                if (parts.Length != 4)
                {
                    return false;
                }

                int iterations = int.Parse(parts[1]);

                byte[] salt =
                    Convert.FromBase64String(parts[2]);

                byte[] expectedHash =
                    Convert.FromBase64String(parts[3]);

                byte[] actualHash;

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256))
                {
                    actualHash =
                        pbkdf2.GetBytes(expectedHash.Length);
                }

                return CompareBytes(
                    actualHash,
                    expectedHash);
            }
            catch
            {
                return false;
            }
        }

        private static bool CompareBytes(
            byte[] first,
            byte[] second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            bool result = true;

            for (int i = 0; i < first.Length; i++)
            {
                result &= first[i] == second[i];
            }

            return result;
        }
    }
}
