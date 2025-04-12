# Shared.Persistence

Generic repository pattern implementation and persistence utilities for Entity Framework Core based data access.

## 📁 Structure

```
Shared.Persistence/
├── Repositories/
│   ├── IRepository.cs       # Generic repository interface
│   └── GenericRepository.cs # EF Core implementation
```

## 🎯 Purpose

- Provides consistent data access patterns
- Implements repository pattern with EF Core
- Supports common CRUD operations

## 🔧 Features

### Generic Repository Interface
```csharp
public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Guid id);
}
```

### EF Core Implementation
```csharp
public class GenericRepository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    // Implementation of IRepository<T> methods
}
```

## 🔌 Integration

1. Register in DI container:
```csharp
services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
```

2. Create entity-specific repository:
```csharp
public interface IOrderRepository : IRepository<Order>
{
    Task<IReadOnlyList<Order>> GetPendingOrdersAsync();
}

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }
    
    public async Task<IReadOnlyList<Order>> GetPendingOrdersAsync()
    {
        return await _dbSet
            .Where(o => o.Status == OrderStatus.Pending)
            .ToListAsync();
    }
}
```

## 🎯 Best Practices

- Use the generic repository for basic CRUD
- Create specific repositories for complex queries
- Always use async operations
- Implement proper error handling
- Use transactions when needed
- Follow EF Core best practices
- Keep repositories focused on data access

## 📚 Dependencies

- Microsoft.EntityFrameworkCore
- Shared.Domain (for IEntity)
