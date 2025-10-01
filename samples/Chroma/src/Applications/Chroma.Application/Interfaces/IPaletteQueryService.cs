using Chroma.Domain.Entities;

namespace Chroma.Application.Interfaces;

public interface IPaletteQueryService
{
    Task<Palette> GetByIdAsync(long paletteId);
    Task<PagedList<Palette>> GetGetAllPalettesAsync(GetAllPalettesQuery query);
}