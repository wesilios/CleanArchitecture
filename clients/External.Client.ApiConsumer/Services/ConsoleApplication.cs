using External.Client.ApiConsumer.Services.Display;
using External.Client.ApiConsumer.Services.Handlers;
using Microsoft.Extensions.Logging;

namespace External.Client.ApiConsumer.Services;

/// <summary>
/// Clean architecture console application with separated concerns
/// </summary>
public class ConsoleApplication
{
    private readonly IMenuDisplayService _menuDisplayService;
    private readonly WelcomeDisplayService _welcomeDisplayService;
    private readonly IPaletteCommandHandler _paletteCommandHandler;
    private readonly ILogger<ConsoleApplication> _logger;

    public ConsoleApplication(
        IMenuDisplayService menuDisplayService,
        WelcomeDisplayService welcomeDisplayService,
        IPaletteCommandHandler paletteCommandHandler,
        ILogger<ConsoleApplication> logger)
    {
        _menuDisplayService = menuDisplayService;
        _welcomeDisplayService = welcomeDisplayService;
        _paletteCommandHandler = paletteCommandHandler;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Clean console application started");

        _welcomeDisplayService.DisplayWelcome();

        var running = true;
        while (running)
        {
            try
            {
                var choice = _menuDisplayService.DisplayMainMenu();

                switch (choice)
                {
                    case "0":
                        running = false;
                        _welcomeDisplayService.DisplayMessage("Goodbye!");
                        break;
                    default:
                        if (!await _paletteCommandHandler.CanHandleAsync(choice))
                        {
                            _welcomeDisplayService.DisplayError($"Unknown option: {choice}");
                            break;
                        }

                        await _paletteCommandHandler.ExecuteAsync(choice);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing user input");
                _welcomeDisplayService.DisplayError("An unexpected error occurred. Please try again.");
            }
        }

        _logger.LogInformation("Console application ended");
    }
}