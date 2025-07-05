using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder
            .HasOne(item => item.Inventory)
            .WithMany()
            .HasForeignKey(item => item.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.WarehouseItem)
            .WithMany()
            .HasForeignKey(item => item.WarehouseItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.CountedByUser)
            .WithMany()
            .HasForeignKey(item => item.CountedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}