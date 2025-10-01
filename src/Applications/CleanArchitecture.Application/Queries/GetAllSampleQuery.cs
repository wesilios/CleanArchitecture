using CleanArchitecture.Application.Common;

namespace CleanArchitecture.Application.Queries;

public class GetAllSampleQuery : PaginationParameters
{
    public string? SearchTerm { get; set; }
}