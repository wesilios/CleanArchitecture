using System.Text.RegularExpressions;

namespace CleanArchitecture.Domain.ValueObjects;

public class Color : ValueObject
{
    private const int MinColorValue = 0;
    private const int MaxColorValue = 255;
    private const decimal MinOpacityValue = decimal.Zero;
    private const decimal MaxOpacityValue = decimal.One;

    public int R { get; }
    public int G { get; }
    public int B { get; }
    public decimal A { get; }

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

    public Color(){}
    
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

        R = red;
        G = green;
        B = blue;
        A = Math.Round(opacity, 2);
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
            R = Convert.ToInt32(hexValue.Substring(0, 2), 16);
            G = Convert.ToInt32(hexValue.Substring(2, 2), 16);
            B = Convert.ToInt32(hexValue.Substring(4, 2), 16);
            A = hexValue.Length == 8
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
        yield return R;
        yield return G;
        yield return B;
        yield return A;
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

    public string ToHexString(bool includeOpacity = false)
    {
        if (includeOpacity)
        {
            var opacityByte = (int)Math.Round(A * MaxColorValue);
            return $"#{R:X2}{G:X2}{B:X2}{opacityByte:X2}";
        }

        return $"#{R:X2}{G:X2}{B:X2}";
    }

    public Color BlendWith(Color other, decimal ratio = 0.5m)
    {
        if (ratio < MinOpacityValue || ratio > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(nameof(ratio), "Blend ratio must be between 0.0 and 1.0");
        }

        var blendedRed = (int)(R * (MaxOpacityValue - ratio) + other.R * ratio);
        var blendedGreen = (int)(G * (MaxOpacityValue - ratio) + other.G * ratio);
        var blendedBlue = (int)(B * (MaxOpacityValue - ratio) + other.B * ratio);
        var blendedOpacity = A * (MaxOpacityValue - ratio) + other.A * ratio;

        return new Color(blendedRed, blendedGreen, blendedBlue, blendedOpacity);
    }

    public Color Lighten(decimal factor = 0.2m)
    {
        if (factor < MinOpacityValue || factor > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "Lightening factor must be between 0.0 and 1.0");
        }

        var lightenedRed = Math.Min(MaxColorValue, (int)(R + (MaxColorValue - R) * factor));
        var lightenedGreen = Math.Min(MaxColorValue, (int)(G + (MaxColorValue - G) * factor));
        var lightenedBlue = Math.Min(MaxColorValue, (int)(B + (MaxColorValue - B) * factor));

        return new Color(lightenedRed, lightenedGreen, lightenedBlue, A);
    }

    public Color Darken(decimal factor = 0.2m)
    {
        if (factor < MinOpacityValue || factor > MaxOpacityValue)
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "Darkening factor must be between 0.0 and 1.0");
        }

        var darkenedRed = Math.Max(MinColorValue, (int)(R * (MaxOpacityValue - factor)));
        var darkenedGreen = Math.Max(MinColorValue, (int)(G * (MaxOpacityValue - factor)));
        var darkenedBlue = Math.Max(MinColorValue, (int)(B * (MaxOpacityValue - factor)));

        return new Color(darkenedRed, darkenedGreen, darkenedBlue, A);
    }

    public Color WithOpacity(decimal opacity)
    {
        return new Color(R, G, B, opacity);
    }

    public bool IsTransparent => A == MinOpacityValue;

    public bool IsOpaque => A == MaxOpacityValue;
}