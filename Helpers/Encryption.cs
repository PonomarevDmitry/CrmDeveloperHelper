using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class Encryption
    {
        public static string EncryptionKey = "#$RTYJKxchjkTHFpoDF";

        #region Encrypt
        public static string Encrypt(string clearText, string password)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);

            cs.Close();

            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }
        #endregion

        #region Decrypt
        public static string Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherData, 0, cipherData.Length);

            cs.Close();

            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }
        #endregion

        public enum Algorithm
        {
            SHA1 = 1,
            SHA256 = 2,
            SHA512 = 3,
            MD5 = 4
        }

        private Encryption()
        {
        }

        public static string ComputeHash(string text, Algorithm algo)
        {
            return ComputeHash(text, algo, null);
        }

        public static string ComputeHash(string text, Algorithm algo, byte[] salt)
        {
            return ComputeHash(text, HashFromAlgorithmEnum(algo), salt);
        }

        private static string ComputeHash(string text, HashAlgorithm hash, byte[] salt)
        {
            if (salt == null) { GenerateSalt(ref salt); }

            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] textWithSalt = new byte[textBytes.Length + salt.Length];

            CopyBytes(ref textBytes, ref textWithSalt, null, false);
            CopyBytes(ref salt, ref textWithSalt, textBytes, true);

            byte[] hashBytes = hash.ComputeHash(textWithSalt);
            byte[] hashWithSalt = new byte[hashBytes.Length + salt.Length];

            CopyBytes(ref hashBytes, ref hashWithSalt, null, false);
            CopyBytes(ref salt, ref hashWithSalt, hashBytes, true);

            return Convert.ToBase64String(hashWithSalt);
        }

        public static bool CheckHash(string text, Algorithm algo, string hashed)
        {
            HashAlgorithm hash = HashFromAlgorithmEnum(algo);

            byte[] hashWithSalt = Convert.FromBase64String(hashed);
            int hashSizeBits = hash.HashSize;
            int hashSizeBytes = hashSizeBits / 8;

            if (hashWithSalt.Length < hashSizeBytes) { return false; }

            byte[] salt = new byte[hashWithSalt.Length - hashSizeBytes];

            for (int i = 0; i < salt.Length; i++)
            {
                salt[i] = hashWithSalt[hashSizeBytes + i];
            }

            string expected = ComputeHash(text, hash, salt);

            return (expected.Equals(hashed));
        }

        private static HashAlgorithm HashFromAlgorithmEnum(Algorithm algo)
        {
            HashAlgorithm hash;
            switch (algo)
            {
                case Algorithm.SHA1:
                    hash = new SHA1CryptoServiceProvider();
                    break;
                case Algorithm.SHA256:
                    hash = new SHA256Managed();
                    break;
                case Algorithm.SHA512:
                    hash = new SHA512Managed();
                    break;
                case Algorithm.MD5:
                    hash = new MD5CryptoServiceProvider();
                    break;
                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }
            return hash;
        }

        private static void CopyBytes(ref byte[] source, ref byte[] dest, byte[] prevSource, bool append)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (append)
                {
                    dest[prevSource.Length + i] = source[i];
                }
                else
                {
                    dest[i] = source[i];
                }
            }
        }

        private static void GenerateSalt(ref byte[] salt)
        {
            int min = 4;
            int max = 8;

            Random random = new Random();
            int size = random.Next(min, max);
            salt = new byte[size];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);
        }
    }
}
