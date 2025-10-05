using External.Client.ApiConsumer.Models;
using Spectre.Console;

namespace External.Client.ApiConsumer.Services.Display;

/// <summary>
/// Service responsible for displaying palette-related information
/// </summary>
public class PaletteDisplayService : IPaletteDisplayService
{
    private readonly PaletteSettings _paletteSettings;

    public PaletteDisplayService(PaletteSettings paletteSettings)
    {
        _paletteSettings = paletteSettings;
    }

    public void DisplayWelcome()
    {
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
            var maxColors = _paletteSettings.MaxColorsPerPalette;
            var statusText = colorCount == maxColors
                ? $"[red bold]{colorCount}/{maxColors} FULL[/]"
                : $"[green]{colorCount}/{maxColors}[/]";

            var colorPreview = "";
            // Sort colors by brightness (dark to bright) and take up to maxColors
            var displayedColors = SortColorsByBrightness(palette.Colors).Take(maxColors).ToList();

            foreach (var color in displayedColors)
            {
                colorPreview += $"[on rgb({color.R},{color.G},{color.B})] ■ [/]";
            }

            // Add empty slots if less than max colors
            for (var i = colorCount; i < maxColors; i++)
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

        // Pagination info
        if (palettes.TotalPages > 1)
        {
            var paginationHints = new List<string>();

            if (palettes.HasPreviousPage)
                paginationHints.Add("[cyan]← Previous page available[/]");

            paginationHints.Add($"[yellow]Page {palettes.PageNumber} of {palettes.TotalPages}[/]");

            if (palettes.HasNextPage)
                paginationHints.Add("[cyan]Next page available →[/]");

            if (paginationHints.Any())
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[dim italic]{string.Join(" | ", paginationHints)}[/]");
            }
        }
    }

    public void DisplayPaletteDetails(PaletteResponse palette)
    {
        var colorCount = palette.Colors.Count;
        const int maxColors = 5;
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
            $"[bold cyan]Capacity:[/] {maxColors - colorCount} slots remaining"
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
            .Caption("[dim]Colors ordered by brightness (dark to bright)[/]");

        table.AddColumn(new TableColumn("[bold]#[/]").Centered().Width(3));
        table.AddColumn(new TableColumn("[bold]Visual[/]").Centered().Width(8));
        table.AddColumn(new TableColumn("[bold]RGB Values[/]").LeftAligned().Width(20));
        table.AddColumn(new TableColumn("[bold]Alpha[/]").Centered().Width(8));
        table.AddColumn(new TableColumn("[bold]Hex Code[/]").Centered().Width(10));
        table.AddColumn(new TableColumn("[bold]Brightness[/]").Centered().Width(10));

        // Sort colors by brightness (dark to bright) for display
        var sortedColors = SortColorsByBrightness(palette.Colors).ToList();

        var order = 1;
        foreach (var color in sortedColors)
        {
            var rgbText = $"({color.R}, {color.G}, {color.B})";
            var alphaText = $"{color.A:F2}";
            var colorBlock = $"[on rgb({color.R},{color.G},{color.B})] ████ [/]";
            var hexValue = !string.IsNullOrEmpty(color.Hex) ? color.Hex : $"#{color.R:X2}{color.G:X2}{color.B:X2}";

            // Calculate brightness (perceived luminance)
            var brightness = CalculateBrightness(color);
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

    public void DisplayPaletteCapacity(int currentColors, int maxColors, string paletteName)
    {
        var remainingSlots = maxColors - currentColors;
        var statusText = currentColors == maxColors ? "[red bold]FULL[/]" : "[green]AVAILABLE[/]";

        // Create visual representation of slots
        var slotsVisual = "";
        for (var i = 0; i < currentColors; i++)
        {
            slotsVisual += "[green]■[/]";
        }

        for (var i = currentColors; i < maxColors; i++)
        {
            slotsVisual += "[dim]□[/]";
        }

        var capacityInfo = new Panel(
            $"[bold]Palette:[/] [cyan]{paletteName.EscapeMarkup()}[/]\n" +
            $"[bold]Capacity:[/] {slotsVisual} ({currentColors}/{maxColors})\n" +
            $"[bold]Status:[/] {statusText}\n" +
            $"[bold]Remaining:[/] [yellow]{remainingSlots} slots available[/]")
        {
            Header = new PanelHeader("[bold yellow]PALETTE CAPACITY STATUS[/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(currentColors == maxColors ? Color.Red : Color.Green),
            Padding = new Padding(2, 1, 2, 1)
        };

        AnsiConsole.Write(capacityInfo);
    }

    private double CalculateBrightness(ColorResponse color)
    {
        // Calculate perceived luminance using standard formula
        return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
    }

    private IEnumerable<ColorResponse> SortColorsByBrightness(IEnumerable<ColorResponse> colors)
    {
        // Sort colors from dark to bright based on perceived luminance
        return colors.OrderBy(CalculateBrightness);
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

        // Analyze brightness distribution
        var brightColors = colorList.Count(c => CalculateBrightness(c) >= 0.7);
        var darkColors = colorList.Count(c => CalculateBrightness(c) <= 0.3);

        if (brightColors > darkColors)
            analysis.Add("[white]High contrast[/] - Good for readability and emphasis");
        else if (darkColors > brightColors)
            analysis.Add("[dim]Low key[/] - Sophisticated and elegant mood");
        else
            analysis.Add("[yellow]Balanced brightness[/] - Harmonious light distribution");

        // Analyze color diversity
        var redDominant = colorList.Count(c => c.R > c.G && c.R > c.B);
        var greenDominant = colorList.Count(c => c.G > c.R && c.G > c.B);
        var blueDominant = colorList.Count(c => c.B > c.R && c.B > c.G);

        if (redDominant > 0 && greenDominant > 0 && blueDominant > 0)
            analysis.Add("[rainbow]Full spectrum[/] - Rich and vibrant color range");
        else if (redDominant > 0 || greenDominant > 0 || blueDominant > 0)
            analysis.Add("[yellow]Focused spectrum[/] - Cohesive color theme");

        return string.Join("\n", analysis);
    }
}