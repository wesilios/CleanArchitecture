namespace CleanArchitecture.Application.Common;

public class PagedList<T>
{
    public List<T> Results { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int ItemsPerPage { get; set; }
}