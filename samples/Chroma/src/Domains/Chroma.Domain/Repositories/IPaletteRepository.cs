namespace Chroma.Domain.Repositories;

public interface IPaletteRepository
{
    Task AddAsync(Palette palette);
    Task UpdateAsync(Palette palette);
    Task SaveChangesAsync();
}