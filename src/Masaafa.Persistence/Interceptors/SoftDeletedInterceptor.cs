using Masaafa.Domain.Common.Entities;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Interceptors;

public class SoftDeletedInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public SoftDeletedInterceptor(IUserContext userContext) =>
        _userContext = userContext ?? throw new ArgumentNullException(nameof(IUserContext));

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var softDeletedEntry = eventData.Context!.ChangeTracker
            .Entries<ISoftDeletedEntity>()
            .Where(entry => entry.State is EntityState.Deleted)
            .ToList();

        var userId = _userContext.GetRequiredUserId();
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in softDeletedEntry)
        {
            entry.State = EntityState.Modified;
            entry.Entity.DeletedAt = now;
            entry.Entity.DeletedBy = userId;
            entry.Entity.IsDeleted = true;
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}