using Masaafa.Domain.Enums;

namespace Masaafa.WebApi.Models.Payments;

public class UpdatePaymentRequest
{
    public Guid Id { get; set; }

    public string PaymentNumber { get; set; } = default!;
    public string? Note { get; set; }

    public Guid ClientId { get; set; }
    public Guid SalesOrderId { get; set; }

    public PaymentType Type { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }
    public DateTimeOffset? RejectedDate { get; set; }
    public string? RejectionReason { get; set; }
}