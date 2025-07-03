namespace Masaafa.Domain.Entities;

public class Warehouse
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Address { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public IEnumerable<WarehouseItem> Items { get; set; }
}