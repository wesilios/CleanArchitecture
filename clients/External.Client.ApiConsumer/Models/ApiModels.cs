namespace External.Client.ApiConsumer.Models;

// API Response wrapper
public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
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

// Response models
public class PalettePaginationResponse
{
    public IEnumerable<PaletteResponse> Results { get; set; } = new List<PaletteResponse>();
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
    public IEnumerable<ColorResponse> Colors { get; set; } = new List<ColorResponse>();
}

public class ColorResponse
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; }
    public string Hex { get; set; } = string.Empty;
}