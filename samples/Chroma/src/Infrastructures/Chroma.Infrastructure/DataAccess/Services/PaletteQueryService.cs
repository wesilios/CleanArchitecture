using Chroma.Application.Common;
using Chroma.Application.Interfaces;
using Chroma.Application.Queries;
using Chroma.Domain.Entities;
using Chroma.Domain.ValueObjects;

namespace Chroma.Infrastructure.DataAccess.Services;

public class PaletteQueryService : IPaletteQueryService
{
    private readonly IChromaConnectionFactory _factory;

    public PaletteQueryService(IChromaConnectionFactory factory)
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