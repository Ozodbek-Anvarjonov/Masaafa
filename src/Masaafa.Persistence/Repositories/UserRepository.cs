using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class UserRepository(AppDbContext context) : EntityRepositoryBase<User, AppDbContext>(context), IUserRepository
{
    public async Task<PaginationResult<User>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var exists = Set.Where(entity => !entity.IsDeleted);

        if (search is not null)
            exists = exists
                .Where(entity => true);

        exists = exists.OrderBy(filter);

        return await exists.ToPaginateAsync(@params);
    }

    public async Task<User?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<User> Get() =>
        base.Get();

    public new Task<User> CreateAsync(User user, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(user, saveChanges: saveChanges, cancellationToken: cancellationToken);

    public new Task<User> UpdateAsync(User user, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(user, saveChanges: saveChanges, cancellationToken: cancellationToken);

    public new Task<User> DeleteAsync(User user, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(user, saveChanges, cancellationToken);
}