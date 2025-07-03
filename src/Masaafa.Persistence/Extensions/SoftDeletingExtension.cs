using Masaafa.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Extensions;

public static class SoftDeletingExtension
{
    public static async Task SoftDeleteAsync<TEntity>(
        this IQueryable<TEntity> source,
        Guid userId,
        CancellationToken cancellationToken = default
        ) where TEntity : class, ISoftDeletedEntity
    {
        await source.ExecuteUpdateAsync(
            call => call
                .SetProperty(entity => entity.IsDeleted, true)
                .SetProperty(entity => entity.DeletedAt, DateTimeOffset.UtcNow)
                .SetProperty(entity => entity.DeletedBy, userId)
            );
    }
}