using Chroma.Application.DataObjects;
using Chroma.Application.Handlers.Abstractions;
using Chroma.Application.Interfaces;
using Chroma.Application.Queries;

namespace Chroma.Application.Handlers;

public class GetAllPalettesQueryHandler : IQueryHandler<GetAllPalettesQuery, List<PaletteDto>>
{
    private readonly IPaletteQueryService _queryService;

    public GetAllPalettesQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public Task<List<PaletteDto>> HandleAsync(GetAllPalettesQuery query)
    {
        throw new NotImplementedException();
    }
}