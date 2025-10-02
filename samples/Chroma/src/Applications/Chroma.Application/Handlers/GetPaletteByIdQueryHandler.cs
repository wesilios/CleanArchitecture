namespace Chroma.Application.Handlers;

public class GetPaletteByIdQueryHandler : IQueryHandler<GetPaletteByIdQuery, IPaletteDto>
{
    private readonly IPaletteQueryService _queryService;

    public GetPaletteByIdQueryHandler(IPaletteQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<IPaletteDto> HandleAsync(GetPaletteByIdQuery query)
    {
        // The read path often uses a lighter projection or direct database access
        // that bypasses the Domain model. Here, we use the Repository for simplicity.
        var palette = await _queryService.GetByIdAsync(query.PaletteId);

        if (palette == null) return NullPaletteDto.Instance;

        // Map the Domain Entity to the Read DTO
        return new PaletteDto
        {
            PaletteId = palette.PaletteId,
            Name = palette.Name,
            Colors = palette.Colors.Select(c => new ColorDto
            {
                R = c.R,
                G = c.G,
                B = c.B,
                A = c.A,
                Hex = c.ToHexString()
            }).ToList()
        };
    }
}