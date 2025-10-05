using External.Client.ApiConsumer.Models;

namespace External.Client.ApiConsumer.Services;

public interface IPaletteService
{
    Task<PalettePaginationResponse?> GetPalettesAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<PaletteResponse?> GetPaletteByIdAsync(long paletteId);
    Task<bool> CreatePaletteAsync(string name);
    Task<bool> UpdatePaletteAsync(long paletteId, string name);
    Task<bool> DeletePaletteAsync(long paletteId);
    Task<bool> AddColorToPaletteAsync(long paletteId, int r, int g, int b, decimal a);
}