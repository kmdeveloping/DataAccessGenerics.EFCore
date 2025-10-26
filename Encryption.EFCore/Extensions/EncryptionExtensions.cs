using Encryption.EFCore.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Encryption.EFCore.Extensions;

public static class EncryptionExtensions
{
    public static string GenerateEncryptionKey()
    {
        using var aes = System.Security.Cryptography.Aes.Create();
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key);
    }
    
    public static void UseEncryption(this ModelBuilder modelBuilder, IEncryptionProvider encryptionProvider)
    {
        if (modelBuilder is null)
            throw new ArgumentNullException(nameof(modelBuilder), "There is not ModelBuilder object.");
        if (encryptionProvider is null)
            throw new ArgumentNullException(nameof(encryptionProvider), "You should create encryption provider.");

        var encryptionConverter = new EncryptionConverter(encryptionProvider);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType != typeof(string) || property.IsDiscriminator()) continue;
                
                var attributes = property.PropertyInfo!.GetCustomAttributes(typeof(EncryptColumnAttribute), false);
                if (attributes.Length > 0) property.SetValueConverter(encryptionConverter);
            }
        }

    }

    private static bool IsDiscriminator(this IMutableProperty property) => property.Name == "Discriminator" || property.PropertyInfo == null;
}