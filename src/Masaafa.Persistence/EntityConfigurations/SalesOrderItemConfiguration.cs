using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class SalesOrderItemConfiguration : IEntityTypeConfiguration<SalesOrderItem>
{
    public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
    {
        builder
            .HasOne(item => item.SalesOrder)
            .WithMany()
            .HasForeignKey(item => item.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.WarehouseItem)
            .WithMany()
            .HasForeignKey(item => item.WarehouseItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.WarehouseItem)
            .WithMany()
            .HasForeignKey(item => item.WarehouseItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}