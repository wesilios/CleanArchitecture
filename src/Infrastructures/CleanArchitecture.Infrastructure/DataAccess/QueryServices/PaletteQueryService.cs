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
                SELECT p.PaletteId, p.Name, p.CreatedTime, c.R, c.G, c.B, c.A
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

    public async Task<PagedList<Palette>> GetGetAllPalettesAsync(GetAllPalettesSearchQuery searchQuery)
    {
        await using var connection = _factory.CreateConnection();

        var parameters = new
        {
            SearchTerm = string.IsNullOrWhiteSpace(searchQuery.SearchTerm)
                ? null
                : $"%{searchQuery.SearchTerm}%",
            Offset = searchQuery.PaginationParameters.OffSet,
            PageSize = searchQuery.PaginationParameters.PageSize
        };

        var countSql = @"SELECT COUNT(DISTINCT p.PaletteId) FROM Palettes p 
                   WHERE (@SearchTerm IS NULL OR p.Name LIKE @SearchTerm)";

        var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

        // First, get the paginated palette IDs
        var paletteIdsSql = @"
            SELECT p.PaletteId
            FROM Palettes p
            WHERE (@SearchTerm IS NULL OR p.Name LIKE @SearchTerm)
            ORDER BY p.PaletteId DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        var paletteIds = await connection.QueryAsync<long>(paletteIdsSql, parameters);

        if (!paletteIds.Any())
        {
            return new PagedList<Palette>
            {
                Results = new List<Palette>(),
                TotalCount = totalCount,
                PageNumber = searchQuery.PaginationParameters.PageNumber,
                PageSize = searchQuery.PaginationParameters.PageSize
            };
        }

        // Then get the full palette data with colors for those specific palettes
        var sql = @"
            SELECT p.PaletteId, p.Name, p.CreatedTime, c.R, c.G, c.B, c.A
            FROM Palettes p
            LEFT JOIN PaletteColors c ON p.PaletteId = c.PaletteId
            WHERE p.PaletteId IN @PaletteIds
            ORDER BY p.PaletteId DESC";

        var paletteDictionary = new Dictionary<long, Palette>();

        await connection.QueryAsync<Palette?, Color?, Palette?>(sql, (palette, color) =>
        {
            if (!paletteDictionary.TryGetValue(palette.PaletteId, out var existingPalette))
            {
                existingPalette = palette;
                paletteDictionary.Add(palette.PaletteId, existingPalette);
            }

            if (color != null)
            {
                existingPalette.AddColor(color);
            }

            return existingPalette;
        }, new { PaletteIds = paletteIds }, splitOn: "R");

        return new PagedList<Palette>
        {
            Results = paletteDictionary.Values.ToList(),
            TotalCount = totalCount,
            PageNumber = searchQuery.PaginationParameters.PageNumber,
            PageSize = searchQuery.PaginationParameters.PageSize
        };
    }
}