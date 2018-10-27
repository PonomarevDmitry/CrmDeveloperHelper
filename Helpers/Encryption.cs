using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public static class Encryption
    {
        #region Encrypt

        public static string Encrypt(string password, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] protectedBytes = ProtectedData.Protect(passwordBytes, saltBytes, DataProtectionScope.CurrentUser);

            string protectedPassword = Convert.ToBase64String(protectedBytes);

            return protectedPassword;
        }

        #endregion Encrypt

        #region Decrypt

        public static string Decrypt(string protectedPassword, string salt)
        {
            byte[] protectedBytes = Convert.FromBase64String(protectedPassword);

            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] passwordBytes = ProtectedData.Unprotect(protectedBytes, saltBytes, DataProtectionScope.CurrentUser);

            string password = Encoding.Unicode.GetString(passwordBytes);

            return password;
        }

        #endregion Decrypt

        public static string GenerateSalt()
        {
            var salt = new byte[256];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);

            string saltString = Convert.ToBase64String(salt);

            return saltString;
        }
    }
}
