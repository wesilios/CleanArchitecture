namespace CleanArchitecture.Application.DataObjects;

public interface IPaletteDto : IDto
{
    long PaletteId { get; set; }
    string Name { get; set; }
    DateTime CreatedTime { get; set; }
    List<ColorDto> Colors { get; set; }
}

public class PaletteDto : IPaletteDto
{
    public long PaletteId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedTime { get; set; }
    public List<ColorDto> Colors { get; set; }
    public string Note { get; set; } = string.Empty;
    public bool Empty { get; set; }
}

public class NullPaletteDto : IPaletteDto
{
    public static readonly NullPaletteDto Instance = new();

    public long PaletteId { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedTime { get; set; }
    public List<ColorDto> Colors { get; set; } = [];
    public string Note { get; set; } = "No palette found.";
    public bool Empty { get; set; } = true;
}

public class ColorDto
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; }
    public string Hex { get; set; }
}