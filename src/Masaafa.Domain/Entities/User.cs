using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public abstract class User : SoftDeletedEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public UserRole Role { get; set; }

    public UserType Type { get; set; }
}