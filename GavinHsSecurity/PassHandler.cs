using System.Security.Cryptography;
using DataAccess;

namespace GavinHsSecurity
{
    public class PassHandler
    {
        //This will handle my password for managing the sites data
        private PassDA _passDA;

        // How many bytes for the salt
        private const int SaltSize = 16;
        // How many bytes for the hash
        private const int HashSize = 32;
        // How many iterations for PBKDF2
        private const int Iterations = 100_000;

        public bool VerifyPassword(string password)
        {
            _passDA = new PassDA();

            string[] parts = _passDA.GetPassword();

            if (parts.Length == 2)
            {
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHash = Convert.FromBase64String(parts[1]);

                byte[] computedHash;
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    computedHash = pbkdf2.GetBytes(HashSize);
                }

                // Compare hashes securely
                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }

            return false;
        }

    }
}
