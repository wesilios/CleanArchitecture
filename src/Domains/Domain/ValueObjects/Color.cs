namespace CleanArchitecture.Domain.ValueObjects;

public class Color : ValueObject
{
    public int RedPigment { get; }
    public int GreenPigment { get; }
    public int BluePigment { get; }

    public static Color From(string code)
    {
        var colour = new Color(code);

        if (!SupportedColors.Contains(colour))
        {
            throw new UnsupportedColorException(code);
        }

        return colour;
    }

    public static Color White => new(255, 255, 255);
    public static Color Red => new(255, 87, 51);
    public static Color Orange => new(255, 195, 0);
    public static Color Yellow => new(255, 255, 102);
    public static Color Green => new(204, 255, 153);
    public static Color Blue => new(102, 102, 255);
    public static Color Purple => new(153, 102, 204);
    public static Color Grey => new(153, 153, 153);

    public Color(int red, int green, int blue)
    {
        // Validation is done inside the constructor
        if (red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
        {
            throw new ArgumentOutOfRangeException("Color values must be between 0 and 255.");
        }

        RedPigment = red;
        GreenPigment = green;
        BluePigment = blue;
    }

    public Color(string hexValue)
    {
        // Add validation to ensure the hexValue is valid.
        // For simplicity, this example assumes valid input.
        if (hexValue.StartsWith("#"))
        {
            hexValue = hexValue[1..];
        }

        // Convert hex to integers
        RedPigment = Convert.ToInt32(hexValue.Substring(0, 2), 16);
        GreenPigment = Convert.ToInt32(hexValue.Substring(2, 2), 16);
        BluePigment = Convert.ToInt32(hexValue.Substring(4, 2), 16);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ToHexString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RedPigment, GreenPigment, BluePigment);
    }
    
    public static explicit operator Color(string code)
    {
        return From(code);
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
        }
    }

    public string ToHexString()
    {
        return $"#{RedPigment:X2}{GreenPigment:X2}{BluePigment:X2}";
    }
}