using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder
            .HasOne(item => item.ItemGroup)
            .WithMany(group => group.Items)
            .HasForeignKey(item => item.ItemGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
