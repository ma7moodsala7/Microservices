namespace Shared.Domain;

/// <summary>
/// Interface for soft-deletable entities
/// </summary>
public interface ISoftDeletableEntity : IEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
    void MarkAsDeleted(string userId);
}