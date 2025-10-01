namespace Chroma.Domain.Entities;

public class Palette : IAggregateRoot
{
    private const int MaxColors = 5;

    private readonly List<Color> _colors = [];

    public long PaletteId { get; private set; }
    public string Name { get; set; }

    private Palette()
    {
    }

    public Palette(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required.");
        Name = name;
    }

    public IReadOnlyCollection<Color> Colors => _colors.AsReadOnly();

    public void AddColor(Color newColor)
    {
        if (_colors.Count >= MaxColors)
        {
            throw new InvalidOperationException("Palette cannot contain more than 5 colors.");
        }

        if (_colors.Contains(newColor))
        {
            throw new InvalidOperationException("A color with the same RGB and Opacity already exists.");
        }

        _colors.Add(newColor);
    }
}