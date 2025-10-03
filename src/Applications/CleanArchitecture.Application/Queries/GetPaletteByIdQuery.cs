namespace CleanArchitecture.Application.Queries;

public class GetPaletteByIdQuery : IQuery<IPaletteDto>
{
    public long PaletteId { get; set; }
}