using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Encryption.EFCore;

internal sealed class EncryptionConverter(IEncryptionProvider encryptionProvider, ConverterMappingHints mappingHints = null)
    : ValueConverter<string, string>(x => encryptionProvider.Encrypt(x), x => encryptionProvider.Decrypt(x), mappingHints);