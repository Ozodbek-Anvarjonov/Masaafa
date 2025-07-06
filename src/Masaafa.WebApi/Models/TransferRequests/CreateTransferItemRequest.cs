namespace Masaafa.WebApi.Models.TransferRequests;

public class CreateTransferItemRequest
{
    public string? Note { get; set; }

    public Guid TransferRequestId { get; set; }
    public Guid FromWarehouseItemId { get; set; }
    public Guid ToWarehouseItemId { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}