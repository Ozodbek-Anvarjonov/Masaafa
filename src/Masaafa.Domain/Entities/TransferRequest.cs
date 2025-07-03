using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class TransferRequest : SoftDeletedEntity
{
    public string RequestNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTime RequestDate { get; set; }

    public Guid FromWarehouseId { get; set; }
    public Warehouse FromWarehouse { get; set; } = default!;

    public Guid ToWarehouseId { get; set; }
    public Warehouse ToWarehouse { get; set; } = default!;

    public TransferRequestStatus Status { get; set; } = TransferRequestStatus.New;
    public TransferProcessStatus ProcessStatus { get; set; } = TransferProcessStatus.OnProcess;
    
    public Guid CreatedByUserId { get; set; }
    public Employee CreatedByUser { get; set; } = default!;

    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Employee? ApprovedByUser { get; set; }

    public DateTime? RejectedDate { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public Employee? RejectedByUser { get; set; }
    public string? RejectionReason { get; set; }

    public IEnumerable<TransferRequestItem> Items { get; set; }
}