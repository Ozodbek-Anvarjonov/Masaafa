using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .HasIndex(entity => entity.PaymentNumber)
            .IsUnique();

        builder
            .HasOne(payment => payment.Client)
            .WithMany()
            .HasForeignKey(payment => payment.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(payment => payment.SalesOrder)
            .WithMany()
            .HasForeignKey(payment => payment.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(payment => payment.CreatedByUser)
            .WithMany()
            .HasForeignKey(payment => payment.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(payment => payment.CompletedByUser)
            .WithMany()
            .HasForeignKey(payment => payment.CompletedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(entity => entity.SalesOrder)
            .WithOne(entity => entity.Payment)
            .HasForeignKey<Payment>(entity => entity.SalesOrderId);
    }
}