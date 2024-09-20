using System.Security.Cryptography;
using System.Text;

namespace Encryption.EFCore;

public interface IEncryptionProvider
{
    public string Encrypt(string textToEncrypt);
    public string Decrypt(string textToDecrypt);
}

public class GenerateEncryptionProvider(string key) : IEncryptionProvider
{
    public string Encrypt(string textToEncrypt)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException("EncryptionKey", "Please initialize your encryption key.");

        if (string.IsNullOrEmpty(textToEncrypt))
            return string.Empty;
                
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(textToEncrypt);
                    }
                    array = memoryStream.ToArray();
                }
            }
        }
        string result = Convert.ToBase64String(array);
        return result;
    }

    public string Decrypt(string textToDecrypt)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException("EncryptionKey", "Please initialize your encryption key.");

        if (string.IsNullOrEmpty(textToDecrypt))
            return string.Empty;
                
        byte[] iv = new byte[16];
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            var buffer = Convert.FromBase64String(textToDecrypt);
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}