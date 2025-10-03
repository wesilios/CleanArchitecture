namespace CleanArchitecture.Application.Handlers.Palette;

public class GetAllPalettesQueryHandler : IQueryHandler<GetAllPalettesSearchQuery, IPagedList<IPaletteDto>>
{
    private readonly IPaletteQueryService _queryService;

    public GetAllPalettesQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<IPagedList<IPaletteDto>> HandleAsync(GetAllPalettesSearchQuery searchQuery)
    {
        var palettesPagedList = await _queryService.GetGetAllPalettesAsync(searchQuery);

        return new PagedList<PaletteDto>
        {
            Results = palettesPagedList.Results.Select(p => new PaletteDto
            {
                PaletteId = p.PaletteId,
                Name = p.Name,
                Colors = p.Colors.Select(c => new ColorDto
                {
                    R = c.R,
                    G = c.G,
                    B = c.B,
                    A = c.A,
                    Hex = c.ToHexString()
                }).ToList()
            }).ToList(),
            TotalCount = palettesPagedList.TotalCount,
            PageNumber = searchQuery.PaginationParameters.PageNumber,
            PageSize = searchQuery.PaginationParameters.PageSize
        };
    }
}