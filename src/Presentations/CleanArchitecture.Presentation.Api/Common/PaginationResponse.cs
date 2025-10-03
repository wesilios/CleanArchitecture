namespace CleanArchitecture.Presentation.Api.Common;

public interface IPaginationResponse<out TResponse> : IEnumerable<TResponse>
{
    IReadOnlyList<TResponse> Results { get; }
    int TotalCount { get; set; }
    int PageNumber { get; set; }
    int ItemsPerPage { get; set; }
}