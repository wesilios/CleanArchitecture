namespace Chroma.Application.Commands;

public class AddPaletteCommand : ICommand
{
    public long PaletteId { get; set; }
    public string Name { get; set; }
}