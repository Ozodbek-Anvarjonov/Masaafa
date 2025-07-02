using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class TransferRequest : SoftDeletedEntity
{
    public DateTime RequestDate { get; set; }

    public string RequestNumber { get; set; } = default!;

    public int FromWarehouseId { get; set; }
    public Warehouse FromWarehouse { get; set; } = default!;

    public int ToWarehouseId { get; set; }
    public Warehouse ToWarehouse { get; set; } = default!;

    public TransferRequestStatus Status { get; set; } = TransferRequestStatus.New;
    public TransferProcessStatus ProcessStatus { get; set; } = TransferProcessStatus.OnProcess;

    public string? RejectionReason { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public Employee CreatedBy { get; set; } = default!;

    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public Employee? ApprovedBy { get; set; }

    public IEnumerable<TransferRequestItem> Items { get; set; }
}