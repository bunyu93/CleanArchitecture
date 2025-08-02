using System;

namespace CleanArchitectureTemplate.Domain.Results;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<ResultError, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error!);
    }

    public static T Match<T, TValue>(
        this Result<TValue> result,
        Func<TValue, T> onSuccess,
        Func<ResultError, T> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error!);
    }
}

