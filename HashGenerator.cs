
/*  Password hashing using PBKDF2 (SHA256 hashing Algorithm)

    .PBKDF2 Hashing + SHA256 Hashing Algorithm
    .Strong Salting (an array of bytes with a cryptographically strong sequence of random nonzero values)
    .Customizable Hashing and salting Keys' size + hashing iteration times.
    .Hashing process time tracking.

 *  https://github.com/idpNET/PBKDF2_hashing
 */

using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace PBKDF2_hashing
{
    /// <summary>
    /// PBKDF2 hashing using SHA256 Algorithm
    /// </summary>
    internal class HashGenerator
    {
        #region Variables Declaration 
        // Defines Hashing algorithm, number of iterations, and keys' size
        private const int SaltKeySize = 48;
        private const int HashKeySize = 128;
        protected static int Iterations = 10000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;
        #endregion


        #region Class Methods
        /// <summary>
        /// Computes hash value of an input byte[] array
        /// </summary>
        /// <param name="InputPassword"></param>
        /// <param name="InputSaltValue"></param>
        /// <remarks>This overload doesn't takes salt value as input via method parameter</remarks>
        /// <returns>Computed hash value in byte[]</returns>
        protected byte[] ComputeBytesHash(string inputPassword, out byte[] Salt)
        {
            // Static salt value
            Salt = Encoding.ASCII.GetBytes("default");


            // Computes and returning a hash value (specified KeySize) using System.Security.Cryptography.Rfc2898DeriveBytes
            // class using the input password, salt, number of iterations and the hash algorithm
            byte[] hashValue;
            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(inputPassword), Salt, Iterations, HashAlgorithm))
            {
                hashValue = deriveBytes.GetBytes(HashKeySize);
            }

            return hashValue;
        }

        //Merges all bytes into a string of bytes

        protected static string MergeBytesIntoString(byte[] InputByte)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < InputByte.Length; i++)
            {
                builder.Append(InputByte[i].ToString("x2"));
            }
            return builder.ToString();
        }

        // Keeps tracking of hash processing time
        protected static TimeSpan RunTimeMeasurement(Action codeToExecute)
        {
            var watch = Stopwatch.StartNew();
            codeToExecute();
            watch.Stop();
            return watch.Elapsed;
        }
        #endregion

    }
}

