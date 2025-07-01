namespace Masaafa.Domain.Common.Entities;

public interface IAuditableEntity : IEntity
{
    DateTimeOffset CreatedAt { get; set; }
    Guid? CreatedBy { get; set; }

    DateTimeOffset? ModifiedAt { get; set; }
    Guid? ModifiedBy { get; set; }
}