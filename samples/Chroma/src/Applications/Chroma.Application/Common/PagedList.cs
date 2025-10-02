using System.Collections;

namespace Chroma.Application.Common;

public class PagedList<TResult> : IPagedList<TResult>
{
    public List<TResult> Results { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int ItemsPerPage { get; set; }
    public IEnumerator<TResult> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public interface IPagedList<out TResult> : IEnumerable<TResult>
{
    int TotalCount { get; set; }
    int PageNumber { get; set; }
    int ItemsPerPage { get; set; }
}