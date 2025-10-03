using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Handlers.Palette;

public class UpdatePaletteCommandHandler : ICommandHandler<UpdatePaletteCommand>
{
    private readonly IPaletteRepository _repository;
    private readonly IPaletteQueryService _queryService;

    public UpdatePaletteCommandHandler(IPaletteRepository repository, IPaletteQueryService queryService)
    {
        _repository = repository;
        _queryService = queryService;
    }

    public async Task HandleAsync(UpdatePaletteCommand command)
    {
        var palette = await _queryService.GetByIdAsync(command.PaletteId);

        if (palette == null)
        {
            throw new KeyNotFoundException($"Palette with Id {command.PaletteId} not found.");
        }

        palette.Name = command.Name;

        await _repository.UpdateAsync(palette);
    }
}