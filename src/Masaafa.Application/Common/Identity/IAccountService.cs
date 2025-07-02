using Masaafa.Domain.Entities;

namespace Masaafa.Application.Common.Identity;

public interface IAccountService
{
    Task<bool> SignUpAsync(Client client, CancellationToken cancellationToken = default);

    Task<(Client Client, string Token)> SignInAsync(Client client, CancellationToken cancellationToken = default);

    Task<(Employee Employee, string Token)> SignInAsync(Employee employee, CancellationToken cancellationToken = default);
}
