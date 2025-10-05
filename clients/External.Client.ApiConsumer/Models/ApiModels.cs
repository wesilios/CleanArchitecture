namespace External.Client.ApiConsumer.Models;

// API Response wrapper - matches ApiResult<T> from API
public class BaseApiResponse<T>
{
    public T Data { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }

    /// <summary>
    /// Ensures the API response was successful, throws exception if not
    /// </summary>
    /// <returns>The response data if successful</returns>
    /// <exception cref="ApiResponseException">Thrown when the response indicates failure</exception>
    public T EnsureSuccess()
    {
        if (StatusCode >= 200 && StatusCode < 300)
        {
            return Data;
        }

        throw new ApiResponseException(StatusCode, Message);
    }
}

/// <summary>
/// Exception thrown when API response indicates failure
/// </summary>
public class ApiResponseException : Exception
{
    public int StatusCode { get; }

    public ApiResponseException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiResponseException(int statusCode, string message, Exception innerException) : base(message,
        innerException)
    {
        StatusCode = statusCode;
    }
}

// Request models
public class CreatePaletteRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdatePaletteRequest
{
    public string Name { get; set; } = string.Empty;
}

public class CreatePaletteColorRequest
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; } = 1.0m;
}

// Response models - matches API structure
public class PalettePaginationResponse
{
    public List<PaletteResponse> Results { get; set; } = [];
    public int PageNumber { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalCount { get; set; }

    // Computed properties for compatibility
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / ItemsPerPage);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    // Compatibility property for existing code
    public IEnumerable<PaletteResponse> Items => Results;
    public int PageSize => ItemsPerPage;
}

public class PaletteResponse
{
    public long PaletteId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedTime { get; set; }
    public List<ColorResponse> Colors { get; set; } = [];
}

public class ColorResponse
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; }
    public string Hex { get; set; } = string.Empty;
}