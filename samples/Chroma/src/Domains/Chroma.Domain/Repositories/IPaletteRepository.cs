namespace Chroma.Domain.Repositories;

public interface IPaletteRepository
{
    Task<Palette> GetByIdAsync(long id);
    Task AddAsync(Palette palette);
    Task UpdateAsync(Palette palette);
    Task SaveChangesAsync();
}