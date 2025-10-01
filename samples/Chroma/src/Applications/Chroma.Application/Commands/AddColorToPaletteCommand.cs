namespace Chroma.Application.Commands;

public class AddColorToPaletteCommand : ICommand
{
    public long PaletteId { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public decimal Opacity { get; set; }
}