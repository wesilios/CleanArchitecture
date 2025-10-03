using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Infrastructure.DataAccess.Repositories;

public class PaletteRepository : IPaletteRepository
{
    private readonly ICleanArchitectureConnectionFactory _factory;
    private readonly CleanArchitectureDbContext _dbContext;

    public PaletteRepository(ICleanArchitectureConnectionFactory factory, CleanArchitectureDbContext dbContext)
    {
        _factory = factory;
        _dbContext = dbContext;
    }

    public async Task UpdateAsync(Palette palette)
    {
        await using var connection = _factory.CreateConnection();
        await connection.OpenAsync();

        await using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            "UPDATE Palettes SET Name = @Name WHERE PaletteId = @PaletteId",
            new { palette.Name, palette.PaletteId }, transaction);

        await connection.ExecuteAsync(
            "DELETE FROM PaletteColors WHERE PaletteId = @PaletteId",
            new { palette.PaletteId }, transaction);

        foreach (var color in palette.Colors)
        {
            await connection.ExecuteAsync(
                @"INSERT INTO PaletteColors (PaletteId, R, G, B, A) 
                      VALUES (@PaletteId, @R, @G, @B, @A)",
                new {
                    palette.PaletteId,
                    color.R,
                    color.G,
                    color.B,
                    color.A },
                transaction);
        }

        await transaction.CommitAsync();
    }

    public async Task AddAsync(Palette palette)
    {
        await _dbContext.Palettes.AddAsync(palette);
        await _dbContext.SaveChangesAsync();
    }

    public Task SaveChangesAsync()
    {
        /* No-op or commit transaction if managed externally */
        return Task.CompletedTask;
    }
}