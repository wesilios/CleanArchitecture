namespace Chroma.Presentation.Api.Responses;

public interface IPaletteResponse
{
    long PaletteId { get; set; }
    string Name { get; set; }
    List<ColorResponse> Colors { get; set; }
    bool Empty { get; set; }
}

public class PaletteResponse : IPaletteResponse
{
    public long PaletteId { get; set; }
    public string Name { get; set; }
    public List<ColorResponse> Colors { get; set; }
    public bool Empty { get; set; } = false;
}

public class NullPaletteResponse : IPaletteResponse
{
    public static readonly NullPaletteResponse Instance = new NullPaletteResponse();
    
    public long PaletteId { get; set; } = 0;
    public string Name { get; set; } = "Not Found";
    public List<ColorResponse> Colors { get; set; } = [];
    public bool Empty { get; set; } = true;
}
