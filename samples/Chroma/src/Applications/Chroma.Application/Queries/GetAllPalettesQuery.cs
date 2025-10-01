using Chroma.Application.Common;
using Chroma.Application.DataObjects;

namespace Chroma.Application.Queries;

public class GetAllPalettesQuery : PaginationParameters, IQuery<List<PaletteDto>>
{
    public string? SearchTerm { get; set; }
}