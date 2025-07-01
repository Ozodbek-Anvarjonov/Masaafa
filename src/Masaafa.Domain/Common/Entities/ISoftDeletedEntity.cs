namespace Masaafa.Domain.Common.Entities;

public interface ISoftDeletedEntity : IAuditableEntity
{
    DateTimeOffset? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }

    bool IsDeleted { get; set; }
}