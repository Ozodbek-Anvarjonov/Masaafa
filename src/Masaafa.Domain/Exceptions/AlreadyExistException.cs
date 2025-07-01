using System.Globalization;
using System.Net;

namespace Masaafa.Domain.Exceptions;

public class AlreadyExistException : AppException
{
    private const string ErrorType = "resource_already_exists";
    private const string ErrorTitle = "Resource Already Exists";
    private const string ErrorMessage = "The resource you are trying to create already exists.";
    private const string ErrorMessageFormat = "{0} with {1} '{2}' already exists.";

    public override string Type => ErrorType;
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    public override string Title => ErrorTitle;
    public override string Detail => Message;

    public AlreadyExistException() : base(ErrorMessage) { }

    public AlreadyExistException(string message)
        : this(message, null) { }

    public AlreadyExistException(string message, Exception? inner)
        : base(GetSafeMessage(message), inner) { }

    public AlreadyExistException(string entity, string property, string key)
        : this(entity, property, key, null) { }

    public AlreadyExistException(string entity, string property, string key, Exception? inner)
        : base(CreateEntityMessage(entity, property, key), inner) { }

    private static string GetSafeMessage(string message) =>
        string.IsNullOrWhiteSpace(message) ? ErrorMessage : message;

    private static string CreateEntityMessage(string entity, string property, string key)
    {
        var safeEntity = entity?.Trim() ?? throw new ArgumentException(nameof(entity));
        var safeProperty = property?.Trim() ?? throw new ArgumentException(nameof(property));
        var safeKey = key?.Trim() ?? throw new ArgumentException(nameof(key));

        return string.Format(
                CultureInfo.InvariantCulture,
                ErrorMessageFormat,
                safeEntity,
                safeProperty.ToUpperInvariant(),
                safeKey
            );
    }
}
