namespace Core.EFCore.Core;

public interface IEntity
{
}

public interface IModifiableEntity : IEntity
{
    DateTime CreatedDt { get; set; }
    DateTime? ModifiedDt { get; set; }
}