using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder
            .HasIndex(entity => entity.SalesOrderNumber)
            .IsUnique();

        builder
            .HasOne(order => order.Client)
            .WithMany()
            .HasForeignKey(order => order.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(order => order.CreatedByUser)
            .WithMany()
            .HasForeignKey(order => order.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(order => order.ApprovedByUser)
            .WithMany()
            .HasForeignKey(order => order.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(order => order.RejectedByUser)
            .WithMany()
            .HasForeignKey (order => order.RejectedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(order => order.CancelledByUser)
            .WithMany()
            .HasForeignKey(order => order.CancelledByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}