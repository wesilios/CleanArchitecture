using External.Client.ApiConsumer.Models;
using External.Client.ApiConsumer.Services;
using External.Client.ApiConsumer.Services.HttpClients;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace External.Client.ApiConsumer.Tests.Services;

public class PaletteServiceTests
{
    private readonly Mock<IApiClientService> _mockApiClient;
    private readonly Mock<ILogger<PaletteService>> _mockLogger;
    private readonly PaletteService _paletteService;

    public PaletteServiceTests()
    {
        _mockApiClient = new Mock<IApiClientService>();
        _mockLogger = new Mock<ILogger<PaletteService>>();
        _paletteService = new PaletteService(_mockApiClient.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetPalettesAsync_ValidParameters_ReturnsExpectedResult()
    {
        // Arrange
        var expectedResponse = new BaseApiResponse<PalettePaginationResponse>
        {
            Data = new PalettePaginationResponse
            {
                Results = new List<PaletteResponse>
                {
                    new() { PaletteId = 1, Name = "Test Palette", Colors = new List<ColorResponse>() }
                },
                PageNumber = 1,
                ItemsPerPage = 10,
                TotalCount = 1
            },
            StatusCode = 200,
            Message = "Success"
        };

        _mockApiClient.Setup(x => x.GetPalettesAsync(1, 10, null))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.GetPalettesAsync(1, 10);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedResponse.Data);
        _mockApiClient.Verify(x => x.GetPalettesAsync(1, 10, null), Times.Once);
    }

    [Fact]
    public async Task GetPalettesAsync_WithSearchTerm_PassesSearchTermToApiClient()
    {
        // Arrange
        var searchTerm = "blue";
        var expectedResponse = new BaseApiResponse<PalettePaginationResponse>
        {
            Data = new PalettePaginationResponse(),
            StatusCode = 200
        };

        _mockApiClient.Setup(x => x.GetPalettesAsync(1, 10, searchTerm))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.GetPalettesAsync(1, 10, searchTerm);

        // Assert
        _mockApiClient.Verify(x => x.GetPalettesAsync(1, 10, searchTerm), Times.Once);
    }

    [Fact]
    public async Task GetPalettesAsync_ApiClientReturnsNull_ReturnsNull()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetPalettesAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((BaseApiResponse<PalettePaginationResponse>?)null);

        // Act
        var result = await _paletteService.GetPalettesAsync(1, 10);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetPaletteByIdAsync_ValidId_ReturnsExpectedResult()
    {
        // Arrange
        var paletteId = 1L;
        var expectedResponse = new BaseApiResponse<PaletteResponse>
        {
            Data = new PaletteResponse
            {
                PaletteId = paletteId,
                Name = "Test Palette",
                Colors = new List<ColorResponse>
                {
                    new() { R = 255, G = 0, B = 0, A = 1.0m, Hex = "#FF0000" }
                }
            },
            StatusCode = 200
        };

        _mockApiClient.Setup(x => x.GetPaletteByIdAsync(paletteId))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.GetPaletteByIdAsync(paletteId);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedResponse.Data);
        _mockApiClient.Verify(x => x.GetPaletteByIdAsync(paletteId), Times.Once);
    }

    [Fact]
    public async Task CreatePaletteAsync_ValidName_ReturnsTrue()
    {
        // Arrange
        var paletteName = "New Palette";
        var expectedResponse = new BaseApiResponse<object>
        {
            Data = new object(),
            StatusCode = 201
        };

        _mockApiClient.Setup(x => x.CreatePaletteAsync(It.Is<CreatePaletteRequest>(r => r.Name == paletteName)))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.CreatePaletteAsync(paletteName);

        // Assert
        result.ShouldBeTrue();
        _mockApiClient.Verify(x => x.CreatePaletteAsync(It.Is<CreatePaletteRequest>(r => r.Name == paletteName)),
            Times.Once);
    }

    [Fact]
    public async Task CreatePaletteAsync_ApiClientReturnsNull_ReturnsFalse()
    {
        // Arrange
        _mockApiClient.Setup(x => x.CreatePaletteAsync(It.IsAny<CreatePaletteRequest>()))
            .ReturnsAsync((BaseApiResponse<object>?)null);

        // Act
        var result = await _paletteService.CreatePaletteAsync("Test");

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdatePaletteAsync_ValidParameters_ReturnsTrue()
    {
        // Arrange
        var paletteId = 1L;
        var newName = "Updated Palette";
        var expectedResponse = new BaseApiResponse<object>
        {
            Data = new object(),
            StatusCode = 200
        };

        _mockApiClient.Setup(x => x.UpdatePaletteAsync(paletteId, It.Is<UpdatePaletteRequest>(r => r.Name == newName)))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.UpdatePaletteAsync(paletteId, newName);

        // Assert
        result.ShouldBeTrue();
        _mockApiClient.Verify(x => x.UpdatePaletteAsync(paletteId, It.Is<UpdatePaletteRequest>(r => r.Name == newName)),
            Times.Once);
    }

    [Fact]
    public async Task DeletePaletteAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var paletteId = 1L;
        var expectedResponse = new BaseApiResponse<object>
        {
            Data = new object(),
            StatusCode = 200
        };

        _mockApiClient.Setup(x => x.DeletePaletteAsync(paletteId))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.DeletePaletteAsync(paletteId);

        // Assert
        result.ShouldBeTrue();
        _mockApiClient.Verify(x => x.DeletePaletteAsync(paletteId), Times.Once);
    }

    [Fact]
    public async Task AddColorToPaletteAsync_ValidParameters_ReturnsTrue()
    {
        // Arrange
        var paletteId = 1L;
        var r = 255;
        var g = 128;
        var b = 0;
        var a = 0.8m;
        var expectedResponse = new BaseApiResponse<object>
        {
            Data = new object(),
            StatusCode = 200
        };

        _mockApiClient.Setup(x => x.AddColorToPaletteAsync(paletteId,
                It.Is<CreatePaletteColorRequest>(req => req.R == r && req.G == g && req.B == b && req.A == a)))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _paletteService.AddColorToPaletteAsync(paletteId, r, g, b, a);

        // Assert
        result.ShouldBeTrue();
        _mockApiClient.Verify(
            x => x.AddColorToPaletteAsync(paletteId,
                It.Is<CreatePaletteColorRequest>(req => req.R == r && req.G == g && req.B == b && req.A == a)),
            Times.Once);
    }

    [Fact]
    public async Task AddColorToPaletteAsync_ExceptionThrown_ReturnsFalse()
    {
        // Arrange
        _mockApiClient.Setup(x => x.AddColorToPaletteAsync(It.IsAny<long>(), It.IsAny<CreatePaletteColorRequest>()))
            .ThrowsAsync(new Exception("API Error"));

        // Act
        var result = await _paletteService.AddColorToPaletteAsync(1L, 255, 0, 0, 1.0m);

        // Assert
        result.ShouldBeFalse();
    }
}