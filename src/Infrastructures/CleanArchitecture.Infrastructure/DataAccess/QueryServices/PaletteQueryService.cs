using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Infrastructure.DataAccess.QueryServices;

public class PaletteQueryService : IPaletteQueryService
{
    private readonly ICleanArchitectureConnectionFactory _factory;

    public PaletteQueryService(ICleanArchitectureConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Palette?> GetByIdAsync(long paletteId)
    {
        await using var connection = _factory.CreateConnection();

        // This SQL fetches the Palette and all its Colors efficiently
        const string sql = @"
                SELECT p.PaletteId, p.Name, c.R, c.G, c.B, c.A
                FROM Palettes p
                LEFT JOIN PaletteColors c ON p.PaletteId = c.PaletteId
                WHERE p.PaletteId = @PaletteId";

        Palette? palette = null;

        await connection.QueryAsync<Palette?, Color?, Palette?>(sql, (p, c) =>
        {
            palette ??= p;

            if (c != null)
            {
                palette?.AddColor(c);
            }

            return palette;
        }, new { PaletteId = paletteId }, splitOn: "R");

        return palette;
    }

    public Task<PagedList<Palette>> GetGetAllPalettesAsync(GetAllPalettesQuery query)
    {
        throw new NotImplementedException();
    }
}