# Shared.Contracts

Shared Data Transfer Objects (DTOs) and operation result types for consistent API responses across all microservices.

## 📁 Structure

```
Shared.Contracts/
├── Results/
│   ├── Result.cs        # Generic operation result wrapper
│   └── PagedResult.cs   # Pagination response wrapper
```

## 🎯 Purpose

- Provides standardized response types for all API endpoints
- Ensures consistent error handling and pagination across services
- Maintains clean separation between domain models and DTOs

## 🔧 Features

### Result<T> Pattern
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    
    public static Result<T> Success(T data) => new(data);
    public static Result<T> Failure(string error) => new(error);
}
```

### PagedResult<T> for Collections
```csharp
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }
}
```

## 🔌 Integration

1. Return success with data:
```csharp
[HttpGet]
public async Task<Result<UserDto>> GetUser(Guid id)
{
    var user = await _userService.GetByIdAsync(id);
    return Result<UserDto>.Success(user);
}
```

2. Return paginated results:
```csharp
[HttpGet]
public async Task<PagedResult<ProductDto>> GetProducts([FromQuery] int page = 1)
{
    return await _productService.GetPagedAsync(page);
}
```

## 🎯 Best Practices

- Always use `Result<T>` for API responses that can fail
- Use `PagedResult<T>` for any collection that might grow large
- Keep DTOs focused on API contract needs
- Don't include domain logic in DTOs
- Use consistent naming patterns for DTOs (e.g., `EntityNameDto`)
