using System.Net;
using System.Text.RegularExpressions;

namespace Masaafa.Domain.Exceptions;

public class CustomException : AppException
{
    private const string DefaultErrorType = "server_error";
    private const HttpStatusCode DefaultStatusCode = HttpStatusCode.InternalServerError;
    private const string DefaultErrorTitle = "Internal Server Error";
    private const string DefaultErrorMessage = "Internal server error.";

    public override string Type { get; }
    public override HttpStatusCode StatusCode { get; }
    public override string Title { get; }

    public CustomException()
        : this(DefaultErrorMessage, DefaultStatusCode) { }

    public CustomException(string message, HttpStatusCode statusCode)
        : this(message, statusCode, null) { }

    public CustomException(string message, HttpStatusCode statusCode, Exception? inner)
         : this(message, statusCode, null, null, inner) { }

    public CustomException(string message, HttpStatusCode statusCode, string? type = null, string? title = null, Exception? inner = null)
         : base(GetSafeMessage(message), inner)
    {
        StatusCode = ValidateStatusCode(statusCode);
        Type = GetSafeType(type, statusCode);
        Title = GetSafeTitle(title, statusCode);
    }

    private static string GetSafeMessage(string? message) =>
        string.IsNullOrWhiteSpace(message) ? DefaultErrorMessage : message;

    private static string GetSafeType(string? type, HttpStatusCode httpStatusCode) =>
        string.IsNullOrWhiteSpace(type) ? GenerateDefaultType(httpStatusCode) : type;

    private static string GetSafeTitle(string? title, HttpStatusCode statusCode) =>
        string.IsNullOrWhiteSpace(title) ? GenerateDefaultTitle(statusCode) : title;

    private static HttpStatusCode ValidateStatusCode(HttpStatusCode statusCode) =>
        (int)statusCode is >= 400 and < 600
            ? statusCode
            : throw new ArgumentException("Status code must be 4xx or 5xx", nameof(statusCode));

    private static string GenerateDefaultType(HttpStatusCode statusCode) =>
        statusCode.ToString().ToLowerInvariant().Replace(" ", "_");

    private static string GenerateDefaultTitle(HttpStatusCode statusCode) =>
        string.Join(' ', Regex.Split(statusCode.ToString(), @"(?<!^)(?=[A-Z])")).Trim();
}