namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferItemReceiveDate
{
    public Guid Id { get; set; }

    public DateTimeOffset ReceiveDate { get; set; }
}