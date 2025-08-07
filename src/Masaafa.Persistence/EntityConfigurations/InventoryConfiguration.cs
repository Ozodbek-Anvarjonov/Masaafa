using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder
            .HasIndex(entity => entity.InventoryNumber)
            .IsUnique();

        builder
            .HasOne(inventory => inventory.Warehouse)
            .WithMany()
            .HasForeignKey(inventory => inventory.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(inventory => inventory.CreatedByUser)
            .WithMany()
            .HasForeignKey(inventory => inventory.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(inventory => inventory.StartedByUser)
            .WithMany()
            .HasForeignKey(inventory => inventory.StartedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(inventory => inventory.CompletedByUser)
            .WithMany()
            .HasForeignKey(inventory => inventory.CompletedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
