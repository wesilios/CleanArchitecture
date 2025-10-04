using System.Text;
using System.Text.Json;
using External.Client.ApiConsumer.Models;
using Microsoft.Extensions.Logging;

namespace External.Client.ApiConsumer.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = null
        };
    }

    public async Task<ApiResponse<PalettePaginationResponse>?> GetPalettesAsync(int pageNumber, int pageSize,
        string? searchTerm = null)
    {
        try
        {
            var queryParams = $"?PageNumber={pageNumber}&PageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryParams += $"&SearchTerm={Uri.EscapeDataString(searchTerm)}";
            }

            _logger.LogInformation("Getting palettes with query: {QueryParams}", queryParams);

            var response = await _httpClient.GetAsync($"palettes{queryParams}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<PalettePaginationResponse>>(content, _jsonOptions);
            }

            _logger.LogWarning("Failed to get palettes. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, content);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting palettes");
            return null;
        }
    }

    public async Task<ApiResponse<PaletteResponse>?> GetPaletteByIdAsync(long paletteId)
    {
        try
        {
            _logger.LogInformation("Getting palette with ID: {PaletteId}", paletteId);

            var response = await _httpClient.GetAsync($"palettes/{paletteId}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<PaletteResponse>>(content, _jsonOptions);
            }

            _logger.LogWarning("Failed to get palette {PaletteId}. Status: {StatusCode}, Content: {Content}",
                paletteId, response.StatusCode, content);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting palette {PaletteId}", paletteId);
            return null;
        }
    }

    public async Task<ApiResponse<object>?> CreatePaletteAsync(CreatePaletteRequest request)
    {
        try
        {
            _logger.LogInformation("Creating palette with name: {Name}", request.Name);

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("palettes", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
            }

            _logger.LogWarning("Failed to create palette. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, responseContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating palette");
            return null;
        }
    }

    public async Task<ApiResponse<object>?> UpdatePaletteAsync(long paletteId, UpdatePaletteRequest request)
    {
        try
        {
            _logger.LogInformation("Updating palette {PaletteId} with name: {Name}", paletteId, request.Name);

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"palettes/{paletteId}", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
            }

            _logger.LogWarning("Failed to update palette {PaletteId}. Status: {StatusCode}, Content: {Content}",
                paletteId, response.StatusCode, responseContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating palette {PaletteId}", paletteId);
            return null;
        }
    }

    public async Task<ApiResponse<object>?> DeletePaletteAsync(long paletteId)
    {
        try
        {
            _logger.LogInformation("Deleting palette with ID: {PaletteId}", paletteId);

            var response = await _httpClient.DeleteAsync($"palettes/{paletteId}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<object>>(content, _jsonOptions);
            }

            _logger.LogWarning("Failed to delete palette {PaletteId}. Status: {StatusCode}, Content: {Content}",
                paletteId, response.StatusCode, content);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting palette {PaletteId}", paletteId);
            return null;
        }
    }

    public async Task<ApiResponse<object>?> AddColorToPaletteAsync(long paletteId, CreatePaletteColorRequest request)
    {
        try
        {
            _logger.LogInformation("Adding color to palette {PaletteId}: R={R}, G={G}, B={B}, A={A}",
                paletteId, request.R, request.G, request.B, request.A);

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"palettes/{paletteId}/colors", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
            }

            _logger.LogWarning("Failed to add color to palette {PaletteId}. Status: {StatusCode}, Content: {Content}",
                paletteId, response.StatusCode, responseContent);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding color to palette {PaletteId}", paletteId);
            return null;
        }
    }
}