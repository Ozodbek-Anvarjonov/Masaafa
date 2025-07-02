using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class WareHouse : SoftDeletedEntity
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;
}