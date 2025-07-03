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
            .WithMany(salesOrder => salesOrder.Items)
            .HasForeignKey(item => item.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.Item)
            .WithMany()
            .HasForeignKey(item => item.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(item => item.Warehouse)
            .WithMany()
            .HasForeignKey(item => item.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}