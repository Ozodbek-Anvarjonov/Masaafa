namespace Masaafa.Domain.Common.Pagination;

public class PaginationResult<TEntity>
{
    public PaginationResult() { }

    public PaginationResult(IEnumerable<TEntity> data, PaginationMetaData? paginationMetaData)
    {
        Data = data;
        PaginationMetaData = paginationMetaData;
    }

    public IEnumerable<TEntity> Data { get; set; }

    public PaginationMetaData? PaginationMetaData { get; set; }
}