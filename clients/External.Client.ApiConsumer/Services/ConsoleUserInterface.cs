using External.Client.ApiConsumer.Models;
using Spectre.Console;

namespace External.Client.ApiConsumer.Services;

public class ConsoleUserInterface : IUserInterface
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

    public string DisplayMainMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold cyan]What would you like to do?[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(new[]
                {
                    "List Palettes",
                    "Create Palette",
                    "View Palette Details",
                    "Update Palette",
                    "Delete Palette",
                    "Add Color to Palette",
                    "Exit"
                }));

        return choice switch
        {
            "List Palettes" => "1",
            "Create Palette" => "2",
            "View Palette Details" => "3",
            "Update Palette" => "4",
            "Delete Palette" => "5",
            "Add Color to Palette" => "6",
            "Exit" => "0",
            _ => ""
        };
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

    public void DisplayPalettes(PalettePaginationResponse? palettes)
    {
        if (palettes == null || !palettes.Items.Any())
        {
            var emptyPanel =
                new Panel("[dim]No palettes found. Try creating a new palette or adjusting your search criteria.[/]")
                {
                    Header = new PanelHeader("[yellow]SEARCH RESULTS[/]"),
                    Border = BoxBorder.Rounded,
                    BorderStyle = new Style(Color.Yellow),
                    Padding = new Padding(2, 1, 2, 1)
                };
            AnsiConsole.Write(emptyPanel);
            return;
        }

        // Header with search info
        var headerText = palettes.TotalCount == 1
            ? $"Found {palettes.TotalCount} palette"
            : $"Found {palettes.TotalCount} palettes";

        var headerPanel = new Panel($"[bold cyan]{headerText}[/]")
        {
            Header = new PanelHeader("[bold yellow]PALETTE COLLECTION[/]"),
            Border = BoxBorder.Double,
            BorderStyle = new Style(Color.Cyan1),
            Padding = new Padding(1, 0, 1, 0)
        };
        AnsiConsole.Write(headerPanel);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .ShowHeaders();

        table.AddColumn(new TableColumn("[bold]ID[/]").Centered().Width(6));
        table.AddColumn(new TableColumn("[bold]Palette Name[/]").LeftAligned().Width(25));
        table.AddColumn(new TableColumn("[bold]Created Date[/]").Centered().Width(12));
        table.AddColumn(new TableColumn("[bold]Status[/]").Centered().Width(12));
        table.AddColumn(new TableColumn("[bold]Color Preview[/]").LeftAligned().Width(30));

        foreach (var palette in palettes.Items)
        {
            var colorCount = palette.Colors.Count();
            var maxColors = 5;
            var statusText = colorCount == maxColors
                ? $"[red bold]{colorCount}/{maxColors} FULL[/]"
                : $"[green]{colorCount}/{maxColors}[/]";

            var colorPreview = "";
            var displayedColors = palette.Colors.Take(maxColors).ToList();

            foreach (var color in displayedColors)
            {
                colorPreview += $"[on rgb({color.R},{color.G},{color.B})] ■ [/]";
            }

            // Add empty slots if less than max colors
            for (int i = colorCount; i < maxColors; i++)
            {
                colorPreview += "[dim]□[/]";
            }

            if (colorCount == 0)
            {
                colorPreview = "[dim]No colors added[/]";
            }

            // Format creation date with relative time
            var createdDate = palette.CreatedTime;
            var daysDiff = (DateTime.Now - createdDate).Days;
            var dateDisplay = daysDiff switch
            {
                0 => "[green]Today[/]",
                1 => "[yellow]Yesterday[/]",
                <= 7 => $"[cyan]{daysDiff}d ago[/]",
                <= 30 => $"[dim]{daysDiff}d ago[/]",
                _ => $"[dim]{createdDate:MM/dd/yy}[/]"
            };

            table.AddRow(
                $"[cyan bold]#{palette.PaletteId}[/]",
                $"[white]{palette.Name.EscapeMarkup()}[/]",
                dateDisplay,
                statusText,
                colorPreview
            );
        }

        AnsiConsole.Write(table);

        // Enhanced pagination info
        var paginationText = palettes.TotalPages > 1
            ? $"[bold]Page {palettes.PageNumber} of {palettes.TotalPages}[/] | [dim]Showing {palettes.Items.Count()} of {palettes.TotalCount} total palettes[/]"
            : $"[dim]Showing all {palettes.TotalCount} palettes[/]";

        var paginationPanel = new Panel(paginationText)
        {
            Border = BoxBorder.None,
            Padding = new Padding(0, 1, 0, 0)
        };

        AnsiConsole.Write(paginationPanel);

        // Navigation hints
        if (palettes.TotalPages > 1)
        {
            var hints = new List<string>();
            if (palettes.HasPreviousPage) hints.Add("[dim]Previous pages available[/]");
            if (palettes.HasNextPage) hints.Add("[dim]More pages available[/]");

            if (hints.Any())
            {
                AnsiConsole.MarkupLine($"[dim italic]{string.Join(" | ", hints)}[/]");
            }
        }
    }

    public void DisplayPaletteDetails(PaletteResponse palette)
    {
        var colorCount = palette.Colors.Count();
        var maxColors = 5;
        var statusText = colorCount == maxColors ? "[red bold]FULL[/]" : "[green]AVAILABLE[/]";
        var createdDate = palette.CreatedTime.ToString("yyyy-MM-dd");

        // Main header with enhanced styling
        var rule = new Rule($"[bold yellow]PALETTE DETAILS[/]")
        {
            Style = Style.Parse("yellow")
        };
        AnsiConsole.Write(rule);

        // Palette info panel
        var infoGrid = new Grid()
            .AddColumn(new GridColumn().NoWrap().PadRight(4))
            .AddColumn(new GridColumn().NoWrap().PadRight(4))
            .AddColumn(new GridColumn().NoWrap());

        infoGrid.AddRow(
            $"[bold cyan]Name:[/] [white]{palette.Name.EscapeMarkup()}[/]",
            $"[bold cyan]ID:[/] [yellow]#{palette.PaletteId}[/]",
            $"[bold cyan]Created:[/] [dim]{createdDate}[/]"
        );

        infoGrid.AddRow(
            $"[bold cyan]Colors:[/] {colorCount}/{maxColors}",
            $"[bold cyan]Status:[/] {statusText}",
            $"[bold cyan]Capacity:[/] {(maxColors - colorCount)} slots remaining"
        );

        var infoPanel = new Panel(infoGrid)
        {
            Header = new PanelHeader($"[bold]'{palette.Name.EscapeMarkup()}'[/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Cyan1),
            Padding = new Padding(2, 1, 2, 1)
        };

        AnsiConsole.Write(infoPanel);

        if (!palette.Colors.Any())
        {
            var emptyPanel =
                new Panel("[dim]This palette has no colors yet. Use 'Add Color to Palette' to add colors.[/]")
                {
                    Header = new PanelHeader("[yellow]EMPTY PALETTE[/]"),
                    Border = BoxBorder.Rounded,
                    BorderStyle = new Style(Color.Yellow),
                    Padding = new Padding(2, 1, 2, 1)
                };
            AnsiConsole.Write(emptyPanel);
            return;
        }

        // Enhanced colors table
        var table = new Table()
            .Border(TableBorder.Heavy)
            .BorderColor(Color.Grey)
            .Title("[bold underline]COLOR COMPOSITION[/]")
            .Caption("[dim]All color values and visual representation[/]");

        table.AddColumn(new TableColumn("[bold]#[/]").Centered().Width(3));
        table.AddColumn(new TableColumn("[bold]Visual[/]").Centered().Width(8));
        table.AddColumn(new TableColumn("[bold]RGB Values[/]").LeftAligned().Width(20));
        table.AddColumn(new TableColumn("[bold]Alpha[/]").Centered().Width(8));
        table.AddColumn(new TableColumn("[bold]Hex Code[/]").Centered().Width(10));
        table.AddColumn(new TableColumn("[bold]Brightness[/]").Centered().Width(10));

        var order = 1;
        foreach (var color in palette.Colors)
        {
            var rgbText = $"({color.R}, {color.G}, {color.B})";
            var alphaText = $"{color.A:F2}";
            var colorBlock = $"[on rgb({color.R},{color.G},{color.B})] ████ [/]";
            var hexValue = !string.IsNullOrEmpty(color.Hex) ? color.Hex : $"#{color.R:X2}{color.G:X2}{color.B:X2}";

            // Calculate brightness (perceived luminance)
            var brightness = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
            var brightnessText = brightness switch
            {
                >= 0.8 => "[white]Bright[/]",
                >= 0.5 => "[yellow]Medium[/]",
                >= 0.2 => "[orange3]Dark[/]",
                _ => "[red]Very Dark[/]"
            };

            table.AddRow(
                $"[cyan bold]{order}[/]",
                colorBlock,
                $"[white]{rgbText}[/]",
                $"[dim]{alphaText}[/]",
                $"[grey]{hexValue}[/]",
                brightnessText
            );

            order++;
        }

        AnsiConsole.Write(table);

        // Color harmony analysis
        if (colorCount >= 2)
        {
            var harmonyPanel = new Panel(AnalyzeColorHarmony(palette.Colors))
            {
                Header = new PanelHeader("[bold]COLOR HARMONY ANALYSIS[/]"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Green),
                Padding = new Padding(1, 0, 1, 0)
            };
            AnsiConsole.Write(harmonyPanel);
        }

        AnsiConsole.WriteLine();
    }

    private string AnalyzeColorHarmony(IEnumerable<ColorResponse> colors)
    {
        var colorList = colors.ToList();
        var analysis = new List<string>();

        // Analyze color temperature
        var warmColors = colorList.Count(c => c.R > c.B);
        var coolColors = colorList.Count(c => c.B > c.R);

        if (warmColors > coolColors)
            analysis.Add("[orange3]Warm palette[/] - Creates cozy, energetic feeling");
        else if (coolColors > warmColors)
            analysis.Add("[cyan]Cool palette[/] - Creates calm, professional feeling");
        else
            analysis.Add("[yellow]Balanced palette[/] - Mix of warm and cool tones");

        // Analyze brightness range
        var brightnesses = colorList.Select(c => (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255.0).ToList();
        var brightnessRange = brightnesses.Max() - brightnesses.Min();

        if (brightnessRange > 0.6)
            analysis.Add("[green]High contrast[/] - Good for accessibility and visual impact");
        else if (brightnessRange > 0.3)
            analysis.Add("[yellow]Medium contrast[/] - Balanced visual hierarchy");
        else
            analysis.Add("[dim]Low contrast[/] - Subtle, monochromatic feel");

        return string.Join("\n", analysis);
    }

    public int GetPageNumber()
    {
        AnsiConsole.MarkupLine("[bold]PAGINATION SETTINGS[/]");
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan bold]Page number to display:[/]")
                .DefaultValue(1)
                .ValidationErrorMessage("[red]Please enter a valid page number (1 or greater)[/]")
                .Validate(page =>
                    page >= 1
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Page number must be 1 or greater[/]")));
    }

    public int GetPageSize()
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>("[cyan bold]Items per page (1-100):[/]")
                .DefaultValue(10)
                .ValidationErrorMessage("[red]Please enter a valid page size (1-100)[/]")
                .Validate(size =>
                    size >= 1 && size <= 100
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Page size must be between 1 and 100[/]")));
    }

    public string? GetSearchTerm()
    {
        AnsiConsole.MarkupLine("[bold]SEARCH FILTER[/]");
        AnsiConsole.MarkupLine("[dim]Enter a search term to filter palettes by name, or press Enter to show all[/]");

        var searchTerm = AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan bold]Search term:[/]")
                .AllowEmpty());

        var result = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim();

        if (result != null)
        {
            AnsiConsole.MarkupLine($"[green]Searching for palettes containing: '{result.EscapeMarkup()}'[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[dim]No search filter applied - showing all palettes[/]");
        }

        return result;
    }

    public string GetPaletteName()
    {
        AnsiConsole.MarkupLine("[bold]PALETTE CREATION[/]");
        AnsiConsole.MarkupLine("[dim]Choose a descriptive name for your new palette (1-100 characters)[/]");

        return AnsiConsole.Prompt(
            new TextPrompt<string>("[cyan bold]Palette name:[/]")
                .ValidationErrorMessage("[red]Palette name cannot be empty and must be 1-100 characters[/]")
                .Validate(name =>
                {
                    if (string.IsNullOrWhiteSpace(name))
                        return ValidationResult.Error("[red]Please enter a palette name[/]");
                    if (name.Trim().Length > 100)
                        return ValidationResult.Error("[red]Palette name must be 100 characters or less[/]");
                    return ValidationResult.Success();
                }));
    }

    public long GetPaletteId()
    {
        AnsiConsole.MarkupLine("[bold]PALETTE SELECTION[/]");
        AnsiConsole.MarkupLine("[dim]Enter the ID number of the palette you want to work with[/]");

        return AnsiConsole.Prompt(
            new TextPrompt<long>("[cyan bold]Palette ID (number only):[/]")
                .ValidationErrorMessage("[red]Please enter a valid palette ID[/]")
                .Validate(id =>
                    id > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Palette ID must be a positive number[/]")));
    }

    public ColorData? GetColorData()
    {
        try
        {
            // Header for color input
            var rule = new Rule("[bold yellow]COLOR INPUT WIZARD[/]")
            {
                Style = Style.Parse("yellow")
            };
            AnsiConsole.Write(rule);

            AnsiConsole.MarkupLine("[dim]Enter RGB values (0-255) and Alpha transparency (0.0-1.0)[/]\n");

            var r = AnsiConsole.Prompt(
                new TextPrompt<int>("[red bold]Red component (0-255):[/]")
                    .ValidationErrorMessage("[red]Please enter a value between 0 and 255[/]")
                    .Validate(value =>
                        value >= 0 && value <= 255
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Value must be between 0 and 255[/]")));

            // Show red preview
            AnsiConsole.MarkupLine($"[dim]Red preview:[/] [on rgb({r},0,0)]    [/] [dim]RGB({r}, 0, 0)[/]\n");

            var g = AnsiConsole.Prompt(
                new TextPrompt<int>("[green bold]Green component (0-255):[/]")
                    .ValidationErrorMessage("[red]Please enter a value between 0 and 255[/]")
                    .Validate(value =>
                        value >= 0 && value <= 255
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Value must be between 0 and 255[/]")));

            // Show red+green preview
            AnsiConsole.MarkupLine($"[dim]Red+Green preview:[/] [on rgb({r},{g},0)]    [/] [dim]RGB({r}, {g}, 0)[/]\n");

            var b = AnsiConsole.Prompt(
                new TextPrompt<int>("[blue bold]Blue component (0-255):[/]")
                    .ValidationErrorMessage("[red]Please enter a value between 0 and 255[/]")
                    .Validate(value =>
                        value >= 0 && value <= 255
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Value must be between 0 and 255[/]")));

            // Show full RGB preview
            AnsiConsole.MarkupLine(
                $"[dim]Full RGB preview:[/] [on rgb({r},{g},{b})]    [/] [dim]RGB({r}, {g}, {b})[/]\n");

            var a = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[yellow bold]Alpha transparency (0.0-1.0):[/]")
                    .DefaultValue(1.0m)
                    .ValidationErrorMessage("[red]Please enter a value between 0.0 and 1.0[/]")
                    .Validate(value =>
                        value >= 0 && value <= 1
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Value must be between 0.0 and 1.0[/]")));

            // Final color preview with analysis
            var hexValue = $"#{r:X2}{g:X2}{b:X2}";
            var brightness = (0.299 * r + 0.587 * g + 0.114 * b) / 255.0;
            var brightnessText = brightness switch
            {
                >= 0.8 => "Very Bright",
                >= 0.6 => "Bright",
                >= 0.4 => "Medium",
                >= 0.2 => "Dark",
                _ => "Very Dark"
            };

            var colorType = (r, g, b) switch
            {
                var (red, green, blue) when red > green && red > blue => "Red-dominant",
                var (red, green, blue) when green > red && green > blue => "Green-dominant",
                var (red, green, blue) when blue > red && blue > green => "Blue-dominant",
                var (red, green, blue) when red == green && green == blue => "Grayscale",
                _ => "Mixed"
            };

            var previewPanel = new Panel(
                $"[on rgb({r},{g},{b})]     FINAL COLOR     [/]\n\n" +
                $"[bold]RGB Values:[/] ({r}, {g}, {b})\n" +
                $"[bold]Hex Code:[/] {hexValue}\n" +
                $"[bold]Alpha:[/] {a:F2} ({(a * 100):F0}% opacity)\n" +
                $"[bold]Brightness:[/] {brightnessText} ({brightness:P0})\n" +
                $"[bold]Color Type:[/] {colorType}")
            {
                Header = new PanelHeader("[bold green]COLOR PREVIEW & ANALYSIS[/]"),
                Border = BoxBorder.Double,
                BorderStyle = new Style(Color.Green),
                Padding = new Padding(2, 1, 2, 1)
            };

            AnsiConsole.Write(previewPanel);

            return new ColorData { R = r, G = g, B = b, A = a };
        }
        catch
        {
            DisplayError("Invalid color input. Please try again.");
            return null;
        }
    }

    public bool ConfirmAction(string message)
    {
        return AnsiConsole.Confirm($"[yellow]WARNING: {message.EscapeMarkup()}[/]");
    }

    public void WaitForKeyPress()
    {
        AnsiConsole.MarkupLine("\n[dim]Press any key to continue...[/]");
        System.Console.ReadKey(true);
    }
}