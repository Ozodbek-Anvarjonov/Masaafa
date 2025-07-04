using Masaafa.Domain.Enums;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Masaafa.WebApi.Models.Users;

public class EmployeeResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; } = default!;

    public string SalesPersonCode { get; set; } = default!;
}