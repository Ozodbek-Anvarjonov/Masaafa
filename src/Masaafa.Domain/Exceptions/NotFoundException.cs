using System.Globalization;
using System.Net;

namespace Masaafa.Domain.Exceptions;

public class NotFoundException : AppException
{
    private const string ErrorType = "not_found";
    private const string ErrorTitle = "Not Found";
    private const string ErrorMessage = "The requested resource was not found.";
    private const string ErrorMessageFormat = "{0} with {1} '{2}' is not found.";

    public override string Type => ErrorType;
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public override string Title => ErrorTitle;
    public override string Detail => Message;

    public NotFoundException() : base(ErrorMessage) { }

    public NotFoundException(string message) : base(GetSafeMessage(message)) { }

    public NotFoundException(string message, Exception? inner) : base(GetSafeMessage(message), inner) { }

    public NotFoundException(string entity, string property, string key) : base(CreateEntityMessage(entity, property, key)) { }

    public NotFoundException(string entity, string property, string key, Exception? inner) : base(CreateEntityMessage(entity, property, key), inner) { }

    private static string CreateEntityMessage(string entity, string property, string key)
    {
        var safeEntity = entity?.Trim() ?? throw new ArgumentNullException(nameof(entity));
        var safeProperty = property?.Trim() ?? throw new ArgumentNullException(nameof(property));
        var safeKey = key?.Trim() ?? throw new ArgumentNullException(nameof(key));

        return string.Format(
                CultureInfo.InvariantCulture,
                ErrorMessageFormat,
                safeEntity,
                safeProperty.ToUpperInvariant(),
                safeKey
            );
    }

    private static string GetSafeMessage(string message) =>
        string.IsNullOrWhiteSpace(message) ? ErrorMessage : message;
}
