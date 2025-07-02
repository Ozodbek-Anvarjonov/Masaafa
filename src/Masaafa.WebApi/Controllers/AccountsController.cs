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
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] string phoneNumber)
    {
        var entity = await accountService.SignInAsync(new User { PhoneNumber = phoneNumber }, CancellationToken);

        return Ok(new
        {
            User = mapper.Map<UserResponse>(entity.User),
            Token = entity.Token,
        });
    }
    
    [HttpPost]
    public async ValueTask<IActionResult> SignUp([FromBody] CreateUserRequest request)
    {
        var entity = await accountService.SignUpAsync(mapper.Map<User>(request), CancellationToken);

        return Ok(entity);
    }
}