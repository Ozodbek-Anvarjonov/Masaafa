using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class Payment : SoftDeletedEntity
{
    public string PaymentNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTimeOffset PaymentDate { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = default!;

    public Guid SalesOrderId { get; set; }
    public SalesOrder SalesOrder { get; set; } = default!;

    public PaymentType Type { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.InProcess;

    public DateTimeOffset CreatedDate { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Employee CreatedByUser { get; set; }

    public DateTimeOffset? CompletedDate { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public Employee? CompletedByUser { get; set; }
}