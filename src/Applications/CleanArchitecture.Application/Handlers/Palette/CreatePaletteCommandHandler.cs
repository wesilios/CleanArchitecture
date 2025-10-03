using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Handlers.Palette;

public class CreatePaletteCommandHandler : ICommandHandler<CreatePaletteCommand>
{
    private readonly IPaletteRepository _repository;

    public CreatePaletteCommandHandler(IPaletteRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(CreatePaletteCommand command)
    {
        await _repository.AddAsync(new Domain.Entities.Palette(command.Name));
    }
}