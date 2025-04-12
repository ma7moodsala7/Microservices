# Shared.Utilities

Common utility functions, guard clauses, and cross-cutting concerns for all microservices.

## 📁 Structure

```
Shared.Utilities/
├── Guards/
│   └── Guard.cs      # Defensive programming helpers
├── Exceptions/
│   └── DomainException.cs  # Base domain exception
```

## 🎯 Purpose

- Provides reusable utility functions
- Implements defensive programming patterns
- Standardizes exception handling

## 🔧 Features

### Guard Clauses
```csharp
public static class Guard
{
    public static class Against
    {
        public static T Null<T>(T value, string paramName)
            where T : class
        {
            if (value is null)
                throw new ArgumentNullException(paramName);
            return value;
        }

        public static string NullOrEmpty(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", paramName);
            return value;
        }
        
        // Additional guard methods...
    }
}
```

### Domain Exceptions
```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}
```

## 🔌 Integration

1. Using guard clauses:
```csharp
public class OrderService
{
    public async Task<Order> CreateOrder(string userId, OrderDetails details)
    {
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        Guard.Against.Null(details, nameof(details));
        
        // Create order...
    }
}
```

2. Using domain exceptions:
```csharp
public class Order
{
    public void Cancel()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Only pending orders can be cancelled.");
            
        Status = OrderStatus.Cancelled;
    }
}
```

## 🎯 Best Practices

- Use guard clauses for parameter validation
- Keep utility functions pure and stateless
- Throw domain-specific exceptions
- Document utility functions well
- Follow single responsibility principle
- Make utilities easy to discover and use
