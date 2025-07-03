using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class SalesOrder : SoftDeletedEntity
{
    public string SalesOrderNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTimeOffset DocDate { get; set; }
    public DateTimeOffset DocDueDate { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = default!;

    public string Address { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public OrderConformationStatus ConformationStatus { get; set; } = OrderConformationStatus.New;

    public Guid CreatedByUserId { get; set; }
    public Employee CreatedByUser { get; set; } = default!;

    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Employee? ApprovedByUser { get; set; }

    public DateTime? RejectedDate { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public Employee? RejectedByUser { get; set; }
    public string? RejectionReason { get; set; }

    public bool IsCancelled { get; set; } = false;
    public DateTime? CancelledDate { get; set; }
    public Guid? CancelledByUserId { get; set; }
    public Employee? CancelledByUser { get; set; }
    public string? CancellationReason { get; set; }
}