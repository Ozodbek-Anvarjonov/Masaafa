using Masaafa.Application.Common.Identity;
using Masaafa.Application.Services;
using Masaafa.Domain.Entities;

namespace Masaafa.Infrastructure.Common.Identity;

public class AccountService(IUserService userService, ITokenGeneratorService tokenGeneratorService) : IAccountService
{
    public async Task<(User User, string Token)> SignInAsync(User user, CancellationToken cancellationToken = default)
    {
        var exist = await userService.GetByPhoneNumberAsync(user.PhoneNumber, cancellationToken);

        var token = await tokenGeneratorService.GenerateTokenAsync(exist, cancellationToken);

        return new(exist, token);
    }

    public async Task<bool> SignUpAsync(User user, CancellationToken cancellationToken = default)
    {
        var entity = await userService.CreateAsync(user, cancellationToken);

        return entity is not null ? true : false;
    }
}