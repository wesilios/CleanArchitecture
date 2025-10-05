using External.Client.ApiConsumer.Models;

namespace External.Client.ApiConsumer.Services.HttpClients;

public interface IApiClientService
{
    Task<BaseApiResponse<PalettePaginationResponse>?> GetPalettesAsync(int pageNumber, int pageSize,
        string? searchTerm = null);

    Task<BaseApiResponse<PaletteResponse>?> GetPaletteByIdAsync(long paletteId);
    Task<BaseApiResponse<object>?> CreatePaletteAsync(CreatePaletteRequest request);
    Task<BaseApiResponse<object>?> UpdatePaletteAsync(long paletteId, UpdatePaletteRequest request);
    Task<BaseApiResponse<object>?> DeletePaletteAsync(long paletteId);
    Task<BaseApiResponse<object>?> AddColorToPaletteAsync(long paletteId, CreatePaletteColorRequest request);
}