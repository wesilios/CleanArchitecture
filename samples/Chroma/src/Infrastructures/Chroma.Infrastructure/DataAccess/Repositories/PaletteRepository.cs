using Chroma.Domain.Entities;
using Chroma.Domain.Repositories;
using Dapper;

namespace Chroma.Infrastructure.DataAccess.Repositories;

public class PaletteRepository : IPaletteRepository
{
    private readonly IChromaConnectionFactory _sqlConnectionFactory;

    public PaletteRepository(IChromaConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Palette> GetByIdAsync(long id)
    {
        await using var connection = _sqlConnectionFactory.CreateConnection();

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

    public async Task UpdateAsync(Palette palette)
    {
        await using var connection = _sqlConnectionFactory.CreateConnection();
        await using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            "UPDATE Palettes SET Name = @Name WHERE Id = @Id",
            new { palette.Name, palette.Id }, transaction);

        await connection.ExecuteAsync(
            "DELETE FROM PaletteColors WHERE PaletteId = @Id",
            new { palette.Id }, transaction);

        foreach (var color in palette.Colors)
        {
            await connection.ExecuteAsync(
                @"INSERT INTO PaletteColors (PaletteId, R, G, B, A) 
                      VALUES (@Id, @RedPigment, @GreenPigment, @BluePigment, @Opacity)",
                new { palette.Id, color.RedPigment, color.GreenPigment, color.BluePigment, color.Opacity },
                transaction);
        }

        transaction.Commit();
    }

    public Task AddAsync(Palette palette)
    {
        /* ... Dapper Insert logic ... */
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        /* No-op or commit transaction if managed externally */
        return Task.CompletedTask;
    }
}