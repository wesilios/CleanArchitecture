using Microsoft.Extensions.Logging;

namespace External.Client.ApiConsumer.Services;

public class ConsoleApplication
{
    private readonly IUserInterface _userInterface;
    private readonly IPaletteService _paletteService;
    private readonly ILogger<ConsoleApplication> _logger;

    public ConsoleApplication(
        IUserInterface userInterface,
        IPaletteService paletteService,
        ILogger<ConsoleApplication> logger)
    {
        _userInterface = userInterface;
        _paletteService = paletteService;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Console application started");

        _userInterface.DisplayWelcome();

        var running = true;
        while (running)
        {
            try
            {
                var choice = _userInterface.DisplayMainMenu();

                switch (choice)
                {
                    case "1":
                        await HandleListPalettes();
                        break;
                    case "2":
                        await HandleCreatePalette();
                        break;
                    case "3":
                        await HandleViewPalette();
                        break;
                    case "4":
                        await HandleUpdatePalette();
                        break;
                    case "5":
                        await HandleDeletePalette();
                        break;
                    case "6":
                        await HandleAddColorToPalette();
                        break;
                    case "0":
                        running = false;
                        _userInterface.DisplayMessage("Goodbye!");
                        break;
                    default:
                        _userInterface.DisplayError("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing user input");
                _userInterface.DisplayError($"An error occurred: {ex.Message}");
            }
        }
    }

    private async Task HandleListPalettes()
    {
        _logger.LogInformation("Listing all palettes");

        var pageNumber = _userInterface.GetPageNumber();
        var pageSize = _userInterface.GetPageSize();
        var searchTerm = _userInterface.GetSearchTerm();

        var palettes = await _paletteService.GetPalettesAsync(pageNumber, pageSize, searchTerm);
        _userInterface.DisplayPalettes(palettes);
    }

    private async Task HandleCreatePalette()
    {
        _logger.LogInformation("Creating new palette");

        var name = _userInterface.GetPaletteName();
        if (string.IsNullOrWhiteSpace(name))
        {
            _userInterface.DisplayError("Palette name cannot be empty.");
            return;
        }

        var success = await _paletteService.CreatePaletteAsync(name);
        if (success)
        {
            _userInterface.DisplaySuccess($"Palette '{name}' created successfully!");
        }
        else
        {
            _userInterface.DisplayError("Failed to create palette.");
        }
    }

    private async Task HandleViewPalette()
    {
        _logger.LogInformation("Viewing palette details");

        var paletteId = _userInterface.GetPaletteId();
        if (paletteId <= 0)
        {
            _userInterface.DisplayError("Invalid palette ID.");
            return;
        }

        var palette = await _paletteService.GetPaletteByIdAsync(paletteId);
        if (palette != null)
        {
            _userInterface.DisplayPaletteDetails(palette);
        }
        else
        {
            _userInterface.DisplayError("Palette not found.");
        }
    }

    private async Task HandleUpdatePalette()
    {
        _logger.LogInformation("Updating palette");

        var paletteId = _userInterface.GetPaletteId();
        if (paletteId <= 0)
        {
            _userInterface.DisplayError("Invalid palette ID.");
            return;
        }

        var newName = _userInterface.GetPaletteName();
        if (string.IsNullOrWhiteSpace(newName))
        {
            _userInterface.DisplayError("Palette name cannot be empty.");
            return;
        }

        var success = await _paletteService.UpdatePaletteAsync(paletteId, newName);
        if (success)
        {
            _userInterface.DisplaySuccess($"Palette updated successfully!");
        }
        else
        {
            _userInterface.DisplayError("Failed to update palette.");
        }
    }

    private async Task HandleDeletePalette()
    {
        _logger.LogInformation("Deleting palette");

        var paletteId = _userInterface.GetPaletteId();
        if (paletteId <= 0)
        {
            _userInterface.DisplayError("Invalid palette ID.");
            return;
        }

        var confirmed = _userInterface.ConfirmAction($"Are you sure you want to delete palette with ID {paletteId}?");
        if (!confirmed)
        {
            _userInterface.DisplayMessage("Operation cancelled.");
            return;
        }

        var success = await _paletteService.DeletePaletteAsync(paletteId);
        if (success)
        {
            _userInterface.DisplaySuccess("Palette deleted successfully!");
        }
        else
        {
            _userInterface.DisplayError("Failed to delete palette.");
        }
    }

    private async Task HandleAddColorToPalette()
    {
        _logger.LogInformation("Adding color to palette");

        var paletteId = _userInterface.GetPaletteId();
        if (paletteId <= 0)
        {
            _userInterface.DisplayError("Invalid palette ID.");
            return;
        }

        var colorData = _userInterface.GetColorData();
        if (colorData == null)
        {
            _userInterface.DisplayError("Invalid color data.");
            return;
        }

        var success =
            await _paletteService.AddColorToPaletteAsync(paletteId, colorData.R, colorData.G, colorData.B, colorData.A);
        if (success)
        {
            _userInterface.DisplaySuccess("Color added to palette successfully!");
        }
        else
        {
            _userInterface.DisplayError("Failed to add color to palette.");
        }
    }
}