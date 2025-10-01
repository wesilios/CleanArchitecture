namespace CleanArchitecture.Application.Common;

public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;
    public int ItemsPerPage { get; set; } = 10;
}