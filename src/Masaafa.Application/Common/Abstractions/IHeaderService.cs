using Masaafa.Domain.Common.Pagination;

namespace Masaafa.Application.Common.Abstractions;

public interface IHeaderService
{
    void WritePagination(PaginationMetaData? paginationMetaData);
}
