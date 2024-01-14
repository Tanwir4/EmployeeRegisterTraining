using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTrainingRegistrationServices.Security
{
    public class PasswordHashing
    {
        /* public static byte[] ComputeStringToSha256Hash(string plainText)
         {
             using (SHA256 sha256Hash = SHA256.Create())
             {
                 byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                 return bytes;
             }
         }*/

        public static byte[] ComputeStringToSha256Hash(string plainText, byte[] salt)
        {
            // Combine the password and salt
            byte[] combinedBytes = Encoding.UTF8.GetBytes(plainText).Concat(salt).ToArray();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash using the combined bytes
                byte[] hashBytes = sha256Hash.ComputeHash(combinedBytes);
                return hashBytes;
            }
        }

        public static byte[] GenerateSalt()
        {
            // Generate a random salt of a suitable length (e.g., 16 bytes)
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        public static bool VerifyPassword(string plainText, byte[] storedHashedPassword,byte[] storedSalt)
        {
            // Combine the password and salt
            byte[] combinedBytes = Encoding.UTF8.GetBytes(plainText).Concat(storedSalt).ToArray();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash using the combined bytes
                byte[] hashBytes = sha256Hash.ComputeHash(combinedBytes);
                return hashBytes.SequenceEqual(storedHashedPassword);
            }
        }
    }
}
