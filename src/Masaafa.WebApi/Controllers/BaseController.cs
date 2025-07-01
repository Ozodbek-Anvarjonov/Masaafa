using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Masaafa.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected CancellationToken CancellationToken => HttpContext.RequestAborted;
}