using Masaafa.Domain.Common.Entities;

namespace Masaafa.Domain.Entities;

public class ItemGroup : SoftDeletedEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;
}