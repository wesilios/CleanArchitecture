using Chroma.Domain.Repositories;

namespace Chroma.Application.Queries;

public class GetPaletteByIdHandler
{
    private readonly IPaletteRepository _repository;

    public GetPaletteByIdHandler(IPaletteRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaletteDto> Handle(GetPaletteByIdQuery query)
    {
        // The read path often uses a lighter projection or direct database access
        // that bypasses the Domain model. Here, we use the Repository for simplicity.
        var palette = await _repository.GetByIdAsync(query.PaletteId);

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