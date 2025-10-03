using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Handlers.Palette;

public class DeletePaletteCommandHandler : ICommandHandler<DeletePaletteCommand>
{
    private readonly IPaletteRepository _repository;
    private readonly IPaletteQueryService _queryService;

    public DeletePaletteCommandHandler(IPaletteRepository repository, IPaletteQueryService queryService)
    {
        _repository = repository;
        _queryService = queryService;
    }

    public async Task HandleAsync(DeletePaletteCommand command)
    {
        var palette = await _queryService.GetByIdAsync(command.PaletteId);

        if (palette == null)
        {
            throw new KeyNotFoundException($"Palette with Id {command.PaletteId} not found.");
        }

        await _repository.DeleteAsync(palette);
    }
}