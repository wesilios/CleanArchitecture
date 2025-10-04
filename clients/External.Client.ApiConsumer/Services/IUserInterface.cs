using External.Client.ApiConsumer.Models;

namespace External.Client.ApiConsumer.Services;

public interface IUserInterface
{
    void DisplayWelcome();
    string DisplayMainMenu();
    void DisplayMessage(string message);
    void DisplayError(string message);
    void DisplaySuccess(string message);
    void DisplayPalettes(PalettePaginationResponse? palettes);
    void DisplayPaletteDetails(PaletteResponse palette);

    int GetPageNumber();
    int GetPageSize();
    string? GetSearchTerm();
    string GetPaletteName();
    long GetPaletteId();
    ColorData? GetColorData();
    bool ConfirmAction(string message);
    void WaitForKeyPress();
}

public class ColorData
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; } = 1.0m;
}