using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class ClientService(IUnitOfWork unitOfWork) : IClientService
{
    public async Task<PaginationResult<Client>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.Clients.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<Client> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Clients.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Client), nameof(Client.Id), id.ToString());

        return entity;
    }

    public async Task<Client> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Clients.GetByPhoneNumberAsync(phoneNumber, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Client), nameof(Client.PhoneNumber), phoneNumber);

        return entity;
    }

    public async Task<Client> CreateAsync(Client client, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Clients.CreateAsync(client, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Client> UpdateAsync(Guid id, Client client, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Clients.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Client), nameof(Client.Id), id.ToString());

        entity.FirstName = client.FirstName;
        entity.LastName = client.LastName;
        entity.CardCode = client.CardCode;
        entity.PhoneNumber = client.PhoneNumber;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.Clients.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}