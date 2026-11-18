using System.Security.Cryptography;
using System.Text;

namespace AuthUserApi.Services
{
    public class PasswordHasher
    {
        public (string hash, string Salt) HashPassword(string password)
        {
            var saltyBytes = RandomNumberGenerator.GetBytes(16);
            var hmac = new HMACSHA256(saltyBytes);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltyBytes));
        }

        public bool Verify(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var hmac = new HMACSHA256(saltBytes);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var storedHashBytes = Convert.FromBase64String(storedHash);
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
        }
    }
}
