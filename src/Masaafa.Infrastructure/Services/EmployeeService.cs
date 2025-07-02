using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Domain.Exceptions;
using Masaafa.Persistence.UnitOfWork.Interfaces;

namespace Masaafa.Infrastructure.Services;

public class EmployeeService(IUnitOfWork unitOfWork) : IEmployeeService
{
    public async Task<PaginationResult<Employee>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.Employees.GetAsync(@params, filter, search, cancellationToken: cancellationToken);
        return result;
    }

    public async Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Employees.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Employee), nameof(Employee.Id), id.ToString());

        return entity;
    }

    public async Task<Employee> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Employees.GetByPhoneNumberAsync(phoneNumber, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Employee), nameof(Employee.PhoneNumber), phoneNumber);

        return entity;
    }

    public async Task<Employee> CreateAsync(Employee client, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Employees.CreateAsync(client, saveChanges: true, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Employee> UpdateAsync(Guid id, Employee employee, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Employees.GetByIdAsync(id, asNoTracking: false, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Employee), nameof(Employee.Id), id.ToString());

        entity.FirstName = employee.FirstName;
        entity.LastName = employee.LastName;
        entity.PhoneNumber = employee.PhoneNumber;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        _ = await unitOfWork.Employees.DeleteAsync(entity, true, cancellationToken: cancellationToken);

        return true;
    }
}