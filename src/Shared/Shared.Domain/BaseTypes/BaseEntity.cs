namespace Shared.Domain;

/// <summary>
/// Base class for all entities with audit and soft-delete capabilities
/// </summary>
public abstract class BaseEntity : IEntity, IAuditableEntity, ISoftDeletableEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void SetCreatedBy(string userId)
    {
        if (string.IsNullOrEmpty(CreatedBy))
        {
            CreatedBy = userId;
        }
    }

    public void SetUpdatedBy(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public void MarkAsDeleted(string userId)
    {
        if (!IsDeleted)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = userId;
        }
    }
}
