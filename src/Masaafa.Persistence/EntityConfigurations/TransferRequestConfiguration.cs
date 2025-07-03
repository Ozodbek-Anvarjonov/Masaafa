using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class TransferRequestConfiguration : IEntityTypeConfiguration<TransferRequest>
{
    public void Configure(EntityTypeBuilder<TransferRequest> builder)
    {
        builder
            .HasOne(transfer => transfer.FromWarehouse)
            .WithMany()
            .HasForeignKey(transfer => transfer.FromWarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(transfer => transfer.ToWarehouse)
            .WithMany()
            .HasForeignKey(transfer => transfer.ToWarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(transfer => transfer.CreatedByUser)
            .WithMany()
            .HasForeignKey(transfer => transfer.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(transfer => transfer.ApprovedByUser)
            .WithMany()
            .HasForeignKey(transfer => transfer.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(transfer => transfer.RejectedByUser)
            .WithMany()
            .HasForeignKey(transfer => transfer.RejectedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
