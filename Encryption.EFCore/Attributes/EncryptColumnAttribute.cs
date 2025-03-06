namespace Encryption.EFCore.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class EncryptColumnAttribute : Attribute
{
}