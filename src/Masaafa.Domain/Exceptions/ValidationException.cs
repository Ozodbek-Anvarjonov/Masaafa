using System.Net;

namespace Masaafa.Domain.Exceptions;

public class ValidationException : AppException
{
    private const string ErrorType = "validation_failed";
    private const string ErrorTitle = "Validation Error";
    private const string ErrorMessage = "One or more validation failures occurred.";

    public override string Type => ErrorType;
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public override string Title => ErrorTitle;
    public override string Detail => Message;

    public ValidationException(string message) : base(GetSafeMessage(message)) { }

    public ValidationException(string message, Exception? inner) : base(GetSafeMessage(message), inner) { }

    private static string GetSafeMessage(string? message) =>
        string.IsNullOrWhiteSpace(message) ? ErrorMessage : message;
}