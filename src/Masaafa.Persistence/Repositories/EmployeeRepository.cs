using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Persistence.DataContext;
using Masaafa.Persistence.Extensions;
using Masaafa.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Repositories;

public class EmployeeRepository(AppDbContext context) : EntityRepositoryBase<Employee, AppDbContext>(context), IEmployeeRepository
{
    public async Task<PaginationResult<Employee>> GetAsync(
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

    public async Task<Employee?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.Id == id && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Employee?> GetByPhoneNumberAsync(string phoneNumber, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exist = Set.Where(entity => entity.PhoneNumber == phoneNumber && !entity.IsDeleted);

        if (asNoTracking)
            exist = exist.AsNoTracking();

        return await exist.FirstOrDefaultAsync(cancellationToken);
    }

    public new IQueryable<Employee> Get() =>
        base.Get();

    public new Task<Employee> CreateAsync(Employee employee, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.CreateAsync(employee, saveChanges: saveChanges, cancellationToken: cancellationToken);

    public new Task<Employee> UpdateAsync(Employee employee, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(employee, saveChanges: saveChanges, cancellationToken: cancellationToken);

    public new Task<Employee> DeleteAsync(Employee employee, bool saveChanges = false, CancellationToken cancellationToken = default) =>
        base.DeleteAsync(employee, saveChanges, cancellationToken);
}