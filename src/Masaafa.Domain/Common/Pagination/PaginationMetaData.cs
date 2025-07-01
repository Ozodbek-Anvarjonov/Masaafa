namespace Masaafa.Domain.Common.Pagination;

public class PaginationMetaData
{
    public PaginationMetaData(int totalCount, PaginationParams @params)
    {
        TotalPages = (int)Math.Ceiling(totalCount / (decimal)@params.PageSize);
        CurrentPage = @params.PageNumber;
    }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;
}