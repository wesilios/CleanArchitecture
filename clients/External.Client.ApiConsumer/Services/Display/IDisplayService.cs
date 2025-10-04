using External.Client.ApiConsumer.Models;

namespace External.Client.ApiConsumer.Services.Display;

/// <summary>
/// Interface for display services that handle UI presentation
/// </summary>
public interface IDisplayService
{
    void DisplayWelcome();
    void DisplayMessage(string message);
    void DisplayError(string message);
    void DisplaySuccess(string message);
}

/// <summary>
/// Interface for palette-specific display operations
/// </summary>
public interface IPaletteDisplayService : IDisplayService
{
    void DisplayPalettes(PalettePaginationResponse? palettes);
    void DisplayPaletteDetails(PaletteResponse palette);
    void DisplayPaletteCapacity(int currentColors, int maxColors, string paletteName);
}

/// <summary>
/// Interface for menu display operations
/// </summary>
public interface IMenuDisplayService : IDisplayService
{
    string DisplayMainMenu();
}