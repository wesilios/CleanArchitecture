namespace CleanArchitecture.Presentation.Api.Common;

public interface IPaginationResponse<out TResponse>
{
    IReadOnlyList<TResponse> Results { get; }
    int TotalCount { get; set; }
    int PageNumber { get; set; }
    int ItemsPerPage { get; set; }
}