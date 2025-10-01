using Chroma.Application.Interfaces;
using Chroma.Domain.Entities;

namespace Chroma.Infrastructure.DataAccess.Services;

public class PaletteQueryService : IPaletteQueryService
{
    private readonly IChromaConnectionFactory _factory;

    public PaletteQueryService(IChromaConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Palette> GetByIdAsync(long paletteId)
    {
        await using var connection = _factory.CreateConnection();

        // This SQL fetches the Palette and all its Colors efficiently
        const string sql = @"
                SELECT p.Id, p.Name, 
                       c.R AS RedPigment, c.G AS GreenPigment, c.B AS BluePigment, c.A AS Opacity
                FROM Palettes p
                LEFT JOIN PaletteColors c ON p.Id = c.PaletteId
                WHERE p.Id = @PaletteId";

        // Use Dapper's QueryMultiple or Query to reconstruct the aggregate root
        // Note: Reconstructing the complex object from flat rows is complex with Dapper.
        // For a complex write model, many teams keep EF Core for the Repository pattern 
        // and ONLY use Dapper for the read side. For simplicity, we assume
        // reconstruction logic here (often using Dapper's mapping features).

        // ... (Actual reconstruction logic is verbose and omitted for brevity)
        // ... (For a real implementation, you'd load Palette and Colors separately or use 
        //      Dapper's 'splitOn' feature, and then manually reconstruct the Palette object 
        //      to ensure domain rules are maintained).

        // Placeholder return:
        return new Palette("Test Palette");
    }
}