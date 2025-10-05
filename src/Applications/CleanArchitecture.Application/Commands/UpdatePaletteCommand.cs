namespace CleanArchitecture.Application.Commands;

public class UpdatePaletteCommand : ICommand
{
    public long PaletteId { get; set; }
    public string Name { get; set; }
}