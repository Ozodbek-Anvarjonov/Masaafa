using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class WarehouseItemConfiguration : IEntityTypeConfiguration<WarehouseItem>
{
    public void Configure(EntityTypeBuilder<WarehouseItem> builder)
    {
        builder
            .HasOne(item => item.Warehouse)
            .WithMany(group => group.Items)
            .HasForeignKey(item => item.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.Item)
            .WithMany()
            .HasForeignKey(item => item.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}