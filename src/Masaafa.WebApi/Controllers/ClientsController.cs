using AutoMapper;
using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Services;
using Masaafa.Domain.Common.Pagination;
using Masaafa.Domain.Entities;
using Masaafa.WebApi.Extensions;
using Masaafa.WebApi.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

//[CustomAuthorize(nameof(UserRole.Agent))]
public class ClientsController(
    IClientService clientService,
    IHeaderService headerService,
    IMapper mapper,
    IValidator<CreateClientRequest> createValidator,
    IValidator<UpdateClientRequest> updateValidator) : BaseController
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(
        [FromQuery] PaginationParams @params,
        [FromQuery] Filter filter,
        [FromQuery] string? search = null)
    {
        var result = await clientService.GetAsync(@params, filter, search, CancellationToken);

        headerService.WritePagination(result.PaginationMetaData);

        return Ok(mapper.Map<IEnumerable<ClientResponse>>(result.Data));
    }


    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id)
    {
        var entity = await clientService.GetByIdAsync(id, CancellationToken);

        return Ok(mapper.Map<ClientResponse>(entity));
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] CreateClientRequest request)
    {
        await createValidator.EnsureValidationAsync(request);

        var entity = await clientService.CreateAsync(mapper.Map<Client>(request), CancellationToken);

        return Ok(mapper.Map<ClientResponse>(entity));
    }

    [HttpPut("{id:guid}")]
    public async ValueTask<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateClientRequest request)
    {
        await updateValidator.EnsureValidationAsync(request);

        var entity = await clientService.UpdateAsync(id, mapper.Map<Client>(request), CancellationToken);

        return Ok(mapper.Map<ClientResponse>(entity));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await clientService.DeleteByIdAsync(id, CancellationToken);

        return Ok(result);
    }
}