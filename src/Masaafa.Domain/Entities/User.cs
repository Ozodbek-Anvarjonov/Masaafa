using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class User : SoftDeletedEntity
{
    UserRole Role { get; set; }
}