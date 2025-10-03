using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces;

public interface IPaletteQueryService
{
    Task<Palette?> GetByIdAsync(long paletteId);
    Task<PagedList<Palette>> GetGetAllPalettesAsync(GetAllPalettesQuery query);
}