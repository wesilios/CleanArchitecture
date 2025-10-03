using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Handlers;

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