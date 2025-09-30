namespace Chroma.Domain.Entities;

public class Palette : IAggregateRoot
{
    private const int MaxColors = 5;

    // The list now holds the Value Object
    private readonly List<Color> _colors = [];

    public long Id { get; private set; } // Entity ID
    public string Name { get; set; }

    // EF Core needs a parameterless constructor, which can be private
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

        // The uniqueness check now relies on the Color VO's overridden Equals() method.
        if (_colors.Contains(newColor))
        {
            throw new InvalidOperationException("A color with the same RGB and Opacity already exists.");
        }

        _colors.Add(newColor);
    }
}