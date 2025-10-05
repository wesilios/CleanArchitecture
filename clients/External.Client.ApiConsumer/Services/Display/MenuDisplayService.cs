using Spectre.Console;

namespace External.Client.ApiConsumer.Services.Display;

/// <summary>
/// Service responsible for displaying menus and handling menu interactions
/// </summary>
public class MenuDisplayService : IMenuDisplayService
{
    public void DisplayWelcome()
    {
        // Delegate to WelcomeDisplayService if needed, or implement basic version
        AnsiConsole.WriteLine();
    }

    public void DisplayMessage(string message)
    {
        AnsiConsole.MarkupLine($"\n[blue]INFO: {message.EscapeMarkup()}[/]");
    }

    public void DisplayError(string message)
    {
        AnsiConsole.MarkupLine($"\n[red]ERROR: {message.EscapeMarkup()}[/]");
    }

    public void DisplaySuccess(string message)
    {
        AnsiConsole.MarkupLine($"\n[green]SUCCESS: {message.EscapeMarkup()}[/]");
    }

    public string DisplayMainMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold cyan]What would you like to do?[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(
                    "List Palettes",
                    "Create Palette",
                    "View Palette Details",
                    "Update Palette",
                    "Delete Palette",
                    "Add Color to Palette",
                    "Exit")
        );

        return choice switch
        {
            "List Palettes" => "1",
            "Create Palette" => "2",
            "View Palette Details" => "3",
            "Update Palette" => "4",
            "Delete Palette" => "5",
            "Add Color to Palette" => "6",
            "Exit" => "0",
            _ => "0"
        };
    }
}