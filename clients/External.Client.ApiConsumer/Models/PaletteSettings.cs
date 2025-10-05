namespace External.Client.ApiConsumer.Models;

/// <summary>
/// Configuration settings for palette operations
/// </summary>
public class PaletteSettings
{
    /// <summary>
    /// Maximum number of colors allowed per palette
    /// </summary>
    public int MaxColorsPerPalette { get; set; } = 4;

    /// <summary>
    /// Validates the palette settings
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when MaxColorsPerPalette is invalid</exception>
    public void Validate()
    {
        if (MaxColorsPerPalette <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxColorsPerPalette),
                "Maximum colors per palette must be greater than 0");
        }

        if (MaxColorsPerPalette > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(MaxColorsPerPalette),
                "Maximum colors per palette cannot exceed 10 for UI display reasons");
        }
    }
}