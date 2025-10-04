using Spectre.Console;

namespace External.Client.ApiConsumer.Services.Display;

/// <summary>
/// Service responsible for displaying welcome screen and basic messages
/// </summary>
public class WelcomeDisplayService : IDisplayService
{
    public void DisplayWelcome()
    {
        AnsiConsole.Clear();

        var panel = new Panel(
            new FigletText("Palette Manager")
                .Centered()
                .Color(Color.Cyan1))
        {
            Header = new PanelHeader("[bold yellow]Clean Architecture Console Client[/]"),
            Border = BoxBorder.Double,
            BorderStyle = new Style(Color.Cyan1),
            Padding = new Padding(2, 1, 2, 1)
        };

        AnsiConsole.Write(panel);
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
}