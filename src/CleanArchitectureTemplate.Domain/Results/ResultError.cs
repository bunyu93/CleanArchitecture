namespace CleanArchitectureTemplate.Domain.Results;

public class ResultError
{
    private ResultError(
        string code,
        string description,
        ErrorType errorType
    )
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType ErrorType { get; }

    public static ResultError Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static ResultError NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static ResultError Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static ResultError Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static ResultError AccessUnAuthorized(string code, string description) =>
        new(code, description, ErrorType.AccessUnAuthorized);

    public static ResultError AccessForbidden(string code, string description) =>
        new(code, description, ErrorType.AccessForbidden);
}
