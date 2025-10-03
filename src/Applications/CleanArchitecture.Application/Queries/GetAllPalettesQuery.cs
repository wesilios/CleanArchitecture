namespace CleanArchitecture.Application.Queries;

public class GetAllPalettesQuery : PaginationParameters, IQuery<IPagedList<IPaletteDto>>
{
    public string? SearchTerm { get; set; }
}