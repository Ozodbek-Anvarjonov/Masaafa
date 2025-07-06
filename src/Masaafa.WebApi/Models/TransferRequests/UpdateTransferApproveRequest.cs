namespace Masaafa.WebApi.Models.TransferRequests;

public class UpdateTransferApproveRequest
{
    public Guid Id { get; set; }

    public DateTimeOffset ApproveDate { get; set; }
}