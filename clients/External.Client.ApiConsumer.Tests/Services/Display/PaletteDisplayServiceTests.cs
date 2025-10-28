using External.Client.ApiConsumer.Models;
using External.Client.ApiConsumer.Services.Display;
using Shouldly;

namespace External.Client.ApiConsumer.Tests.Services.Display;

public class PaletteDisplayServiceTests
{
    private readonly PaletteSettings _paletteSettings;
    private readonly PaletteDisplayService _displayService;

    public PaletteDisplayServiceTests()
    {
        _paletteSettings = new PaletteSettings { MaxColorsPerPalette = 4 };
        _displayService = new PaletteDisplayService(_paletteSettings);
    }

    [Fact]
    public void Constructor_WithPaletteSettings_InitializesCorrectly()
    {
        // Act & Assert
        _displayService.ShouldNotBeNull();
    }

    [Fact]
    public void DisplayMessage_WithValidMessage_DoesNotThrow()
    {
        // Arrange
        var message = "Test message";

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayMessage(message));
    }

    [Fact]
    public void DisplayError_WithValidMessage_DoesNotThrow()
    {
        // Arrange
        var message = "Test error";

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayError(message));
    }

    [Fact]
    public void DisplaySuccess_WithValidMessage_DoesNotThrow()
    {
        // Arrange
        var message = "Test success";

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplaySuccess(message));
    }

    [Fact]
    public void DisplayPaletteDetails_WithValidPalette_DoesNotThrow()
    {
        // Arrange
        var palette = new PaletteResponse
        {
            PaletteId = 1,
            Name = "Test Palette",
            CreatedTime = DateTime.Now,
            Colors = new List<ColorResponse>
            {
                new() { R = 255, G = 0, B = 0, A = 1.0m, Hex = "#FF0000" },
                new() { R = 0, G = 255, B = 0, A = 1.0m, Hex = "#00FF00" }
            }
        };

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPaletteDetails(palette));
    }

    [Fact]
    public void DisplayPalettes_WithValidPaginationResponse_DoesNotThrow()
    {
        // Arrange
        var palettes = new PalettePaginationResponse
        {
            Results = new List<PaletteResponse>
            {
                new()
                {
                    PaletteId = 1,
                    Name = "Palette 1",
                    CreatedTime = DateTime.Now,
                    Colors = new List<ColorResponse>
                    {
                        new() { R = 255, G = 0, B = 0, A = 1.0m, Hex = "#FF0000" }
                    }
                },
                new()
                {
                    PaletteId = 2,
                    Name = "Palette 2",
                    CreatedTime = DateTime.Now.AddDays(-1),
                    Colors = new List<ColorResponse>()
                }
            },
            PageNumber = 1,
            ItemsPerPage = 10,
            TotalCount = 2
        };

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPalettes(palettes));
    }

    [Fact]
    public void DisplayPaletteCapacity_WithValidParameters_DoesNotThrow()
    {
        // Arrange
        var currentCount = 2;
        var maxColors = _paletteSettings.MaxColorsPerPalette;
        var paletteName = "Test Palette";

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPaletteCapacity(currentCount, maxColors, paletteName));
    }

    [Theory]
    [InlineData(0, 4)]
    [InlineData(2, 4)]
    [InlineData(4, 4)]
    public void DisplayPaletteCapacity_WithDifferentCapacities_DoesNotThrow(int currentCount, int maxColors)
    {
        // Arrange
        var paletteName = "Test Palette";

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPaletteCapacity(currentCount, maxColors, paletteName));
    }

    [Fact]
    public void DisplayPalettes_WithEmptyResults_DoesNotThrow()
    {
        // Arrange
        var emptyPalettes = new PalettePaginationResponse
        {
            Results = new List<PaletteResponse>(),
            PageNumber = 1,
            ItemsPerPage = 10,
            TotalCount = 0
        };

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPalettes(emptyPalettes));
    }

    [Fact]
    public void DisplayPaletteDetails_WithEmptyColors_DoesNotThrow()
    {
        // Arrange
        var paletteWithNoColors = new PaletteResponse
        {
            PaletteId = 1,
            Name = "Empty Palette",
            CreatedTime = DateTime.Now,
            Colors = new List<ColorResponse>()
        };

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPaletteDetails(paletteWithNoColors));
    }

    [Fact]
    public void DisplayPaletteDetails_WithMaxColors_DoesNotThrow()
    {
        // Arrange
        var colors = new List<ColorResponse>();
        for (int i = 0; i < _paletteSettings.MaxColorsPerPalette; i++)
        {
            colors.Add(new ColorResponse
            {
                R = i * 50,
                G = i * 60,
                B = i * 70,
                A = 1.0m,
                Hex = $"#{i * 50:X2}{i * 60:X2}{i * 70:X2}"
            });
        }

        var fullPalette = new PaletteResponse
        {
            PaletteId = 1,
            Name = "Full Palette",
            CreatedTime = DateTime.Now,
            Colors = colors
        };

        // Act & Assert
        Should.NotThrow(() => _displayService.DisplayPaletteDetails(fullPalette));
    }
}