using Masaafa.Application.Settings;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Masaafa.WebApi.Services;

public class HttpUserContext : IUserContext
{
    public Guid SystemId { get; }
    public Guid? UserId { get; }

    public HttpUserContext(IOptions<SystemSettings> options, IHttpContextAccessor accessor)
    {
        SystemId = options.Value.SystemId;

        var userIdClaim = accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
            UserId = userId;
        else
            UserId = null;
    }

    public Guid GetRequiredUserId() =>
        UserId is null ? SystemId : UserId.Value;
}