using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeTrainingRegistrationServices.Security
{
    public class PasswordHashing
    {
        public static byte[] ComputeStringToSha256Hash(string plainText, byte[] salt)
        {
            byte[] combinedBytes = Encoding.UTF8.GetBytes(plainText).Concat(salt).ToArray();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hashBytes = sha256Hash.ComputeHash(combinedBytes);
                return hashBytes;
            }
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        public static bool VerifyPassword(string plainText, byte[] storedHashedPassword,byte[] storedSalt)
        {
            byte[] combinedBytes = Encoding.UTF8.GetBytes(plainText).Concat(storedSalt).ToArray();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hashBytes = sha256Hash.ComputeHash(combinedBytes);
                return hashBytes.SequenceEqual(storedHashedPassword);
            }
        }
    }
}
