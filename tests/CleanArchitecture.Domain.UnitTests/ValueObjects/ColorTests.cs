using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Domain.UnitTests.ValueObjects;

public class ColorTests
{
    [Fact]
    public void Constructor_WithValidRgbValues_ShouldCreateColor()
    {
        // Arrange
        int red = 255, green = 128, blue = 64;

        // Act
        var color = new Color(red, green, blue);

        // Assert
        color.R.ShouldBe(red);
        color.G.ShouldBe(green);
        color.B.ShouldBe(blue);
        color.A.ShouldBe(1); // Default opacity
    }

    [Fact]
    public void Constructor_WithValidRgbaValues_ShouldCreateColor()
    {
        // Arrange
        int red = 255, green = 128, blue = 64;
        var opacity = 1.0m;

        // Act
        var color = new Color(red, green, blue, opacity);

        // Assert
        color.R.ShouldBe(red);
        color.G.ShouldBe(green);
        color.B.ShouldBe(blue);
        color.A.ShouldBe(opacity);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 0, -1)]
    [InlineData(256, 0, 0)]
    [InlineData(0, 256, 0)]
    [InlineData(0, 0, 256)]
    [InlineData(300, 300, 300)]
    public void Constructor_WithInvalidRgbValues_ShouldThrowArgumentOutOfRangeException(int red, int green, int blue)
    {
        // Act
        Action act = () => new Color(red, green, blue);

        // Assert
        var exception = Should.Throw<ArgumentOutOfRangeException>(act);
        exception.Message.ShouldContain("Color values must be between 0 and 255");
    }

    [Theory]
    [InlineData(255, 255, 255, -0.1)]
    [InlineData(255, 255, 255, 1.1)]
    public void Constructor_WithInvalidOpacityValues_ShouldThrowArgumentOutOfRangeException(int red, int green,
        int blue, decimal opacity)
    {
        // Act
        Action act = () => new Color(red, green, blue, opacity);

        // Assert
        Should.Throw<ArgumentOutOfRangeException>(act);
    }

    [Theory]
    [InlineData("#FF0000", 255, 0, 0, 1)]
    [InlineData("#00FF00", 0, 255, 0, 1)]
    [InlineData("#0000FF", 0, 0, 255, 1)]
    [InlineData("#FFFFFF", 255, 255, 255, 1)]
    [InlineData("#000000", 0, 0, 0, 1)]
    [InlineData("FF0000", 255, 0, 0, 1)] // Without # prefix
    [InlineData("#FF000080", 255, 0, 0, 0.50)] // With alpha channel
    [InlineData("#00FF0000", 0, 255, 0, 0)] // Transparent green
    public void Constructor_WithValidHexString_ShouldCreateColor(string hexValue, int expectedRed, int expectedGreen,
        int expectedBlue, decimal expectedOpacity)
    {
        // Act
        var color = new Color(hexValue);

        // Assert
        color.R.ShouldBe(expectedRed);
        color.G.ShouldBe(expectedGreen);
        color.B.ShouldBe(expectedBlue);
        color.A.ShouldBe(expectedOpacity);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("#FF")]
    [InlineData("#FFFFF")]
    [InlineData("#GGGGGG")]
    [InlineData("#12345")]
    [InlineData("#1234567")]
    [InlineData("#123456789")]
    public void Constructor_WithInvalidHexString_ShouldThrowArgumentException(string invalidHex)
    {
        // Act
        Action act = () => new Color(invalidHex);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void From_WithSupportedColorHex_ShouldReturnColor()
    {
        // Arrange
        var whiteHex = "#FFFFFF";

        // Act
        var color = Color.CreateFromHex(whiteHex);

        // Assert
        color.ShouldBe(Color.White);
    }

    [Fact]
    public void From_WithUnsupportedColorHex_ShouldThrowUnsupportedColorException()
    {
        // Arrange
        var unsupportedHex = "#123456";

        // Act & Assert
        var exception = Should.Throw<UnsupportedColorException>(() => Color.CreateFromHex(unsupportedHex));
        exception.Message.ShouldContain(unsupportedHex);
    }

    [Fact]
    public void StaticColors_ShouldHaveCorrectRgbValues()
    {
        // Assert White
        Color.White.R.ShouldBe(255);
        Color.White.G.ShouldBe(255);
        Color.White.B.ShouldBe(255);

        // Assert Red
        Color.Red.R.ShouldBe(255);
        Color.Red.G.ShouldBe(87);
        Color.Red.B.ShouldBe(51);

        // Assert Orange
        Color.Orange.R.ShouldBe(255);
        Color.Orange.G.ShouldBe(195);
        Color.Orange.B.ShouldBe(0);

        // Assert Yellow
        Color.Yellow.R.ShouldBe(255);
        Color.Yellow.G.ShouldBe(255);
        Color.Yellow.B.ShouldBe(102);

        // Assert Green
        Color.Green.R.ShouldBe(204);
        Color.Green.G.ShouldBe(255);
        Color.Green.B.ShouldBe(153);

        // Assert Blue
        Color.Blue.R.ShouldBe(102);
        Color.Blue.G.ShouldBe(102);
        Color.Blue.B.ShouldBe(255);

        // Assert Purple
        Color.Purple.R.ShouldBe(153);
        Color.Purple.G.ShouldBe(102);
        Color.Purple.B.ShouldBe(204);

        // Assert Grey
        Color.Grey.R.ShouldBe(153);
        Color.Grey.G.ShouldBe(153);
        Color.Grey.B.ShouldBe(153);
    }

    [Fact]
    public void ToHexString_ShouldReturnCorrectFormat()
    {
        // Arrange
        var color = new Color(255, 128, 64);
        var expectedHex = "#FF8040";

        // Act
        var result = color.ToHexString();

        // Assert
        result.ShouldBe(expectedHex);
    }

    [Fact]
    public void ToString_ShouldReturnHexString()
    {
        // Arrange
        var color = Color.White;

        // Act
        var result = color.ToString();

        // Assert
        result.ShouldBe("#FFFFFF");
    }

    [Fact]
    public void ExplicitOperator_WithValidHexString_ShouldReturnColor()
    {
        // Arrange
        var hexString = "#FFFFFF";

        // Act
        var color = (Color)hexString;

        // Assert
        color.ShouldBe(Color.White);
    }

    [Fact]
    public void ExplicitOperator_WithUnsupportedHexString_ShouldThrowUnsupportedColorException()
    {
        // Arrange
        var unsupportedHex = "#123456";

        // Act & Assert
        var act = () =>
        {
            var color = (Color)unsupportedHex;
        };
        act.ShouldThrow<UnsupportedColorException>();
    }


    [Fact]
    public void Equals_WithSameRgbValues_ShouldReturnTrue()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(255, 128, 64);

        // Act & Assert
        color1.Equals(color2).ShouldBeTrue();
        (color1 == color2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_WithDifferentRgbValues_ShouldReturnFalse()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(128, 255, 64);

        // Act & Assert
        color1.Equals(color2).ShouldBeFalse();
        (color1 != color2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var color = new Color(255, 128, 64);

        // Act & Assert
        color.Equals(null).ShouldBeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var color = new Color(255, 128, 64);
        var otherObject = "not a color";

        // Act & Assert
        color.Equals(otherObject).ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameColors_ShouldReturnSameHashCode()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(255, 128, 64);

        // Act & Assert
        color1.GetHashCode().ShouldBe(color2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentColors_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(128, 255, 64);

        // Act & Assert
        color1.GetHashCode().ShouldNotBe(color2.GetHashCode());
    }

    [Fact]
    public void SupportedColors_ShouldContainAllStaticColors()
    {
        // Arrange
        var expectedColors = new[]
        {
            Color.White,
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Green,
            Color.Blue,
            Color.Purple,
            Color.Grey,
            Color.Transparent
        };

        // Act
        var supportedColors = GetSupportedColors();

        // Assert
        foreach (var color in expectedColors)
            supportedColors.ShouldContain(color);
        supportedColors.Count().ShouldBe(expectedColors.Length);
    }

    [Theory]
    [InlineData("#FF5733")] // Red
    [InlineData("#FFC300")] // Orange
    [InlineData("#FFFF66")] // Yellow
    [InlineData("#CCFF99")] // Green
    [InlineData("#6666FF")] // Blue
    [InlineData("#9966CC")] // Purple
    [InlineData("#999999")] // Grey
    [InlineData("#FFFFFF")] // White
    [InlineData("#00000000")] // Transparent
    public void From_WithAllSupportedColors_ShouldNotThrowException(string colorHex)
    {
        // Act
        Action act = () => Color.CreateFromHex(colorHex);

        // Assert
        act.ShouldNotThrow();
    }

    [Fact]
    public void Constructor_WithBoundaryValues_ShouldWork()
    {
        // Act & Assert
        Action act1 = () => new Color(0, 0, 0);
        Action act2 = () => new Color(255, 255, 255);
        Action act3 = () => new Color(0, 255, 0);
        Action act4 = () => new Color(255, 0, 255);

        act1.ShouldNotThrow();
        act2.ShouldNotThrow();
        act3.ShouldNotThrow();
        act4.ShouldNotThrow();
    }

    [Fact]
    public void Constructor_WithValidRgbValues_ShouldCreateColorUsingShouldly()
    {
        // Arrange
        int red = 100, green = 150, blue = 200;

        // Act
        var color = new Color(red, green, blue);

        // Assert using Shouldly
        color.R.ShouldBe(red);
        color.G.ShouldBe(green);
        color.B.ShouldBe(blue);
    }

    [Fact]
    public void From_WithUnsupportedColor_ShouldThrowUsingShouldly()
    {
        // Arrange
        var unsupportedHex = "#ABCDEF";

        // Act & Assert using Shouldly
        Should.Throw<UnsupportedColorException>(() => Color.CreateFromHex(unsupportedHex))
            .Message.ShouldContain(unsupportedHex);
    }

    [Fact]
    public void ToHexString_ShouldReturnCorrectFormatUsingShouldly()
    {
        // Arrange
        var color = new Color(15, 255, 128);
        var expectedHex = "#0FFF80";

        // Act
        var result = color.ToHexString();

        // Assert using Shouldly
        result.ShouldBe(expectedHex);
    }

    [Fact]
    public void Equals_WithSameColors_ShouldReturnTrueUsingShouldly()
    {
        // Arrange
        var color1 = new Color(50, 100, 150);
        var color2 = new Color(50, 100, 150);

        // Act & Assert using Shouldly
        color1.ShouldBe(color2);
        color1.Equals(color2).ShouldBeTrue();
    }

    [Fact]
    public void CreateFromHex_WithSupportedColorHex_ShouldReturnColor()
    {
        // Arrange
        var whiteHex = "#FFFFFF";

        // Act
        var color = Color.CreateFromHex(whiteHex);

        // Assert
        color.ShouldBe(Color.White);
    }

    [Fact]
    public void BlendWith_WithValidRatio_ShouldReturnBlendedColor()
    {
        // Arrange
        var red = new Color(255, 0, 0);
        var blue = new Color(0, 0, 255);

        // Act
        var blended = red.BlendWith(blue, 0.5m);

        // Assert
        blended.R.ShouldBe(127);
        blended.G.ShouldBe(0);
        blended.B.ShouldBe(127);
    }

    [Fact]
    public void Lighten_WithValidFactor_ShouldReturnLighterColor()
    {
        // Arrange
        var darkGray = new Color(100, 100, 100);

        // Act
        var lightened = darkGray.Lighten(0.5m);

        // Assert
        lightened.R.ShouldBeGreaterThan(100);
        lightened.G.ShouldBeGreaterThan(100);
        lightened.B.ShouldBeGreaterThan(100);
    }

    [Fact]
    public void Darken_WithValidFactor_ShouldReturnDarkerColor()
    {
        // Arrange
        var lightGray = new Color(200, 200, 200);

        // Act
        var darkened = lightGray.Darken(0.5m);

        // Assert
        darkened.R.ShouldBeLessThan(200);
        darkened.G.ShouldBeLessThan(200);
        darkened.B.ShouldBeLessThan(200);
    }

    [Fact]
    public void WithOpacity_ShouldReturnColorWithNewOpacity()
    {
        // Arrange
        var color = Color.Red;

        // Act
        var semiTransparent = color.WithOpacity(0.50m);

        // Assert
        semiTransparent.A.ShouldBe(0.50m);
        semiTransparent.R.ShouldBe(color.R);
        semiTransparent.G.ShouldBe(color.G);
        semiTransparent.B.ShouldBe(color.B);
    }

    [Fact]
    public void IsTransparent_WithZeroOpacity_ShouldReturnTrue()
    {
        // Arrange
        var transparent = Color.Transparent;

        // Act & Assert
        transparent.IsTransparent.ShouldBeTrue();
        transparent.IsOpaque.ShouldBeFalse();
    }

    [Fact]
    public void IsOpaque_WithFullOpacity_ShouldReturnTrue()
    {
        // Arrange
        var opaque = Color.White;

        // Act & Assert
        opaque.IsOpaque.ShouldBeTrue();
        opaque.IsTransparent.ShouldBeFalse();
    }

    [Fact]
    public void ToHexString_WithOpacity_ShouldIncludeAlphaChannel()
    {
        // Arrange
        var color = new Color(255, 128, 64, 0.50m);

        // Act
        var hexWithOpacity = color.ToHexString(true);
        var hexWithoutOpacity = color.ToHexString(false);

        // Assert
        hexWithOpacity.ShouldBe("#FF804080");
        hexWithoutOpacity.ShouldBe("#FF8040");
    }

    // Helper method to access protected SupportedColors property
    private static IEnumerable<Color> GetSupportedColors()
    {
        // Use reflection to access the protected property for testing
        var property = typeof(Color).GetProperty("SupportedColors",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return (IEnumerable<Color>)property!.GetValue(null)!;
    }
}