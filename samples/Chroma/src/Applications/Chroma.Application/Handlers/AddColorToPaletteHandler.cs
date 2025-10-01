using Chroma.Domain.Repositories;
using Chroma.Domain.ValueObjects;

namespace Chroma.Application.Handlers;

public class AddColorToPaletteHandler : ICommandHandler<AddColorToPaletteCommand>
{
    private readonly IPaletteRepository _repository;
    private readonly IPaletteQueryService _queryService;

    public AddColorToPaletteHandler(IPaletteRepository repository, IPaletteQueryService queryService)
    {
        _repository = repository;
        _queryService = queryService;
    }

    public async Task HandleAsync(AddColorToPaletteCommand command)
    {
        var palette = await _queryService.GetByIdAsync(command.PaletteId);

        if (palette == null)
        {
            throw new KeyNotFoundException($"Palette with Id {command.PaletteId} not found.");
        }

        var newColor = new Color(command.Red, command.Green, command.Blue, command.Opacity);

        palette.AddColor(newColor);

        await _repository.UpdateAsync(palette);
        await _repository.SaveChangesAsync();
    }
}