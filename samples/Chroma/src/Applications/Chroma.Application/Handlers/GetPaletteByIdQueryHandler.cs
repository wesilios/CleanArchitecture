using Chroma.Application.DataObjects;
using Chroma.Application.Handlers.Abstractions;
using Chroma.Application.Interfaces;
using Chroma.Application.Queries;

namespace Chroma.Application.Handlers;

public class GetPaletteByIdQueryHandler : IQueryHandler<GetPaletteByIdQuery, PaletteDto>
{
    private readonly IPaletteQueryService _queryService;

    public GetPaletteByIdQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaletteDto> HandleAsync(GetPaletteByIdQuery query)
    {
        // The read path often uses a lighter projection or direct database access
        // that bypasses the Domain model. Here, we use the Repository for simplicity.
        var palette = await _queryService.GetByIdAsync(query.PaletteId);

        if (palette == null) return null;

        // Map the Domain Entity to the Read DTO
        return new PaletteDto
        {
            PaletteId = palette.PaletteId,
            Name = palette.Name,
            Colors = palette.Colors.Select(c => new ColorDto
            {
                R = c.RedPigment,
                G = c.GreenPigment,
                B = c.BluePigment,
                A = c.Opacity
            }).ToList()
        };
    }
}