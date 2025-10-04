namespace External.Client.ApiConsumer.Services.Handlers;

/// <summary>
/// Base interface for command handlers
/// </summary>
public interface ICommandHandler
{
    Task<bool> CanHandleAsync(string command);
    Task ExecuteAsync(string command);
}

/// <summary>
/// Interface for palette-specific command operations
/// </summary>
public interface IPaletteCommandHandler : ICommandHandler
{
    Task HandleListPalettesAsync();
    Task HandleCreatePaletteAsync();
    Task HandleViewPaletteAsync();
    Task HandleUpdatePaletteAsync();
    Task HandleDeletePaletteAsync();
    Task HandleAddColorToPaletteAsync();
}