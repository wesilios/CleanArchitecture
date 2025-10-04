using External.Client.ApiConsumer.Models;
using Microsoft.Extensions.Logging;

namespace External.Client.ApiConsumer.Services.HttpClients;

/// <summary>
/// Palette API client implementation using Refit with proper error handling and logging
/// </summary>
public class PaletteApiClientService : IApiClientService
{
    private readonly IPaletteApiClient _paletteApiClient;
    private readonly ILogger<PaletteApiClientService> _logger;

    public PaletteApiClientService(IPaletteApiClient paletteApiClient, ILogger<PaletteApiClientService> logger)
    {
        _paletteApiClient = paletteApiClient;
        _logger = logger;
    }

    public async Task<BaseApiResponse<PalettePaginationResponse>?> GetPalettesAsync(int pageNumber, int pageSize,
        string? searchTerm = null)
    {
        _logger.LogInformation("Getting palettes - Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm}",
            pageNumber, pageSize, searchTerm);

        var response = await _paletteApiClient.GetPalettesAsync(pageNumber, pageSize, searchTerm);

        // Check if Refit captured an error
        if (response.Error != null)
        {
            _logger.LogWarning("API error while getting palettes: {Error}", response.Error.Message);
            return null;
        }

        // Check if the API response indicates success
        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _logger.LogDebug("Successfully retrieved palettes");
            return response.Content;
        }

        _logger.LogWarning("API returned unsuccessful response. Status: {StatusCode}, Message: {Message}",
            response.StatusCode, response.Content?.Message);
        return null;
    }

    public async Task<BaseApiResponse<PaletteResponse>?> GetPaletteByIdAsync(long paletteId)
    {
        _logger.LogInformation("Getting palette with ID: {PaletteId}", paletteId);

        var response = await _paletteApiClient.GetPaletteByIdAsync(paletteId);

        if (response.Error != null)
        {
            _logger.LogWarning("API error while getting palette {PaletteId}: {Error}", paletteId,
                response.Error.Message);
            return null;
        }

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _logger.LogDebug("Successfully retrieved palette {PaletteId}", paletteId);
            return response.Content;
        }

        _logger.LogWarning(
            "API returned unsuccessful response for palette {PaletteId}. Status: {StatusCode}, Message: {Message}",
            paletteId, response.StatusCode, response.Content?.Message);
        return null;
    }

    public async Task<BaseApiResponse<object>?> CreatePaletteAsync(CreatePaletteRequest request)
    {
        _logger.LogInformation("Creating palette with name: {Name}", request.Name);

        var response = await _paletteApiClient.CreatePaletteAsync(request);

        if (response.Error != null)
        {
            _logger.LogWarning("API error while creating palette: {Error}", response.Error.Message);
            return null;
        }

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _logger.LogDebug("Successfully created palette: {Name}", request.Name);
            return response.Content;
        }

        _logger.LogWarning(
            "API returned unsuccessful response while creating palette. Status: {StatusCode}, Message: {Message}",
            response.StatusCode, response.Content?.Message);
        return null;
    }

    public async Task<BaseApiResponse<object>?> UpdatePaletteAsync(long paletteId, UpdatePaletteRequest request)
    {
        _logger.LogInformation("Updating palette {PaletteId} with name: {Name}", paletteId, request.Name);

        var response = await _paletteApiClient.UpdatePaletteAsync(paletteId, request);

        if (response.Error != null)
        {
            _logger.LogWarning("API error while updating palette {PaletteId}: {Error}", paletteId,
                response.Error.Message);
            return null;
        }

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _logger.LogDebug("Successfully updated palette {PaletteId}", paletteId);
            return response.Content;
        }

        _logger.LogWarning(
            "API returned unsuccessful response while updating palette {PaletteId}. Status: {StatusCode}, Message: {Message}",
            paletteId, response.StatusCode, response.Content?.Message);
        return null;
    }

    public async Task<BaseApiResponse<object>?> DeletePaletteAsync(long paletteId)
    {
        _logger.LogInformation("Deleting palette with ID: {PaletteId}", paletteId);

        var response = await _paletteApiClient.DeletePaletteAsync(paletteId);

        if (response.Error != null)
        {
            _logger.LogWarning("API error while deleting palette {PaletteId}: {Error}", paletteId,
                response.Error.Message);
            return null;
        }

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _logger.LogDebug("Successfully deleted palette {PaletteId}", paletteId);
            return response.Content;
        }

        _logger.LogWarning(
            "API returned unsuccessful response while deleting palette {PaletteId}. Status: {StatusCode}, Message: {Message}",
            paletteId, response.StatusCode, response.Content?.Message);
        return null;
    }

    public async Task<BaseApiResponse<object>?> AddColorToPaletteAsync(long paletteId,
        CreatePaletteColorRequest request)
    {
        _logger.LogInformation("Adding color to palette {PaletteId}: R={R}, G={G}, B={B}, A={A}",
            paletteId, request.R, request.G, request.B, request.A);

        var response = await _paletteApiClient.AddColorToPaletteAsync(paletteId, request);

        if (response.Error != null)
        {
            _logger.LogWarning("API error while adding color to palette {PaletteId}: {Error}", paletteId,
                response.Error.Message);
            return null;
        }

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            _logger.LogDebug("Successfully added color to palette {PaletteId}", paletteId);
            return response.Content;
        }

        _logger.LogWarning(
            "API returned unsuccessful response while adding color to palette {PaletteId}. Status: {StatusCode}, Message: {Message}",
            paletteId, response.StatusCode, response.Content?.Message);
        return null;
    }
}