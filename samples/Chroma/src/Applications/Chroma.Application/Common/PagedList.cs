namespace Chroma.Application.Common;

public class PagedList<TResult>
{
    public List<TResult> Results { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int ItemsPerPage { get; set; }
}