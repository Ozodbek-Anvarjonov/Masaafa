using AutoMapper;
using Masaafa.Application.Common.Identity;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.Infrastructure.Services;
using Masaafa.WebApi.Models.Users;
using Masaafa.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class AccountsController(IAccountService accountService, IMapper mapper) : BaseController
{
    [HttpGet("client")]
    public async ValueTask<IActionResult> GetClient([FromQuery] string phoneNumber)
    {
        var entity = await accountService.SignInAsync(new Client { PhoneNumber = phoneNumber }, CancellationToken);

        return Ok(new
        {
            User = mapper.Map<ClientResponse>(entity.Client),
            Token = entity.Token,
        });
    }
    
    [HttpPost("client")]
    public async ValueTask<IActionResult> SignUp([FromBody] CreateClientRequest request)
    {
        var entity = await accountService.SignUpAsync(mapper.Map<Client>(request), CancellationToken);

        return Ok(entity);
    }

    [HttpGet("employee")]
    public async ValueTask<IActionResult> GetEmployee([FromQuery] string phoneNumber)
    {
        var entity = await accountService.SignInAsync(new Employee { PhoneNumber = phoneNumber }, CancellationToken);

        return Ok(new
        {
            User = mapper.Map<EmployeeResponse>(entity.Employee),
            Token = entity.Token,
        });
    }
}