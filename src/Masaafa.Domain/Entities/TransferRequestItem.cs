using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class TransferRequestItem : SoftDeletedEntity
{
    public string? Note { get; set; }

    public Guid TransferRequestId { get; set; }
    public TransferRequest TransferRequest { get; set; } = default!;

    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public decimal? SentQuantity { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}