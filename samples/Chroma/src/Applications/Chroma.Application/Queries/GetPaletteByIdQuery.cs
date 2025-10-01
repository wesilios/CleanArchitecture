using Chroma.Application.DataObjects;

namespace Chroma.Application.Queries;

public class GetPaletteByIdQuery : IQuery<PaletteDto>
{
    public long PaletteId { get; set; }
}