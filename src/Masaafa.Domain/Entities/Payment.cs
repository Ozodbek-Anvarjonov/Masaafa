using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class Payment : SoftDeletedEntity
{
    public string PaymentNumber { get; set; } = default!;
    public DateTimeOffset PaymentDate { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = default!;

    public PaymentType Type { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.InProcess;

    public string Comment { get; set; } = default!;

    public Guid CreatedById { get; set; }
    public Employee CreatedBy { get; set; }

    public DateTimeOffset? CompletedDate { get; set; }
    public Guid? CompletedById { get; set; }
    public Employee? CompletedBy { get; set; }
}