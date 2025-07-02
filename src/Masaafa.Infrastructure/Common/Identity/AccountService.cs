using Masaafa.Application.Common.Identity;
using Masaafa.Application.Services;
using Masaafa.Domain.Entities;

namespace Masaafa.Infrastructure.Common.Identity;

public class AccountService(IClientService clientService, IEmployeeService employeeService, ITokenGeneratorService tokenGeneratorService) : IAccountService
{
    public async Task<(Client Client, string Token)> SignInAsync(Client client, CancellationToken cancellationToken = default)
    {
        var exist = await clientService.GetByPhoneNumberAsync(client.PhoneNumber, cancellationToken);

        var token = await tokenGeneratorService.GenerateTokenAsync(exist, cancellationToken);

        return new(exist, token);
    }

    public async Task<(Employee Employee, string Token)> SignInAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        var exist = await employeeService.GetByPhoneNumberAsync(employee.PhoneNumber, cancellationToken);

        var token = await tokenGeneratorService.GenerateTokenAsync(exist, cancellationToken);

        return new(exist, token);
    }

    public async Task<bool> SignUpAsync(Client client, CancellationToken cancellationToken = default)
    {
        var entity = await clientService.CreateAsync(client, cancellationToken);

        return entity is not null ? true : false;
    }
}