using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class Warehouse : SoftDeletedEntity
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Address { get; set; } = default!;

    public bool IsActive { get; set; } = true;
}