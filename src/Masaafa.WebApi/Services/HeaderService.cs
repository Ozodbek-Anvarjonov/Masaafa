using Masaafa.Application.Common.Abstractions;
using Masaafa.Domain.Common.Pagination;
using Newtonsoft.Json;

namespace Masaafa.WebApi.Services;

public class HeaderService(IHttpContextAccessor contextAccessor) : IHeaderService
{
    public void WritePagination(PaginationMetaData? paginationMetaData)
    {
        if (paginationMetaData is null) return;

        var json = JsonConvert.SerializeObject(paginationMetaData);
        var headers = contextAccessor.HttpContext?.Response?.Headers;

        headers?.Remove("X-Pagination");
        headers?.Add("X-Pagination", json);
    }
}