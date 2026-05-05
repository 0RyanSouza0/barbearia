namespace Barbearia.Application.Comom;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; set; }
    public string Message { get; set; }

    public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
    public static Result<T> Failure(string message) => new Result<T> { IsSuccess = false, Message = message };

}

