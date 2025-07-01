using Masaafa.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public abstract class EntityRepositoryBase<TEntity, TContext>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    protected readonly TContext Context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public EntityRepositoryBase(TContext context) =>
        Context = context;

    protected IQueryable<TEntity> Get() =>
        Set.AsQueryable();

    protected async Task<TEntity> CreateAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default)
    {
        _ = await Set.AddAsync(entity, cancellationToken);

        if (saveChanges)
            _ = await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    protected async Task<TEntity> UpdateAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default)
    {
        _ = Set.Update(entity);

        if (saveChanges)
            _ = await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    protected async Task<TEntity> DeleteAsync(TEntity entity, bool saveChanges = false, CancellationToken cancellationToken = default)
    {
        _ = Set.Remove(entity);

        if (saveChanges)
            _ = await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}