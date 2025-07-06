namespace Masaafa.WebApi.Models.Payments;

public class UpdatePaymentCompleteRequest
{
    public Guid Id { get; set; }

    public DateTimeOffset CompletedDate { get; set; }
}