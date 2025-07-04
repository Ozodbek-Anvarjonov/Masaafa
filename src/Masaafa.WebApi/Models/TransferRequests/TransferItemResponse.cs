using Masaafa.WebApi.Models.Items;

namespace Masaafa.WebApi.Models.TransferRequests;

public class TransferItemResponse
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public Guid TransferRequestId { get; set; }
    public TransferResponse TransferRequest { get; set; } = default!;

    public Guid ItemId { get; set; }
    public ItemResponse Item { get; set; } = default!;

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public decimal? SentQuantity { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}