using Chroma.Domain.Entities;
using Chroma.Domain.Repositories;
using Dapper;

namespace Chroma.Infrastructure.DataAccess.Repositories;

public class PaletteRepository : IPaletteRepository
{
    private readonly IChromaConnectionFactory _factory;

    public PaletteRepository(IChromaConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task UpdateAsync(Palette palette)
    {
        await using var connection = _factory.CreateConnection();
        await using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            "UPDATE Palettes SET Name = @Name WHERE Id = @Id",
            new { palette.Name, Id = palette.PaletteId }, transaction);

        await connection.ExecuteAsync(
            "DELETE FROM PaletteColors WHERE PaletteId = @Id",
            new { Id = palette.PaletteId }, transaction);

        foreach (var color in palette.Colors)
        {
            await connection.ExecuteAsync(
                @"INSERT INTO PaletteColors (PaletteId, R, G, B, A) 
                      VALUES (@Id, @RedPigment, @GreenPigment, @BluePigment, @Opacity)",
                new { Id = palette.PaletteId, color.RedPigment, color.GreenPigment, color.BluePigment, color.Opacity },
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