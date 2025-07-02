using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;

namespace Masaafa.Persistence.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<PaginationResult<Employee>> GetAsync(
       PaginationParams @params,
       Filter filter,
       string? search = null,
       bool asNoTracking = true,
       CancellationToken cancellationToken = default);

    Task<Employee?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Employee?> GetByPhoneNumberAsync(string phoneNumber, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<Employee> CreateAsync(Employee employee, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Employee> UpdateAsync(Employee employee, bool saveChanges = false, CancellationToken cancellationToken = default);

    Task<Employee> DeleteAsync(Employee employee, bool saveChanges = false, CancellationToken cancellationToken = default);
}
