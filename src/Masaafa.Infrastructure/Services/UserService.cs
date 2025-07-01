using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<PaginationResult<User>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.Users.GetAsync(@params, filter, search, cancellationToken: cancellationToken);

        return result;
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Users.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(User), nameof(User.Id), id.ToString());

        return entity;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Users.CreateAsync(user, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<User> UpdateAsync(Guid id, User user, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Users.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(User), nameof(User.Id), id.ToString());

        entity.FirstName = user.FirstName;
        entity.LastName = user.LastName;
        entity.JobTitle = user.JobTitle;
        entity.CardCode = user.CardCode;
        entity.PhoneNumber = user.PhoneNumber;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.Users.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}