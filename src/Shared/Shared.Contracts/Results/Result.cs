namespace Shared.Contracts;

/// <summary>
/// Represents the result of an operation with optional value and error information
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IReadOnlyList<string> Errors { get; }

    protected Result(bool isSuccess, T? value, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors.ToList().AsReadOnly();
    }

    public static Result<T> Success(T value) => new(true, value, Array.Empty<string>());

    public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors);

    public static Result<T> Failure(string error) => new(false, default, new[] { error });
}

/// <summary>
/// Non-generic Result for operations that don't return a value
/// </summary>
public class Result : Result<object>
{
    protected Result(bool isSuccess, IEnumerable<string> errors) 
        : base(isSuccess, null, errors)
    {
    }

    public static Result Success() => new(true, Array.Empty<string>());

    public static new Result Failure(IEnumerable<string> errors) => new(false, errors);

    public static new Result Failure(string error) => new(false, new[] { error });
}
