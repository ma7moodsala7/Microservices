namespace Shared.Domain;

/// <summary>
/// Base interface for all entities
/// </summary>
public interface IEntity
{
    Guid Id { get; }
}
