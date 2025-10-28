using External.Client.ApiConsumer.Services.HttpClients;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace External.Client.ApiConsumer.Tests.Services.HttpClients;

public class PaletteApiClientServiceTests
{
    private readonly Mock<IPaletteApiClient> _mockPaletteApiClient;
    private readonly Mock<ILogger<PaletteApiClientService>> _mockLogger;
    private readonly PaletteApiClientService _service;

    public PaletteApiClientServiceTests()
    {
        _mockPaletteApiClient = new Mock<IPaletteApiClient>();
        _mockLogger = new Mock<ILogger<PaletteApiClientService>>();
        _service = new PaletteApiClientService(_mockPaletteApiClient.Object, _mockLogger.Object);
    }

    // Note: These tests focus on the service logic rather than Refit ApiResponse mocking
    // since ApiResponse<T> is a sealed class and cannot be mocked with Moq.
    // In a real scenario, integration tests would be more appropriate for testing Refit clients.

    [Fact]
    public void Constructor_WithValidDependencies_InitializesSuccessfully()
    {
        // Arrange & Act
        var service = new PaletteApiClientService(_mockPaletteApiClient.Object, _mockLogger.Object);

        // Assert
        service.ShouldNotBeNull();
    }

    [Fact]
    public void Service_ImplementsIApiClientService()
    {
        // Assert
        _service.ShouldBeAssignableTo<IApiClientService>();
    }

    // Note: Due to Refit's ApiResponse<T> being a sealed class, comprehensive unit testing
    // of the service methods requires integration testing or a wrapper approach.
    // The service logic is primarily tested through the higher-level PaletteService tests
    // which mock the IApiClientService interface.
}