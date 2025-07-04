using Masaafa.Domain.Enums;
using Masaafa.WebApi.Models.SalesOrders;
using Masaafa.WebApi.Models.Users;
using System.Text.Json.Serialization;

namespace Masaafa.WebApi.Models.Payments;

public class PaymentResponse
{
    public Guid Id { get; set; }

    public string PaymentNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTimeOffset PaymentDate { get; set; }

    public Guid ClientId { get; set; }
    public ClientResponse Client { get; set; } = default!;

    public Guid SalesOrderId { get; set; }
    public SalesOrderResponse SalesOrder { get; set; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentType Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; } = PaymentStatus.InProcess;

    public DateTimeOffset CreatedDate { get; set; }
    public Guid CreatedByUserId { get; set; }
    public EmployeeResponse CreatedByUser { get; set; } = default!;

    public DateTimeOffset? CompletedDate { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public EmployeeResponse? CompletedByUser { get; set; }

    public DateTimeOffset? RejectedDate { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public EmployeeResponse? RejectedByUser { get; set; }
    public string? RejectionReason { get; set; }
}