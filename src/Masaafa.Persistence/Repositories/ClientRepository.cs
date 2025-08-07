using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class ClientRepository(AppDbContext context) : EntityRepositoryBase<Client, AppDbContext>(context), IClientRepository
{
    public async Task<PaginationResult<Client>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        var exists = Set.Where(entity => !entity.IsDeleted);

        if (search is not null)
            exists = exists
                .Where(entity => entity.FirstName.ToLower().Contains(search.ToLower())
                    || entity.LastName.ToLower().Contains(search.ToLower())
                    || entity.PhoneNumber.ToLower().Contains(search.ToLower())
                    || entity.CardCode.ToLower().Contains(search.ToLower())
                    || entity.Balance.ToString().Contains(search));

        exists = exists.OrderBy(filter);

        if (asNoTracking)
            exists = exists.AsNoTracking();

        return await exists.ToPaginateAsync(@params);
    }

    public async Task<Client?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Client?> GetByPhoneNumberAsync(string phoneNumber, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.PhoneNumber == phoneNumber && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<Client> Get() =>
        base.Get();

    public new Task<Client> CreateAsync(Client client, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(client, saveChanges: saveChanges, cancellationToken: cancellationToken);

    public new Task<Client> UpdateAsync(Client client, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(client, saveChanges: saveChanges, cancellationToken: cancellationToken);

    public new Task<Client> DeleteAsync(Client client, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(client, saveChanges, cancellationToken);
}