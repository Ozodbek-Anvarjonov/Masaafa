using Masaafa.Domain.Common.Entities;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Masaafa.Persistence.Interceptors;

public class AuditableInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public AuditableInterceptor(IUserContext userContext) =>
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var autidableEntry = eventData.Context!.ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .ToList();

        var userId = _userContext.GetRequiredUserId();
        var now = DateTimeOffset.UtcNow;


        foreach (var entry in autidableEntry)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
            }
            else
            {
                entry.Entity.ModifiedAt = now;
                entry.Entity.ModifiedBy = userId;
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}