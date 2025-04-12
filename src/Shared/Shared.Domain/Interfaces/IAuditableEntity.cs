namespace Shared.Domain;

/// <summary>
/// Interface for auditable entities
/// </summary>
public interface IAuditableEntity : IEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    string? CreatedBy { get; }
    string? UpdatedBy { get; }
}