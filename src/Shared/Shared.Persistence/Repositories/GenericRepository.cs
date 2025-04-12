using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace Shared.Persistence;

/// <summary>
/// Generic repository implementation using Entity Framework Core
/// </summary>
public abstract class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly DbContext _context;

    protected GenericRepository(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        if (entity is ISoftDeletableEntity softDeletable)
        {
            softDeletable.MarkAsDeleted("system"); // TODO: Get actual user ID
            _context.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            _context.Set<T>().Remove(entity);
        }
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Set<T>().AnyAsync(e => e.Id == id);
    }
}
