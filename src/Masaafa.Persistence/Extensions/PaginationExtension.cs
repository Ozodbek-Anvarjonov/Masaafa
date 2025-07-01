using Masaafa.Domain.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Masaafa.Persistence.Extensions;

public static class PaginationExtension
{
    public static PaginationResult<T> ToPaginate<T>(this IQueryable<T> source, PaginationParams? @params = default)
    {
        if (@params == null || @params.PageNumber <= 0 || @params.PageSize <= 0)
            @params = new PaginationParams();

        int totalCount = source.Count();

        source = source
            .Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.PageSize);

        return new PaginationResult<T>(source.ToList(), new PaginationMetaData(totalCount, @params));
    }

    public static async Task<PaginationResult<T>> ToPaginateAsync<T>(this IQueryable<T> source, PaginationParams? @params = default)
    {
        if (@params == null || @params.PageNumber <= 0 || @params.PageSize <= 0)
            @params = new PaginationParams();

        int totalCount = await source.CountAsync();

        source = source
            .Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.PageSize);

        var pagedItems = await source.ToListAsync();
        var pagedResult = new PaginationResult<T>(pagedItems, new PaginationMetaData(totalCount, @params));

        return pagedResult;
    }
}
