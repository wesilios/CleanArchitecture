namespace CleanArchitecture.Application.Commands;

public class AddColorToPaletteCommand : ICommand
{
    public long PaletteId { get; set; }
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public decimal A { get; set; }
}