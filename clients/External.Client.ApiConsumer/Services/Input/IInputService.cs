using External.Client.ApiConsumer.Models;

namespace External.Client.ApiConsumer.Services.Input;

/// <summary>
/// Interface for input services that handle user input collection
/// </summary>
public interface IInputService
{
    bool ConfirmAction(string message);
    void WaitForKeyPress();
}

/// <summary>
/// Interface for palette-specific input operations
/// </summary>
public interface IPaletteInputService : IInputService
{
    int GetPageNumber();
    int GetPageSize();
    string? GetSearchTerm();
    string GetPaletteName();
    long GetPaletteId();
    CreatePaletteColorRequest? GetColorData();
}