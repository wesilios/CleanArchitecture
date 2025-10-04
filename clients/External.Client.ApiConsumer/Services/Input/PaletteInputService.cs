using External.Client.ApiConsumer.Models;
using Spectre.Console;

namespace External.Client.ApiConsumer.Services.Input;

/// <summary>
/// Service responsible for collecting palette-related user input
/// </summary>
public class PaletteInputService : IPaletteInputService
{
    public bool ConfirmAction(string message)
    {
        return AnsiConsole.Confirm($"[yellow]WARNING: {message.EscapeMarkup()}[/]");
    }

    public void WaitForKeyPress()
    {
        AnsiConsole.MarkupLine("\n[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    public int GetPageNumber()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Enter page number:[/]")
                .DefaultValue(1)
                .ValidationErrorMessage("[red]Please enter a valid page number (1 or greater)[/]")
                .Validate(page => page >= 1 ? ValidationResult.Success() : ValidationResult.Error()));
    }

    public int GetPageSize()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan]Enter page size (items per page):[/]")
                .DefaultValue(10)
                .ValidationErrorMessage("[red]Please enter a valid page size (1-50)[/]")
                .Validate(size => size is >= 1 and <= 50 ? ValidationResult.Success() : ValidationResult.Error()));
    }

    public string? GetSearchTerm()
    {
        var searchTerm = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Enter search term (or press Enter to skip):[/]")
                .DefaultValue("")
                .AllowEmpty());

        return string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm;
    }

    public string GetPaletteName()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan]Enter palette name:[/]")
                .ValidationErrorMessage("[red]Palette name cannot be empty[/]")
                .Validate(name =>
                    !string.IsNullOrWhiteSpace(name) ? ValidationResult.Success() : ValidationResult.Error()));
    }

    public long GetPaletteId()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<long>("[cyan]Enter palette ID:[/]")
                .ValidationErrorMessage("[red]Please enter a valid palette ID (positive number)[/]")
                .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error()));
    }

    public CreatePaletteColorRequest? GetColorData()
    {
        AnsiConsole.Write(new Rule("[bold yellow]COLOR INPUT WIZARD[/]")
        {
            Style = Style.Parse("yellow")
        });

        AnsiConsole.MarkupLine("[dim]Enter RGB values (0-255) and Alpha transparency (0.0-1.0)[/]\n");

        try
        {
            // Get Red component with preview
            var r = AnsiConsole.Prompt(
                new TextPrompt<int>("[red]Red component (0-255):[/]")
                    .ValidationErrorMessage("[red]Please enter a value between 0 and 255[/]")
                    .Validate(value =>
                        value is >= 0 and <= 255 ? ValidationResult.Success() : ValidationResult.Error()));

            AnsiConsole.MarkupLine($"Red preview: [on rgb({r},0,0)]     [/] RGB({r}, 0, 0)\n");

            // Get Green component with preview
            var g = AnsiConsole.Prompt(
                new TextPrompt<int>("[green]Green component (0-255):[/]")
                    .ValidationErrorMessage("[red]Please enter a value between 0 and 255[/]")
                    .Validate(value =>
                        value is >= 0 and <= 255 ? ValidationResult.Success() : ValidationResult.Error()));

            AnsiConsole.MarkupLine($"Red+Green preview: [on rgb({r},{g},0)]     [/] RGB({r}, {g}, 0)\n");

            // Get Blue component with preview
            var b = AnsiConsole.Prompt(
                new TextPrompt<int>("[blue]Blue component (0-255):[/]")
                    .ValidationErrorMessage("[red]Please enter a value between 0 and 255[/]")
                    .Validate(value =>
                        value is >= 0 and <= 255 ? ValidationResult.Success() : ValidationResult.Error()));

            AnsiConsole.MarkupLine($"Full RGB preview: [on rgb({r},{g},{b})]     [/] RGB({r}, {g}, {b})\n");

            // Get Alpha component
            var a = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[yellow]Alpha transparency (0.0-1.0) [1.0]:[/]")
                    .DefaultValue(1.0m)
                    .ValidationErrorMessage("[red]Please enter a value between 0.0 and 1.0[/]")
                    .Validate(value =>
                        value is >= 0.0m and <= 1.0m ? ValidationResult.Success() : ValidationResult.Error()));

            // Calculate brightness and color type for analysis
            var brightness = (0.299 * r + 0.587 * g + 0.114 * b) / 255.0;
            var brightnessText = brightness switch
            {
                >= 0.8 => "[white]Bright[/]",
                >= 0.5 => "[yellow]Medium[/]",
                >= 0.2 => "[orange3]Dark[/]",
                _ => "[red]Very Dark[/]"
            };

            var colorType = (r, g, b) switch
            {
                var (red, green, blue) when red > green && red > blue => "[red]Red-dominant[/]",
                var (red, green, blue) when green > red && green > blue => "[green]Green-dominant[/]",
                var (red, green, blue) when blue > red && blue > green => "[blue]Blue-dominant[/]",
                var (red, green, blue) when red == green && green == blue => "[grey]Grayscale[/]",
                _ => "[yellow]Mixed[/]"
            };

            var hexValue = $"#{r:X2}{g:X2}{b:X2}";

            // Display final color preview and analysis
            var previewPanel = new Panel(
                $"[bold]FINAL COLOR[/]\n\n" +
                $"[on rgb({r},{g},{b})]                    [/]\n\n" +
                $"[bold]RGB Values:[/] ({r}, {g}, {b})\n" +
                $"[bold]Hex Code:[/] {hexValue}\n" +
                $"[bold]Alpha:[/] {a:F2} ({a * 100:F0}% opacity)\n" +
                $"[bold]Brightness:[/] {brightnessText} ({brightness:P0})\n" +
                $"[bold]Color Type:[/] {colorType}")
            {
                Header = new PanelHeader("[bold green]COLOR PREVIEW & ANALYSIS[/]"),
                Border = BoxBorder.Double,
                BorderStyle = new Style(Color.Green),
                Padding = new Padding(2, 1, 2, 1)
            };

            AnsiConsole.Write(previewPanel);

            return new CreatePaletteColorRequest { R = r, G = g, B = b, A = a };
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Please try again.[/]");
            return null;
        }
    }
}