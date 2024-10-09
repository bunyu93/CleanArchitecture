﻿using System;
using System.Text.Json.Serialization;

namespace CleanArchitectureTemplate.Domain.Result;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Error = default;
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Error? Error { get; }

    public static implicit operator Result(Error error) =>
        new(error);

    public static Result Success() =>
        new();

    public static Result Failure(Error error) =>
        new(error);
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(
        TValue value
    ) : base()
    {
        _value = value;
    }

    private Result(
        Error error
    ) : base(error)
    {
        _value = default;
    }

    public TValue Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Value can not be accessed when IsSuccess is false");

    public static implicit operator Result<TValue>(Error error) =>
        new(error);

    public static implicit operator Result<TValue>(TValue value) =>
        new(value);

    public static Result<TValue> Success(TValue value) =>
        new(value);

    public static new Result<TValue> Failure(Error error) =>
        new(error);
}