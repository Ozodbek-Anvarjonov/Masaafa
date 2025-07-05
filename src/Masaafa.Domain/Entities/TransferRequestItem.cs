using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class TransferRequestItem : SoftDeletedEntity
{
    public string? Note { get; set; }

    public Guid TransferRequestId { get; set; }
    public TransferRequest TransferRequest { get; set; } = default!;

    public Guid FromWarehouseItemId { get; set; }
    public WarehouseItem FromWarehouseItem { get; set; } = default!;

    public Guid ToWarehouseItemId { get; set; }
    public WarehouseItem ToWarehouseItem { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public DateTime? SentDate { get; set; }
    public Guid SendByUserId { get; set; }
    public Employee? SendByUser { get; set; }

    public DateTime? ReceivedDate { get; set; }
    public Guid? ReceivedByUserId { get; set; }
    public Employee? ReceivedByUser { get; set; }
}