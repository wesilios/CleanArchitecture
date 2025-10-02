namespace Chroma.Application.Handlers;

public class GetAllPalettesQueryHandler : IQueryHandler<GetAllPalettesQuery, IPagedList<IPaletteDto>>
{
    private readonly IPaletteQueryService _queryService;

    public GetAllPalettesQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<IPagedList<IPaletteDto>> HandleAsync(GetAllPalettesQuery query)
    {
        var palettesPagedList = await _queryService.GetGetAllPalettesAsync(query);

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
            PageNumber = query.PageNumber,
            ItemsPerPage = query.PageSize
        };
    }
}