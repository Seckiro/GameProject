using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class RSAUtility
{
    public static (string PublicKey, string PrivateKey) GenerateRSAKey()
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        return (rsa.ToXmlString(false), rsa.ToXmlString(true));
    }

    public static string Encrypt(string str, string key)
    {
        using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
        {
            using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(str)), outputStream = new MemoryStream())
            {
                int readSize;
                int bufferSize = (rsaProvider.KeySize / 8) - 11;//加密块最大长度限制 会填充11字节
                byte[] buffer = new byte[bufferSize];
                rsaProvider.FromXmlString(key);
                while ((readSize = inputStream.Read(buffer, 0, bufferSize)) > 0)
                {
                    byte[] temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    byte[] encryptedBytes = rsaProvider.Encrypt(temp, false);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return Convert.ToBase64String(outputStream.ToArray());
            }
        }
    }

    public static string Decrypt(string str, string key)
    {
        using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
        {

            using (MemoryStream inputStream = new MemoryStream(Convert.FromBase64String(str)), outputStream = new MemoryStream())
            {
                int readSize;
                int bufferSize = rsaProvider.KeySize / 8;
                byte[] buffer = new byte[bufferSize];
                rsaProvider.FromXmlString(key);
                while ((readSize = inputStream.Read(buffer, 0, bufferSize)) > 0)
                {
                    byte[] temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    byte[] rawBytes = rsaProvider.Decrypt(temp, false);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }
}
