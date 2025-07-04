namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferItemRequest
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public Guid ItemId { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal? SentQuantity { get; set; }
    public decimal? ReceivedQuantity { get; set; }
    public DateTime? SentDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}