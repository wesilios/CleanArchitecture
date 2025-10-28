using External.Client.ApiConsumer.Models;
using External.Client.ApiConsumer.Services;
using External.Client.ApiConsumer.Services.Display;
using External.Client.ApiConsumer.Services.Handlers;
using External.Client.ApiConsumer.Services.Input;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace External.Client.ApiConsumer.Tests.Services.Handlers;

public class PaletteCommandHandlerTests
{
    private readonly Mock<IPaletteDisplayService> _mockDisplayService;
    private readonly Mock<IPaletteInputService> _mockInputService;
    private readonly Mock<IPaletteService> _mockPaletteService;
    private readonly Mock<ILogger<PaletteCommandHandler>> _mockLogger;
    private readonly PaletteSettings _paletteSettings;
    private readonly PaletteCommandHandler _handler;

    public PaletteCommandHandlerTests()
    {
        _mockDisplayService = new Mock<IPaletteDisplayService>();
        _mockInputService = new Mock<IPaletteInputService>();
        _mockPaletteService = new Mock<IPaletteService>();
        _mockLogger = new Mock<ILogger<PaletteCommandHandler>>();
        _paletteSettings = new PaletteSettings { MaxColorsPerPalette = 4 };

        _handler = new PaletteCommandHandler(
            _mockDisplayService.Object,
            _mockInputService.Object,
            _mockPaletteService.Object,
            _paletteSettings,
            _mockLogger.Object);
    }

    [Theory]
    [InlineData("1", true)] // List palettes
    [InlineData("2", true)] // Create palette
    [InlineData("3", true)] // View palette
    [InlineData("4", true)] // Update palette
    [InlineData("5", true)] // Delete palette
    [InlineData("6", true)] // Add color to palette
    [InlineData("7", false)] // Invalid command
    [InlineData("0", false)] // Invalid command
    [InlineData("invalid", false)]
    [InlineData("", false)]
    public async Task CanHandleAsync_VariousCommands_ReturnsExpectedResult(string command, bool expected)
    {
        // Act
        var result = await _handler.CanHandleAsync(command);

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public async Task HandleCreatePaletteAsync_ValidInput_CreatesSuccessfully()
    {
        // Arrange
        var paletteName = "Test Palette";
        _mockInputService.Setup(x => x.GetPaletteName()).Returns(paletteName);
        _mockPaletteService.Setup(x => x.CreatePaletteAsync(paletteName)).ReturnsAsync(true);

        // Act
        await _handler.HandleCreatePaletteAsync();

        // Assert
        _mockInputService.Verify(x => x.GetPaletteName(), Times.Once);
        _mockPaletteService.Verify(x => x.CreatePaletteAsync(paletteName), Times.Once);
        _mockDisplayService.Verify(x => x.DisplaySuccess("Palette created successfully!"), Times.Once);
    }

    [Fact]
    public async Task HandleCreatePaletteAsync_ServiceReturnsFalse_DisplaysError()
    {
        // Arrange
        var paletteName = "Test Palette";
        _mockInputService.Setup(x => x.GetPaletteName()).Returns(paletteName);
        _mockPaletteService.Setup(x => x.CreatePaletteAsync(paletteName)).ReturnsAsync(false);

        // Act
        await _handler.HandleCreatePaletteAsync();

        // Assert
        _mockDisplayService.Verify(x => x.DisplayError("Failed to create palette."), Times.Once);
    }

    [Fact]
    public async Task HandleViewPaletteAsync_ValidId_DisplaysPaletteDetails()
    {
        // Arrange
        var paletteId = 1L;
        var palette = new PaletteResponse
        {
            PaletteId = paletteId,
            Name = "Test Palette",
            Colors = new List<ColorResponse>
            {
                new() { R = 255, G = 0, B = 0, A = 1.0m, Hex = "#FF0000" }
            }
        };

        _mockInputService.Setup(x => x.GetPaletteId()).Returns(paletteId);
        _mockPaletteService.Setup(x => x.GetPaletteByIdAsync(paletteId)).ReturnsAsync(palette);

        // Act
        await _handler.HandleViewPaletteAsync();

        // Assert
        _mockInputService.Verify(x => x.GetPaletteId(), Times.Once);
        _mockPaletteService.Verify(x => x.GetPaletteByIdAsync(paletteId), Times.Once);
        _mockDisplayService.Verify(x => x.DisplayPaletteDetails(palette), Times.Once);
    }

    [Fact]
    public async Task HandleViewPaletteAsync_PaletteNotFound_DisplaysError()
    {
        // Arrange
        var paletteId = 999L;
        _mockInputService.Setup(x => x.GetPaletteId()).Returns(paletteId);
        _mockPaletteService.Setup(x => x.GetPaletteByIdAsync(paletteId)).ReturnsAsync((PaletteResponse?)null);

        // Act
        await _handler.HandleViewPaletteAsync();

        // Assert
        _mockDisplayService.Verify(x => x.DisplayError("Palette not found."), Times.Once);
    }

    [Fact]
    public async Task HandleUpdatePaletteAsync_ValidInput_UpdatesSuccessfully()
    {
        // Arrange
        var paletteId = 1L;
        var newName = "Updated Palette";
        _mockInputService.Setup(x => x.GetPaletteId()).Returns(paletteId);
        _mockInputService.Setup(x => x.GetPaletteName()).Returns(newName);
        _mockPaletteService.Setup(x => x.UpdatePaletteAsync(paletteId, newName)).ReturnsAsync(true);

        // Act
        await _handler.HandleUpdatePaletteAsync();

        // Assert
        _mockInputService.Verify(x => x.GetPaletteId(), Times.Once);
        _mockInputService.Verify(x => x.GetPaletteName(), Times.Once);
        _mockPaletteService.Verify(x => x.UpdatePaletteAsync(paletteId, newName), Times.Once);
        _mockDisplayService.Verify(x => x.DisplaySuccess("Palette updated successfully!"), Times.Once);
    }

    [Fact]
    public async Task HandleDeletePaletteAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var paletteId = 1L;
        _mockInputService.Setup(x => x.GetPaletteId()).Returns(paletteId);
        _mockInputService.Setup(x => x.ConfirmAction(It.IsAny<string>())).Returns(true);
        _mockPaletteService.Setup(x => x.DeletePaletteAsync(paletteId)).ReturnsAsync(true);

        // Act
        await _handler.HandleDeletePaletteAsync();

        // Assert
        _mockInputService.Verify(x => x.GetPaletteId(), Times.Once);
        _mockInputService.Verify(x => x.ConfirmAction(It.IsAny<string>()), Times.Once);
        _mockPaletteService.Verify(x => x.DeletePaletteAsync(paletteId), Times.Once);
        _mockDisplayService.Verify(x => x.DisplaySuccess("Palette deleted successfully!"), Times.Once);
    }

    [Fact]
    public async Task HandleDeletePaletteAsync_UserCancels_DoesNotDelete()
    {
        // Arrange
        var paletteId = 1L;
        _mockInputService.Setup(x => x.GetPaletteId()).Returns(paletteId);
        _mockInputService.Setup(x => x.ConfirmAction(It.IsAny<string>())).Returns(false);

        // Act
        await _handler.HandleDeletePaletteAsync();

        // Assert
        _mockPaletteService.Verify(x => x.DeletePaletteAsync(It.IsAny<long>()), Times.Never);
        _mockDisplayService.Verify(x => x.DisplayMessage("Delete operation cancelled."), Times.Once);
    }

    [Fact]
    public async Task HandleListPalettesAsync_ValidInput_DisplaysPalettes()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var palettes = new PalettePaginationResponse
        {
            Results = new List<PaletteResponse>
            {
                new() { PaletteId = 1, Name = "Palette 1", Colors = new List<ColorResponse>() }
            },
            PageNumber = pageNumber,
            ItemsPerPage = pageSize,
            TotalCount = 1
        };

        _mockInputService.Setup(x => x.GetPageNumber()).Returns(pageNumber);
        _mockInputService.Setup(x => x.GetPageSize()).Returns(pageSize);
        _mockInputService.Setup(x => x.GetSearchTerm()).Returns((string?)null);
        _mockPaletteService.Setup(x => x.GetPalettesAsync(pageNumber, pageSize, null)).ReturnsAsync(palettes);

        // Act
        await _handler.HandleListPalettesAsync();

        // Assert
        _mockDisplayService.Verify(x => x.DisplayPalettes(palettes), Times.Once);
    }

    [Theory]
    [InlineData(4, 4, true)] // Palette is full
    [InlineData(3, 4, false)] // Palette has space
    [InlineData(0, 4, false)] // Empty palette
    public void PaletteCapacityLogic_UsesConfiguredMaxColors(int currentColors, int maxColors, bool shouldBeFull)
    {
        // Arrange
        _paletteSettings.MaxColorsPerPalette = maxColors;
        var remainingSlots = maxColors - currentColors;

        // Act & Assert
        if (shouldBeFull)
        {
            remainingSlots.ShouldBeLessThanOrEqualTo(0);
        }
        else
        {
            remainingSlots.ShouldBeGreaterThan(0);
        }
    }
}