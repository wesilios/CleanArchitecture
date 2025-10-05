using External.Client.ApiConsumer.Models;
using Refit;

namespace External.Client.ApiConsumer.Services.HttpClients;

/// <summary>
/// Refit API service interface for palette operations
/// </summary>
public interface IPaletteApiClient
{
    /// <summary>
    /// Get paginated list of palettes with optional search
    /// </summary>
    [Get("/palettes")]
    Task<ApiResponse<BaseApiResponse<PalettePaginationResponse>>> GetPalettesAsync(
        [Query] int pageNumber,
        [Query] int pageSize,
        [Query] string? searchTerm = null);

    /// <summary>
    /// Get palette by ID
    /// </summary>
    [Get("/palettes/{paletteId}")]
    Task<ApiResponse<BaseApiResponse<PaletteResponse>>> GetPaletteByIdAsync([AliasAs("paletteId")] long paletteId);

    /// <summary>
    /// Create a new palette
    /// </summary>
    [Post("/palettes")]
    Task<ApiResponse<BaseApiResponse<object>>> CreatePaletteAsync([Body] CreatePaletteRequest request);

    /// <summary>
    /// Update an existing palette
    /// </summary>
    [Put("/palettes/{paletteId}")]
    Task<ApiResponse<BaseApiResponse<object>>> UpdatePaletteAsync([AliasAs("paletteId")] long paletteId,
        [Body] UpdatePaletteRequest request);

    /// <summary>
    /// Delete a palette
    /// </summary>
    [Delete("/palettes/{paletteId}")]
    Task<ApiResponse<BaseApiResponse<object>>> DeletePaletteAsync([AliasAs("paletteId")] long paletteId);

    /// <summary>
    /// Add a color to a palette
    /// </summary>
    [Post("/palettes/{paletteId}/colors")]
    Task<ApiResponse<BaseApiResponse<object>>> AddColorToPaletteAsync([AliasAs("paletteId")] long paletteId,
        [Body] CreatePaletteColorRequest request);
}