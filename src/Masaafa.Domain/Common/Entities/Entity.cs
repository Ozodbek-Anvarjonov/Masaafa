using System.ComponentModel.DataAnnotations;

namespace Masaafa.Domain.Common.Entities;

public class Entity : IEntity
{
    [Key]
    public Guid Id { get; protected set; }
}