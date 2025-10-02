using Chroma.Domain.Entities;
using Chroma.Domain.Repositories;

namespace Chroma.Application.Handlers;

public class AddPaletteCommandHandler : ICommandHandler<AddPaletteCommand>
{
    private readonly IPaletteRepository _repository;

    public AddPaletteCommandHandler(IPaletteRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(AddPaletteCommand command)
    {
        await _repository.AddAsync(new Palette(command.Name));
    }
}