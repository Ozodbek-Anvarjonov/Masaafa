namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferItemRequest
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}