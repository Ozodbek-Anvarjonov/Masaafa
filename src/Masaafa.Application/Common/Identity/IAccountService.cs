using Masaafa.Domain.Entities;

namespace Masaafa.Application.Common.Identity;

public interface IAccountService
{
    Task<bool> SignUpAsync(User user, CancellationToken cancellationToken = default);

    Task<(User User, string Token)> SignInAsync(User user, CancellationToken cancellationToken = default);
}
