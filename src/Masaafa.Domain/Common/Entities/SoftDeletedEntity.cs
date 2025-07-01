namespace Masaafa.Domain.Common.Entities;

public class SoftDeletedEntity : AuditableEntity, ISoftDeletedEntity
{
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}