namespace CleanArchitecture.Domain.Repositories;

public interface IPaletteRepository
{
    Task AddAsync(Palette palette);
    Task AddColorAsync(Palette palette);
    Task UpdateAsync(Palette palette);
    Task DeleteAsync(Palette palette);
}