using External.Client.ApiConsumer.Models;

namespace External.Client.ApiConsumer.Services;

public interface IApiClient
{
    Task<ApiResponse<PalettePaginationResponse>?> GetPalettesAsync(int pageNumber, int pageSize,
        string? searchTerm = null);

    Task<ApiResponse<PaletteResponse>?> GetPaletteByIdAsync(long paletteId);
    Task<ApiResponse<object>?> CreatePaletteAsync(CreatePaletteRequest request);
    Task<ApiResponse<object>?> UpdatePaletteAsync(long paletteId, UpdatePaletteRequest request);
    Task<ApiResponse<object>?> DeletePaletteAsync(long paletteId);
    Task<ApiResponse<object>?> AddColorToPaletteAsync(long paletteId, CreatePaletteColorRequest request);
}