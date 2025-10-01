namespace Chroma.Application.DataObjects;

public class PaletteDto
{
    public long PaletteId { get; set; }
    public string Name { get; set; }
    public List<ColorDto> Colors { get; set; }
}

public class ColorDto
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; }
}