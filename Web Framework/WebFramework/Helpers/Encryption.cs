namespace WebFramework.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Encryption
    {
        private static readonly Regex _regex = new Regex("[^a-zA-Z0-9]");

        public static string Encrypt(string input, string key, bool isBase64Friendly = false)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
                                    {
                                        Key = Encoding.UTF8.GetBytes(key),
                                        Mode = CipherMode.ECB,
                                        Padding = PaddingMode.PKCS7
                                    };

            var cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
            var array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            tripleDESCryptoServiceProvider.Clear();

            var text = Convert.ToBase64String(array, 0, array.Length);
            if (!isBase64Friendly)
            {
                return text;
            }

            return Encryption._regex.Replace(text, "");
        }

        public static string Decrypt(string input, string key, bool isBase64Friendly = false)
        {
            input = (isBase64Friendly ? Encryption._regex.Replace(input, "") : input);
            var array = Convert.FromBase64String(input);
            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
                                    {
                                        Key = Encoding.UTF8.GetBytes(key),
                                        Mode = CipherMode.ECB,
                                        Padding = PaddingMode.PKCS7
                                    };

            var cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
            var bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            tripleDESCryptoServiceProvider.Clear();

            return Encoding.UTF8.GetString(bytes);
        }
    }
}