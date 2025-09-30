using Chroma.Domain.Exceptions;
using Chroma.Domain.ValueObjects;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Chroma.Domain.UnitTests.ValueObjects;

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
        color.RedPigment.Should().Be(red);
        color.GreenPigment.Should().Be(green);
        color.BluePigment.Should().Be(blue);
        color.Opacity.Should().Be(1); // Default opacity
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
        color.RedPigment.Should().Be(red);
        color.GreenPigment.Should().Be(green);
        color.BluePigment.Should().Be(blue);
        color.Opacity.Should().Be(opacity);
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
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Color values must be between 0 and 255*");
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
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Opacity value must be between 0 and 1*");
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
        color.RedPigment.Should().Be(expectedRed);
        color.GreenPigment.Should().Be(expectedGreen);
        color.BluePigment.Should().Be(expectedBlue);
        color.Opacity.Should().Be(expectedOpacity);
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
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_WithSupportedColorHex_ShouldReturnColor()
    {
        // Arrange
        var whiteHex = "#FFFFFF";

        // Act
        var color = Color.CreateFromHex(whiteHex);

        // Assert
        color.Should().Be(Color.White);
    }

    [Fact]
    public void From_WithUnsupportedColorHex_ShouldThrowUnsupportedColorException()
    {
        // Arrange
        var unsupportedHex = "#123456";

        // Act
        Action act = () => Color.CreateFromHex(unsupportedHex);

        // Assert
        act.Should().Throw<UnsupportedColorException>()
            .And.Message.Should().Contain(unsupportedHex);
    }

    [Fact]
    public void StaticColors_ShouldHaveCorrectRgbValues()
    {
        // Assert White
        Color.White.RedPigment.Should().Be(255);
        Color.White.GreenPigment.Should().Be(255);
        Color.White.BluePigment.Should().Be(255);

        // Assert Red
        Color.Red.RedPigment.Should().Be(255);
        Color.Red.GreenPigment.Should().Be(87);
        Color.Red.BluePigment.Should().Be(51);

        // Assert Orange
        Color.Orange.RedPigment.Should().Be(255);
        Color.Orange.GreenPigment.Should().Be(195);
        Color.Orange.BluePigment.Should().Be(0);

        // Assert Yellow
        Color.Yellow.RedPigment.Should().Be(255);
        Color.Yellow.GreenPigment.Should().Be(255);
        Color.Yellow.BluePigment.Should().Be(102);

        // Assert Green
        Color.Green.RedPigment.Should().Be(204);
        Color.Green.GreenPigment.Should().Be(255);
        Color.Green.BluePigment.Should().Be(153);

        // Assert Blue
        Color.Blue.RedPigment.Should().Be(102);
        Color.Blue.GreenPigment.Should().Be(102);
        Color.Blue.BluePigment.Should().Be(255);

        // Assert Purple
        Color.Purple.RedPigment.Should().Be(153);
        Color.Purple.GreenPigment.Should().Be(102);
        Color.Purple.BluePigment.Should().Be(204);

        // Assert Grey
        Color.Grey.RedPigment.Should().Be(153);
        Color.Grey.GreenPigment.Should().Be(153);
        Color.Grey.BluePigment.Should().Be(153);
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
        result.Should().Be(expectedHex);
    }

    [Fact]
    public void ToString_ShouldReturnHexString()
    {
        // Arrange
        var color = Color.White;

        // Act
        var result = color.ToString();

        // Assert
        result.Should().Be("#FFFFFF");
    }

    [Fact]
    public void ExplicitOperator_WithValidHexString_ShouldReturnColor()
    {
        // Arrange
        var hexString = "#FFFFFF";

        // Act
        var color = (Color)hexString;

        // Assert
        color.Should().Be(Color.White);
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
        act.Should().Throw<UnsupportedColorException>();
    }


    [Fact]
    public void Equals_WithSameRgbValues_ShouldReturnTrue()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(255, 128, 64);

        // Act & Assert
        color1.Equals(color2).Should().BeTrue();
        (color1 == color2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentRgbValues_ShouldReturnFalse()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(128, 255, 64);

        // Act & Assert
        color1.Equals(color2).Should().BeFalse();
        (color1 != color2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var color = new Color(255, 128, 64);

        // Act & Assert
        color.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var color = new Color(255, 128, 64);
        var otherObject = "not a color";

        // Act & Assert
        color.Equals(otherObject).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameColors_ShouldReturnSameHashCode()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(255, 128, 64);

        // Act & Assert
        color1.GetHashCode().Should().Be(color2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentColors_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var color1 = new Color(255, 128, 64);
        var color2 = new Color(128, 255, 64);

        // Act & Assert
        color1.GetHashCode().Should().NotBe(color2.GetHashCode());
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
        supportedColors.Should().Contain(expectedColors);
        supportedColors.Should().HaveCount(expectedColors.Length);
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
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithBoundaryValues_ShouldWork()
    {
        // Act & Assert
        Action act1 = () => new Color(0, 0, 0);
        Action act2 = () => new Color(255, 255, 255);
        Action act3 = () => new Color(0, 255, 0);
        Action act4 = () => new Color(255, 0, 255);

        act1.Should().NotThrow();
        act2.Should().NotThrow();
        act3.Should().NotThrow();
        act4.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithValidRgbValues_ShouldCreateColorUsingShouldly()
    {
        // Arrange
        int red = 100, green = 150, blue = 200;

        // Act
        var color = new Color(red, green, blue);

        // Assert using Shouldly
        color.RedPigment.ShouldBe(red);
        color.GreenPigment.ShouldBe(green);
        color.BluePigment.ShouldBe(blue);
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
        color.Should().Be(Color.White);
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
        blended.RedPigment.Should().Be(127);
        blended.GreenPigment.Should().Be(0);
        blended.BluePigment.Should().Be(127);
    }

    [Fact]
    public void Lighten_WithValidFactor_ShouldReturnLighterColor()
    {
        // Arrange
        var darkGray = new Color(100, 100, 100);

        // Act
        var lightened = darkGray.Lighten(0.5m);

        // Assert
        lightened.RedPigment.Should().BeGreaterThan(100);
        lightened.GreenPigment.Should().BeGreaterThan(100);
        lightened.BluePigment.Should().BeGreaterThan(100);
    }

    [Fact]
    public void Darken_WithValidFactor_ShouldReturnDarkerColor()
    {
        // Arrange
        var lightGray = new Color(200, 200, 200);

        // Act
        var darkened = lightGray.Darken(0.5m);

        // Assert
        darkened.RedPigment.Should().BeLessThan(200);
        darkened.GreenPigment.Should().BeLessThan(200);
        darkened.BluePigment.Should().BeLessThan(200);
    }

    [Fact]
    public void WithOpacity_ShouldReturnColorWithNewOpacity()
    {
        // Arrange
        var color = Color.Red;

        // Act
        var semiTransparent = color.WithOpacity(0.50m);

        // Assert
        semiTransparent.Opacity.Should().Be(0.50m);
        semiTransparent.RedPigment.Should().Be(color.RedPigment);
        semiTransparent.GreenPigment.Should().Be(color.GreenPigment);
        semiTransparent.BluePigment.Should().Be(color.BluePigment);
    }

    [Fact]
    public void IsTransparent_WithZeroOpacity_ShouldReturnTrue()
    {
        // Arrange
        var transparent = Color.Transparent;

        // Act & Assert
        transparent.IsTransparent.Should().BeTrue();
        transparent.IsOpaque.Should().BeFalse();
    }

    [Fact]
    public void IsOpaque_WithFullOpacity_ShouldReturnTrue()
    {
        // Arrange
        var opaque = Color.White;

        // Act & Assert
        opaque.IsOpaque.Should().BeTrue();
        opaque.IsTransparent.Should().BeFalse();
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
        hexWithOpacity.Should().Be("#FF804080");
        hexWithoutOpacity.Should().Be("#FF8040");
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