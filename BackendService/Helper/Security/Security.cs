using System;
using System.Security.Cryptography;

namespace BackendService.Helper.Security
{
    public class InvalidHashException : Exception
    {
        public InvalidHashException()
        {
        }

        public InvalidHashException(string message)
            : base(message) { }

        public InvalidHashException(string message, Exception inner)
            : base(message, inner) { }
    }
    public class CannotPerformOperationException : Exception
    {
        public CannotPerformOperationException()
        {
        }

        public CannotPerformOperationException(string message)
            : base(message) { }

        public CannotPerformOperationException(string message, Exception inner)
            : base(message, inner) { }
    }
    public class Security
    {
        // These constants may be changed without breaking existing hashes.
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;

        // These constants define the encoding and may not be changed.
        public const int HASH_SECTIONS = 2;
        public const int SALT_INDEX = 0;
        public const int PBKDF2_INDEX = 1;

        public static string CreateHash(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SALT_BYTES];

            try
            {
                //old code for non core
                //using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider()) {
                //  csprng.GetBytes(salt);
                //}

                //new code for core
                using (var csprng = RandomNumberGenerator.Create())
                {
                    csprng.GetBytes(salt);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CannotPerformOperationException(
                    "Random number generator not available.",
                    ex
                );
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to random number generator.",
                    ex
                );
            }

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTES);

            // format: algorithm:iterations:hashSize:salt:hash
            string parts =
                //PBKDF2_ITERATIONS +
                //":" +
                //hash.Length +
                //":" +
                Convert.ToBase64String(salt) +
                ":" +
                Convert.ToBase64String(hash);
            return parts;
        }

        public static bool VerifyPassword(string password, string goodHash)
        {
            char[] delimiter = { ':' };
            string[] split = goodHash.Split(delimiter);

            if (split.Length != HASH_SECTIONS)
            {
                throw new InvalidHashException(
                    "Fields are missing from the password hash."
                );
            }
            byte[] salt;
            try
            {
                salt = Convert.FromBase64String(split[SALT_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Convert.FromBase64String",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Base64 decoding of salt failed.",
                    ex
                );
            }

            byte[] hash;
            try
            {
                hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Convert.FromBase64String",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Base64 decoding of pbkdf2 output failed.",
                    ex
                );
            }

            byte[] testHash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTES);
            return SlowEquals(hash, testHash);
        }
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
