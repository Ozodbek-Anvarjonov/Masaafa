namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferItemSentDate
{
    public Guid Id { get; set; }

    public DateTimeOffset SentDate { get; set; }
}