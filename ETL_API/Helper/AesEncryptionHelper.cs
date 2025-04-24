using System.Security.Cryptography;
using System.Text;

namespace ETL_API.Helper
{
    public class AesEncryptionHelper
    {
        //Can use this link to test your secret strinngs - https://mothereff.in/byte-counter
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("GreenstemBusinessSoftwareAPI2383"); //Must Exactly 32 Btyes
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("Gb$$b62633933@##"); //Must Exactly 16 Btyes

        public static string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using (StreamWriter writer = new(cryptoStream))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] buffer = Convert.FromBase64String(encryptedText);

            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader reader = new(cryptoStream);

            return reader.ReadToEnd();
        }
    }
}
