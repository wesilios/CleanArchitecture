using System.Text.RegularExpressions;

namespace Chroma.Domain.ValueObjects;

public class Color : ValueObject
{
    public Color()
    {
    }

    private const int MinColorValue = 0;
    private const int MaxColorValue = 255;
    private const decimal MinOpacityValue = decimal.Zero;
    private const decimal MaxOpacityValue = decimal.One;

    public int RedPigment { get; }
    public int GreenPigment { get; }
    public int BluePigment { get; }
    public decimal Opacity { get; }

    public static Color CreateFromHex(string hexCode)
    {
        var color = new Color(hexCode);

        if (!SupportedColors.Contains(color))
        {
            throw new UnsupportedColorException(hexCode);
        }

        return color;
    }

    public static Color White => new(MaxColorValue, MaxColorValue, MaxColorValue);
    public static Color Red => new(MaxColorValue, 87, 51);
    public static Color Orange => new(MaxColorValue, 195, 0);
    public static Color Yellow => new(MaxColorValue, MaxColorValue, 102);
    public static Color Green => new(204, MaxColorValue, 153);
    public static Color Blue => new(102, 102, MaxColorValue);
    public static Color Purple => new(153, 102, 204);
    public static Color Grey => new(153, 153, 153);
    public static Color Transparent => new(MinColorValue, MinColorValue, MinColorValue, MinOpacityValue);

    public Color(int red, int green, int blue, decimal opacity = MaxOpacityValue)
    {
        // Validation is done inside the constructor
        if (red < MinColorValue || red > MaxColorValue ||
            green < MinColorValue || green > MaxColorValue ||
            blue < MinColorValue || blue > MaxColorValue)
        {
            throw new ArgumentOutOfRangeException($"Color values must be between {MinColorValue} and {MaxColorValue}.");
        }

        if (opacity < MinOpacityValue || opacity > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(
                $"Opacity value must be between {MinOpacityValue} and {MaxOpacityValue}.");
        }

        RedPigment = red;
        GreenPigment = green;
        BluePigment = blue;
        Opacity = Math.Round(opacity, 2);
    }

    public Color(string hexValue)
    {
        if (string.IsNullOrWhiteSpace(hexValue))
        {
            throw new ArgumentException("Hex value cannot be null or empty.", nameof(hexValue));
        }

        if (hexValue.StartsWith("#"))
        {
            hexValue = hexValue[1..];
        }

        if (hexValue.Length != 6 && hexValue.Length != 8)
        {
            throw new ArgumentException("Hex value must be 6 or 8 characters long.", nameof(hexValue));
        }

        if (!Regex.IsMatch(hexValue, "^[0-9A-Fa-f]+$"))
        {
            throw new ArgumentException("Hex value contains invalid characters.", nameof(hexValue));
        }

        try
        {
            RedPigment = Convert.ToInt32(hexValue.Substring(0, 2), 16);
            GreenPigment = Convert.ToInt32(hexValue.Substring(2, 2), 16);
            BluePigment = Convert.ToInt32(hexValue.Substring(4, 2), 16);
            Opacity = hexValue.Length == 8
                ? Math.Round(Convert.ToInt32(hexValue.Substring(6, 2), 16) / 255m, 2)
                : MaxOpacityValue;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Invalid hex value: {hexValue}", nameof(hexValue), ex);
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RedPigment;
        yield return GreenPigment;
        yield return BluePigment;
        yield return Opacity;
    }

    public static explicit operator Color(string hexCode)
    {
        return CreateFromHex(hexCode);
    }

    public override string ToString()
    {
        return ToHexString();
    }

    protected static IEnumerable<Color> SupportedColors
    {
        get
        {
            yield return White;
            yield return Red;
            yield return Orange;
            yield return Yellow;
            yield return Green;
            yield return Blue;
            yield return Purple;
            yield return Grey;
            yield return Transparent;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RedPigment, GreenPigment, BluePigment, Opacity);
    }

    public string ToHexString(bool includeOpacity = false)
    {
        if (includeOpacity)
        {
            var opacityByte = (int)Math.Round(Opacity * MaxColorValue);
            return $"#{RedPigment:X2}{GreenPigment:X2}{BluePigment:X2}{opacityByte:X2}";
        }

        return $"#{RedPigment:X2}{GreenPigment:X2}{BluePigment:X2}";
    }

    public Color BlendWith(Color other, decimal ratio = 0.5m)
    {
        if (ratio < MinOpacityValue || ratio > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(nameof(ratio), "Blend ratio must be between 0.0 and 1.0");
        }

        var blendedRed = (int)(RedPigment * (MaxOpacityValue - ratio) + other.RedPigment * ratio);
        var blendedGreen = (int)(GreenPigment * (MaxOpacityValue - ratio) + other.GreenPigment * ratio);
        var blendedBlue = (int)(BluePigment * (MaxOpacityValue - ratio) + other.BluePigment * ratio);
        var blendedOpacity = Opacity * (MaxOpacityValue - ratio) + other.Opacity * ratio;

        return new Color(blendedRed, blendedGreen, blendedBlue, blendedOpacity);
    }

    public Color Lighten(decimal factor = 0.2m)
    {
        if (factor < MinOpacityValue || factor > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "Lightening factor must be between 0.0 and 1.0");
        }

        var lightenedRed = Math.Min(MaxColorValue, (int)(RedPigment + (MaxColorValue - RedPigment) * factor));
        var lightenedGreen = Math.Min(MaxColorValue, (int)(GreenPigment + (MaxColorValue - GreenPigment) * factor));
        var lightenedBlue = Math.Min(MaxColorValue, (int)(BluePigment + (MaxColorValue - BluePigment) * factor));

        return new Color(lightenedRed, lightenedGreen, lightenedBlue, Opacity);
    }

    public Color Darken(decimal factor = 0.2m)
    {
        if (factor < MinOpacityValue || factor > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "Darkening factor must be between 0.0 and 1.0");
        }

        var darkenedRed = Math.Max(MinColorValue, (int)(RedPigment * (MaxOpacityValue - factor)));
        var darkenedGreen = Math.Max(MinColorValue, (int)(GreenPigment * (MaxOpacityValue - factor)));
        var darkenedBlue = Math.Max(MinColorValue, (int)(BluePigment * (MaxOpacityValue - factor)));

        return new Color(darkenedRed, darkenedGreen, darkenedBlue, Opacity);
    }

    public Color WithOpacity(decimal opacity)
    {
        return new Color(RedPigment, GreenPigment, BluePigment, opacity);
    }

    public bool IsTransparent => Opacity == MinOpacityValue;

    public bool IsOpaque => Opacity == MaxOpacityValue;
}