using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .HasData(new Employee
            {
                Id = Guid.Parse("3738e2ff-c8a4-43fd-ad0f-97de71dfa8bb"),
                FirstName = "Director",
                LastName = "Director",
                PhoneNumber = "+998950148306",
                Role = UserRole.SalesDirector,
                Type = UserType.Employee,
                SalesPersonCode = "123456",
            });
    }
}