using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class TransferRequestItem : SoftDeletedEntity
{
    public Guid TransferRequestId { get; set; }
    public TransferRequest TransferRequest { get; set; } = default!;

    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal LineTotal => Quantity * UnitCost;

    public Guid FromWarehouseId { get; set; }
    public Warehouse FromWarehouse { get; set; } = default!;

    public Guid ToWarehouseId { get; set; }
    public Warehouse ToWarehouse { get; set; } = default!;

    public decimal? SentQuantity { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ReceivedDate { get; set; }

    public TransferItemStatus Status { get; set; } = TransferItemStatus.Pending;
    public string Notes { get; set; }
    public string? RejectionReason { get; set; }
}