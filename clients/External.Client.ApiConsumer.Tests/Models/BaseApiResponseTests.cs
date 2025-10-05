using External.Client.ApiConsumer.Models;
using FluentAssertions;

namespace External.Client.ApiConsumer.Tests.Models;

public class BaseApiResponseTests
{
    [Fact]
    public void BaseApiResponse_DefaultConstructor_InitializesWithDefaults()
    {
        // Act
        var response = new BaseApiResponse<string>();

        // Assert
        response.Data.Should().BeNull();
        response.Message.Should().Be(string.Empty);
        response.StatusCode.Should().Be(0);
    }

    [Fact]
    public void BaseApiResponse_WithData_InitializesCorrectly()
    {
        // Arrange
        var testData = "Test Data";
        var testMessage = "Success";
        var testStatusCode = 200;

        // Act
        var response = new BaseApiResponse<string>
        {
            Data = testData,
            Message = testMessage,
            StatusCode = testStatusCode
        };

        // Assert
        response.Data.Should().Be(testData);
        response.Message.Should().Be(testMessage);
        response.StatusCode.Should().Be(testStatusCode);
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(204)]
    [InlineData(299)]
    public void EnsureSuccess_WithSuccessStatusCodes_ReturnsData(int statusCode)
    {
        // Arrange
        var testData = "Success Data";
        var response = new BaseApiResponse<string>
        {
            Data = testData,
            StatusCode = statusCode,
            Message = "Success"
        };

        // Act
        var result = response.EnsureSuccess();

        // Assert
        result.Should().Be(testData);
    }

    [Theory]
    [InlineData(400, "Bad Request")]
    [InlineData(401, "Unauthorized")]
    [InlineData(404, "Not Found")]
    [InlineData(500, "Internal Server Error")]
    public void EnsureSuccess_WithErrorStatusCodes_ThrowsApiResponseException(int statusCode, string message)
    {
        // Arrange
        var response = new BaseApiResponse<string>
        {
            Data = "Some Data",
            StatusCode = statusCode,
            Message = message
        };

        // Act & Assert
        response.Invoking(r => r.EnsureSuccess())
            .Should().Throw<ApiResponseException>()
            .Which.StatusCode.Should().Be(statusCode);
    }

    [Fact]
    public void EnsureSuccess_WithErrorStatusCode_ThrowsExceptionWithCorrectMessage()
    {
        // Arrange
        var errorMessage = "Custom error message";
        var response = new BaseApiResponse<string>
        {
            Data = "Some Data",
            StatusCode = 400,
            Message = errorMessage
        };

        // Act & Assert
        response.Invoking(r => r.EnsureSuccess())
            .Should().Throw<ApiResponseException>()
            .Which.Message.Should().Be(errorMessage);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(199)]
    [InlineData(300)]
    [InlineData(399)]
    public void EnsureSuccess_WithNonSuccessStatusCodes_ThrowsException(int statusCode)
    {
        // Arrange
        var response = new BaseApiResponse<string>
        {
            Data = "Some Data",
            StatusCode = statusCode,
            Message = "Error"
        };

        // Act & Assert
        response.Invoking(r => r.EnsureSuccess())
            .Should().Throw<ApiResponseException>();
    }

    [Fact]
    public void EnsureSuccess_WithNullData_ReturnsNull()
    {
        // Arrange
        var response = new BaseApiResponse<string?>
        {
            Data = null,
            StatusCode = 200,
            Message = "Success"
        };

        // Act
        var result = response.EnsureSuccess();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void EnsureSuccess_WithComplexObject_ReturnsCorrectData()
    {
        // Arrange
        var complexData = new PaletteResponse
        {
            PaletteId = 1,
            Name = "Test Palette",
            Colors = new List<ColorResponse>
            {
                new() { R = 255, G = 0, B = 0, A = 1.0m, Hex = "#FF0000" }
            }
        };

        var response = new BaseApiResponse<PaletteResponse>
        {
            Data = complexData,
            StatusCode = 200,
            Message = "Success"
        };

        // Act
        var result = response.EnsureSuccess();

        // Assert
        result.Should().BeEquivalentTo(complexData);
    }
}

public class ApiResponseExceptionTests
{
    [Fact]
    public void ApiResponseException_WithStatusCodeAndMessage_InitializesCorrectly()
    {
        // Arrange
        var statusCode = 404;
        var message = "Not Found";

        // Act
        var exception = new ApiResponseException(statusCode, message);

        // Assert
        exception.StatusCode.Should().Be(statusCode);
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void ApiResponseException_WithOnlyStatusCode_InitializesWithDefaultMessage()
    {
        // Arrange
        var statusCode = 500;

        // Act
        var exception = new ApiResponseException(statusCode, string.Empty);

        // Assert
        exception.StatusCode.Should().Be(statusCode);
        exception.Message.Should().Be(string.Empty);
    }

    [Fact]
    public void ApiResponseException_InheritsFromException()
    {
        // Arrange & Act
        var exception = new ApiResponseException(400, "Bad Request");

        // Assert
        exception.Should().BeAssignableTo<Exception>();
    }
}