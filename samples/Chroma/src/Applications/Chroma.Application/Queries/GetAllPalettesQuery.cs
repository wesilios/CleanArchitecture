namespace Chroma.Application.Queries;

public class GetAllPalettesQuery : PaginationParameters, IQuery<PagedList<PaletteDto>>
{
    public string? SearchTerm { get; set; }
}