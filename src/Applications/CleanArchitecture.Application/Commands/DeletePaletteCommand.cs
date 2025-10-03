namespace CleanArchitecture.Application.Commands;

public class DeletePaletteCommand : ICommand
{
    public long PaletteId { get; set; }
}