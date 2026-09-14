using System;

namespace CleanArchitectureTemplate.Domain.Results;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Error = default;
    }

    protected Result(ResultError error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }

    public ResultError? Error { get; }

    public static Result Success() => new();

    public static Result Failure(ResultError error) => new(error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    public static Result<TValue> Failure<TValue>(ResultError error) => new(error);
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue value) : base()
    {
        _value = value;
    }

    internal Result(ResultError error) : base(error)
    {
        _value = default;
    }

    public TValue Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Value can not be accessed when IsSuccess is false");
}
