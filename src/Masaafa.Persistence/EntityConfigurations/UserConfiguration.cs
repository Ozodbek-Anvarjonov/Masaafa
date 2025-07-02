using Masaafa.Domain.Entities;
using Masaafa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasDiscriminator<UserType>(nameof(User.Type))
            .HasValue<Client>(UserType.Client)
            .HasValue<Employee>(UserType.Employee);
    }
}