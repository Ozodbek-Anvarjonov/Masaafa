using System.ComponentModel.DataAnnotations;

namespace Masaafa.Domain.Common.Entities;

public class Entity : IEntity
{
    public Guid Id { get; set; }
}