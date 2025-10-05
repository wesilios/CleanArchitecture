using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Handlers.Color;

public class CreateColorToPaletteHandler : ICommandHandler<CreateColorToPaletteCommand>
{
    private readonly IPaletteRepository _repository;
    private readonly IPaletteQueryService _queryService;

    public CreateColorToPaletteHandler(IPaletteRepository repository, IPaletteQueryService queryService)
    {
        _repository = repository;
        _queryService = queryService;
    }

    public async Task HandleAsync(CreateColorToPaletteCommand command)
    {
        var palette = await _queryService.GetByIdAsync(command.PaletteId);

        if (palette == null)
        {
            throw new KeyNotFoundException($"Palette with Id {command.PaletteId} not found.");
        }

        var newColor = new Domain.ValueObjects.Color(command.R, command.G, command.B, command.A);

        palette.AddColor(newColor);

        await _repository.AddColorAsync(palette);
    }
}