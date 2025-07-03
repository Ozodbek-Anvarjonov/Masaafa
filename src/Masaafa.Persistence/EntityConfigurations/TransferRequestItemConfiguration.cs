using Masaafa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masaafa.Persistence.EntityConfigurations;

public class TransferRequestItemConfiguration : IEntityTypeConfiguration<TransferRequestItem>
{
    public void Configure(EntityTypeBuilder<TransferRequestItem> builder)
    {
        builder
            .HasOne(item => item.TransferRequest)
            .WithMany()
            .HasForeignKey(item => item.TransferRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
           .HasOne(item => item.Item)
           .WithMany()
           .HasForeignKey(item => item.ItemId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
