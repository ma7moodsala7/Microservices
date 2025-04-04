namespace Common.Models;

public class Result
{
    public bool IsSuccess { get; private set; }
    public IEnumerable<string> Errors { get; private set; }

    private Result(bool isSuccess, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}
