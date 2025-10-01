namespace Chroma.Application.Handlers;

public class GetAllPalettesQueryHandler : IQueryHandler<GetAllPalettesQuery, PagedList<PaletteDto>>
{
    private readonly IPaletteQueryService _queryService;

    public GetAllPalettesQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PagedList<PaletteDto>> HandleAsync(GetAllPalettesQuery query)
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
                    R = c.RedPigment,
                    G = c.GreenPigment,
                    B = c.BluePigment,
                    A = c.Opacity
                }).ToList()
            }).ToList(),
            TotalCount = palettesPagedList.TotalCount,
            PageNumber = query.PageNumber,
            ItemsPerPage = query.PageSize
        };
    }
}