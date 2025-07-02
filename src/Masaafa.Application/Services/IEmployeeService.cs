using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Application.Services;

public interface IEmployeeService
{
    Task<PaginationResult<Employee>> GetAsync(
        PaginationParams @params,
        Filter filter,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Employee> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

    Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken = default);

    Task<Employee> UpdateAsync(Guid id, Employee employee, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}