namespace Chroma.Application.Handlers;

public class GetAllPalettesQueryHandler : IQueryHandler<GetAllPalettesQuery, PagedList<PaletteDto>>
{
    private readonly IPaletteQueryService _queryService;

    public GetAllPalettesQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public Task<PagedList<PaletteDto>> HandleAsync(GetAllPalettesQuery query)
    {
        throw new NotImplementedException();
    }
}