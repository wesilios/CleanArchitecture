using Chroma.Domain.Repositories;
using Chroma.Domain.ValueObjects;

namespace Chroma.Application.Commands;

public class AddColorToPaletteHandler
{
    private readonly IPaletteRepository _repository;

    public AddColorToPaletteHandler(IPaletteRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddColorToPaletteCommand command)
    {
        // 1. Get the Aggregate Root (Palette)
        var palette = await _repository.GetByIdAsync(command.PaletteId);

        if (palette == null)
        {
            throw new KeyNotFoundException($"Palette with Id {command.PaletteId} not found.");
        }

        // 2. Create the new Color entity
        var newColor = new Color(command.Red, command.Green, command.Blue, command.Opacity);

        // 3. Execute the business logic on the Aggregate Root
        // This is where the max 5 and uniqueness rules are checked.
        palette.AddColor(newColor);

        // 4. Persist the changes
        await _repository.UpdateAsync(palette);
        await _repository.SaveChangesAsync();
    }
}