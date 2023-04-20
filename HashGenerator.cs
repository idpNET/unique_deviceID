
/*  Password hasing using PBKDF2 (SHA256 hashing Algorithm)
 *  +Setting iteration time
 *  +Hashing process time tracking
 *  https://github.com/idpNET/PBKDF2_hashing
 */

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace PBKDF2_hashing
{
    internal class HashGenerator
    {   
        private const int keySize = 24;
        protected static int iterations=10000;
        private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;

        protected static string HashIt(string inputPassword, out byte[] salt)
        {
            salt = Encoding.ASCII.GetBytes("DEFAULT");
            var hashValue = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(inputPassword),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hashValue);
        }

        // Keep tracking of hash processing time
        protected static TimeSpan RunTimeMeasurement(Action codeToExecute)
        {
            var watch = Stopwatch.StartNew();
            codeToExecute();
            watch.Stop();
            return watch.Elapsed;
        }

        
    }
}

