using External.Client.ApiConsumer.Models;
using Microsoft.Extensions.Logging;

namespace External.Client.ApiConsumer.Services;

public class PaletteService : IPaletteService
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<PaletteService> _logger;

    public PaletteService(IApiClient apiClient, ILogger<PaletteService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<PalettePaginationResponse?> GetPalettesAsync(int pageNumber, int pageSize,
        string? searchTerm = null)
    {
        try
        {
            var response = await _apiClient.GetPalettesAsync(pageNumber, pageSize, searchTerm);
            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting palettes");
            return null;
        }
    }

    public async Task<PaletteResponse?> GetPaletteByIdAsync(long paletteId)
    {
        try
        {
            var response = await _apiClient.GetPaletteByIdAsync(paletteId);
            return response?.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting palette {PaletteId}", paletteId);
            return null;
        }
    }

    public async Task<bool> CreatePaletteAsync(string name)
    {
        try
        {
            var request = new CreatePaletteRequest { Name = name };
            var response = await _apiClient.CreatePaletteAsync(request);
            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating palette");
            return false;
        }
    }

    public async Task<bool> UpdatePaletteAsync(long paletteId, string name)
    {
        try
        {
            var request = new UpdatePaletteRequest { Name = name };
            var response = await _apiClient.UpdatePaletteAsync(paletteId, request);
            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating palette {PaletteId}", paletteId);
            return false;
        }
    }

    public async Task<bool> DeletePaletteAsync(long paletteId)
    {
        try
        {
            var response = await _apiClient.DeletePaletteAsync(paletteId);
            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting palette {PaletteId}", paletteId);
            return false;
        }
    }

    public async Task<bool> AddColorToPaletteAsync(long paletteId, int r, int g, int b, decimal a)
    {
        try
        {
            var request = new CreatePaletteColorRequest { R = r, G = g, B = b, A = a };
            var response = await _apiClient.AddColorToPaletteAsync(paletteId, request);
            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding color to palette {PaletteId}", paletteId);
            return false;
        }
    }
}