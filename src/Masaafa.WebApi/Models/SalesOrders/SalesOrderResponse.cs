using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Masaafa.WebApi.Models.Users;
using System.Text.Json.Serialization;

namespace Masaafa.WebApi.Models.SalesOrders;

public class SalesOrderResponse
{
    public Guid Id { get; set; }

    public string SalesOrderNumber { get; set; } = default!;
    public string? Note { get; set; }

    public DateTimeOffset DocDate { get; set; }
    public DateTimeOffset DocDueDate { get; set; }

    public Guid? PaymentId { get; set; }
    //public Payment? Payment { get; set; }

    public Guid ClientId { get; set; }
    public ClientResponse Client { get; set; } = default!;

    public string Address { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderConformationStatus ConformationStatus { get; set; } = OrderConformationStatus.New;

    public Guid CreatedByUserId { get; set; }
    public EmployeeResponse CreatedByUser { get; set; } = default!;

    public DateTimeOffset? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public EmployeeResponse? ApprovedByUser { get; set; }

    public DateTimeOffset? RejectedDate { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public EmployeeResponse? RejectedByUser { get; set; }
    public string? RejectionReason { get; set; }

    public bool IsCancelled { get; set; } = false;
    public DateTimeOffset? CancelledDate { get; set; }
    public Guid? CancelledByUserId { get; set; }
    public EmployeeResponse? CancelledByUser { get; set; }
    public string? CancellationReason { get; set; }
}