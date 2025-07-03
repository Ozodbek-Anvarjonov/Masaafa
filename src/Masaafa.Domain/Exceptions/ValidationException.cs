using System.Net;

namespace Masaafa.Domain.Exceptions;

public class ValidationException : AppException
{
    private const string ErrorType = "validation_failed";
    private const string ErrorTitle = "Validation Error";
    private const string ErrorMessage = "One or more validation failures occurred.";
    private readonly IDictionary<string, string[]> _errors;

    public override string Type => ErrorType;
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public override string Title => ErrorTitle;
    public override IDictionary<string, string[]> Errors => _errors;

    public ValidationException() : base(ErrorMessage) =>
        _errors = new Dictionary<string, string[]>();

    public ValidationException(Exception? inner) : base(ErrorMessage, inner) =>
        _errors = new Dictionary<string, string[]>();

    public ValidationException(IDictionary<string, string[]> errors) : base(ErrorMessage) =>
        _errors = errors ?? new Dictionary<string, string[]>();
}