using Masaafa.Domain.Common.Entities;
using Masaafa.Domain.Enums;

namespace Masaafa.Domain.Entities;

public class User : SoftDeletedEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public string CardCode { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public UserRole Role { get; set; }
}