using System.Security.Cryptography;
using System.Text;

namespace Encryption.EFCore;

public interface IEncryptionProvider
{
    public string Encrypt(string textToEncrypt);
    public string Decrypt(string textToDecrypt);
}

public class GenerateEncryptionProvider(string encryptionKey) : IEncryptionProvider
{
    private readonly string _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey), "Please initialize your encryption key.");
    
    public string Encrypt(string textToEncrypt)
    {
        if (string.IsNullOrEmpty(textToEncrypt)) return string.Empty;

        var iv = new byte[16];

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
        aes.IV = iv;
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var streamWriter = new StreamWriter(cryptoStream);
        streamWriter.Write(textToEncrypt);
        
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string textToDecrypt)
    {
        if (string.IsNullOrEmpty(textToDecrypt)) return string.Empty;
                
        var iv = new byte[16];
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
        aes.IV = iv;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        var buffer = Convert.FromBase64String(textToDecrypt);
        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader((Stream)cryptoStream);
        return streamReader.ReadToEnd();
    }
}