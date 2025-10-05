using System.Collections;

namespace CleanArchitecture.Application.Common;

public class PagedList<TResult> : IPagedList<TResult>
{
    public List<TResult> Results { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int ItemsPerPage => Results.Count;

    IReadOnlyList<TResult> IPagedList<TResult>.Results => Results.AsReadOnly();

    public IEnumerator<TResult> GetEnumerator()
    {
        return Results.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public interface IPagedList<out TResult> : IEnumerable<TResult>
{
    IReadOnlyList<TResult> Results { get; }
    int TotalCount { get; set; }
    int PageNumber { get; set; }
    int PageSize { get; set; }
    int ItemsPerPage { get; }
}