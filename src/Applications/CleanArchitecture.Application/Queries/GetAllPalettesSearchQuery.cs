namespace CleanArchitecture.Application.Queries;

public class GetAllPalettesSearchQuery : SearchQuery<IPagedList<IPaletteDto>>
{
    public PaginationParameters PaginationParameters { get; init; } = new();
}