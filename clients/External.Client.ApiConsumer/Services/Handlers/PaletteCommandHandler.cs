using External.Client.ApiConsumer.Services.Display;
using External.Client.ApiConsumer.Services.Input;
using Microsoft.Extensions.Logging;

namespace External.Client.ApiConsumer.Services.Handlers;

/// <summary>
/// Command handler for palette-related operations
/// </summary>
public class PaletteCommandHandler : IPaletteCommandHandler
{
    private readonly IPaletteDisplayService _displayService;
    private readonly IPaletteInputService _inputService;
    private readonly IPaletteService _paletteService;
    private readonly ILogger<PaletteCommandHandler> _logger;

    public PaletteCommandHandler(
        IPaletteDisplayService displayService,
        IPaletteInputService inputService,
        IPaletteService paletteService,
        ILogger<PaletteCommandHandler> logger)
    {
        _displayService = displayService;
        _inputService = inputService;
        _paletteService = paletteService;
        _logger = logger;
    }

    public Task<bool> CanHandleAsync(string command)
    {
        return Task.FromResult(command is "1" or "2" or "3" or "4" or "5" or "6");
    }

    public async Task ExecuteAsync(string command)
    {
        switch (command)
        {
            case "1":
                await HandleListPalettesAsync();
                break;
            case "2":
                await HandleCreatePaletteAsync();
                break;
            case "3":
                await HandleViewPaletteAsync();
                break;
            case "4":
                await HandleUpdatePaletteAsync();
                break;
            case "5":
                await HandleDeletePaletteAsync();
                break;
            case "6":
                await HandleAddColorToPaletteAsync();
                break;
            default:
                _displayService.DisplayError($"Unknown command: {command}");
                break;
        }
    }

    public async Task HandleListPalettesAsync()
    {
        _logger.LogInformation("Listing palettes");

        var pageNumber = _inputService.GetPageNumber();
        var pageSize = _inputService.GetPageSize();
        var searchTerm = _inputService.GetSearchTerm();

        var palettes = await _paletteService.GetPalettesAsync(pageNumber, pageSize, searchTerm);
        _displayService.DisplayPalettes(palettes);

        _inputService.WaitForKeyPress();
    }

    public async Task HandleCreatePaletteAsync()
    {
        _logger.LogInformation("Creating new palette");

        var paletteName = _inputService.GetPaletteName();
        var success = await _paletteService.CreatePaletteAsync(paletteName);

        if (!success)
        {
            _displayService.DisplayError("Failed to create palette.");
            _inputService.WaitForKeyPress();
            return;
        }

        _displayService.DisplaySuccess("Palette created successfully!");
        _inputService.WaitForKeyPress();
    }

    public async Task HandleViewPaletteAsync()
    {
        _logger.LogInformation("Viewing palette details");

        var paletteId = _inputService.GetPaletteId();
        var palette = await _paletteService.GetPaletteByIdAsync(paletteId);

        if (palette == null)
        {
            _displayService.DisplayError("Palette not found.");
            _inputService.WaitForKeyPress();
            return;
        }

        _displayService.DisplayPaletteDetails(palette);
        _inputService.WaitForKeyPress();
    }

    public async Task HandleUpdatePaletteAsync()
    {
        _logger.LogInformation("Updating palette");

        var paletteId = _inputService.GetPaletteId();
        var newName = _inputService.GetPaletteName();
        var success = await _paletteService.UpdatePaletteAsync(paletteId, newName);

        if (!success)
        {
            _displayService.DisplayError("Failed to update palette.");
            _inputService.WaitForKeyPress();
            return;
        }

        _displayService.DisplaySuccess("Palette updated successfully!");
        _inputService.WaitForKeyPress();
    }

    public async Task HandleDeletePaletteAsync()
    {
        _logger.LogInformation("Deleting palette");

        var paletteId = _inputService.GetPaletteId();
        var confirmed =
            _inputService.ConfirmAction("Are you sure you want to delete this palette? This action cannot be undone.");

        if (!confirmed)
        {
            _displayService.DisplayMessage("Delete operation cancelled.");
            _inputService.WaitForKeyPress();
            return;
        }

        var success = await _paletteService.DeletePaletteAsync(paletteId);
        if (!success)
        {
            _displayService.DisplayError("Failed to delete palette.");
            _inputService.WaitForKeyPress();
            return;
        }

        _displayService.DisplaySuccess("Palette deleted successfully!");
        _inputService.WaitForKeyPress();
    }

    public async Task HandleAddColorToPaletteAsync()
    {
        _logger.LogInformation("Adding color to palette");

        var paletteId = _inputService.GetPaletteId();
        if (paletteId <= 0)
        {
            _displayService.DisplayError("Invalid palette ID.");
            _inputService.WaitForKeyPress();
            return;
        }

        // Get initial palette information to show current status
        var palette = await _paletteService.GetPaletteByIdAsync(paletteId);
        if (palette == null)
        {
            _displayService.DisplayError("Palette not found.");
            _inputService.WaitForKeyPress();
            return;
        }

        _displayService.DisplayPaletteDetails(palette);

        var continueAdding = true;
        while (continueAdding && palette != null)
        {
            var currentColorCount = palette.Colors?.Count ?? 0;
            const int maxColors = 5;
            var remainingSlots = maxColors - currentColorCount;

            // Display current palette capacity status
            _displayService.DisplayPaletteCapacity(currentColorCount, maxColors, palette.Name ?? "Unknown Palette");

            if (remainingSlots <= 0)
            {
                _displayService.DisplayMessage("This palette is already full (5/5 colors). Cannot add more colors.");
                break;
            }

            var colorData = _inputService.GetColorData();
            if (colorData == null)
            {
                _displayService.DisplayError("Invalid color data.");
                continue;
            }

            var success =
                await _paletteService.AddColorToPaletteAsync(paletteId, colorData.R, colorData.G, colorData.B,
                    colorData.A);

            // Handle failed color addition
            if (!success)
            {
                _displayService.DisplayError("Failed to add color to palette.");
                continueAdding = _inputService.ConfirmAction("Would you like to try again?");
                continue;
            }

            // Color added successfully
            _displayService.DisplaySuccess("Color added to palette successfully!");

            // Get updated palette information
            palette = await _paletteService.GetPaletteByIdAsync(paletteId);
            if (palette == null)
            {
                _displayService.DisplayError("Could not retrieve updated palette information.");
                continueAdding = _inputService.ConfirmAction("Would you like to try adding another color?");
                continue;
            }

            // Display updated palette information
            var updatedColorCount = palette.Colors?.Count ?? 0;
            var updatedRemainingSlots = maxColors - updatedColorCount;

            _displayService.DisplayPaletteDetails(palette);
            _displayService.DisplayPaletteCapacity(updatedColorCount, maxColors, palette.Name ?? "Unknown Palette");

            // Check if palette is now full
            if (updatedRemainingSlots <= 0)
            {
                _displayService.DisplaySuccess("Palette is now full! All 5 color slots have been used.");
                continueAdding = false;
                continue;
            }

            // Ask user if they want to add another color
            continueAdding =
                _inputService.ConfirmAction(
                    $"Would you like to add another color? ({updatedRemainingSlots} slots remaining)");
        }

        _displayService.DisplayMessage("Finished adding colors to palette.");
        _inputService.WaitForKeyPress();
    }
}