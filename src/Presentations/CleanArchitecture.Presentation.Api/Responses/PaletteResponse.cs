using System.Text.Json.Serialization;
using CleanArchitecture.Application.DataObjects;

namespace CleanArchitecture.Presentation.Api.Responses;

public interface IPaletteResponse
{
    long PaletteId { get; set; }
    string Name { get; set; }
    List<ColorResponse> Colors { get; set; }
    [JsonIgnore] bool Empty { get; set; }
}

public class PaletteResponse : IPaletteResponse
{
    public static PaletteResponse CreateInstance(IPaletteDto dto)
    {
        return new PaletteResponse
        {
            PaletteId = dto.PaletteId,
            Name = dto.Name,
            Colors = dto.Colors.Select(c => new ColorResponse
            {
                R = c.R,
                G = c.G,
                B = c.B,
                A = c.A,
                Hex = c.Hex
            }).ToList()
        };
    }

    public long PaletteId { get; set; }
    public string Name { get; set; }
    public List<ColorResponse> Colors { get; set; }
    [JsonIgnore]
    public bool Empty { get; set; } = false;
}

public class NullPaletteResponse : IPaletteResponse
{
    public static readonly NullPaletteResponse Instance = new();

    public long PaletteId { get; set; } = 0;
    public string Name { get; set; } = "Not Found";
    public List<ColorResponse> Colors { get; set; } = [];
    [JsonIgnore]
    public bool Empty { get; set; } = true;
}