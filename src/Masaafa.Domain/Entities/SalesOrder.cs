using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class SalesOrder : SoftDeletedEntity
{
    public string SalesOrderNumber { get; set; } = default!;

    public DateTimeOffset DocDate { get; set; }

    public DateTimeOffset DocDueDate { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public OrderConformationStatus ConformationStatus { get; set; } = OrderConformationStatus.New;

    public IEnumerable<SalesOrderItem> Items { get; set; }
}