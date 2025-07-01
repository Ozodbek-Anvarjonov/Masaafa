using AutoMapper;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

public class UsersController(IUserService userService, IHeaderService headerService, IMapper mapper) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        string? search = null)
    {
        var result = await userService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<UserResponse>>(result.Data));
    }


    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await userService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<UserResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateUserRequest request)
    {
        var entity = await userService.CreateAsync(mapper.Map<User>(request), CancellationToken);

        return Ok(entity);
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        var entity = await userService.UpdateAsync(id, mapper.Map<User>(request), CancellationToken);

        return Ok(mapper.Map<UserResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await userService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}