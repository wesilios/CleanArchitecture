using System.Collections;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.DataObjects;

namespace CleanArchitecture.Presentation.Api.Responses;

public class PalettePaginationResponse : IPaginationResponse<PaletteResponse>
{
    public static PalettePaginationResponse MapToResponse(IPagedList<IPaletteDto> pagedPalettes)
    {
        return new PalettePaginationResponse
        {
            Results = pagedPalettes.Results.Select(PaletteResponse.CreateInstance).ToList(),
            TotalCount = pagedPalettes.TotalCount,
            PageNumber = pagedPalettes.PageNumber,
            ItemsPerPage = pagedPalettes.ItemsPerPage
        };
    }

    public List<PaletteResponse> Results { get; set; } = new();

    IReadOnlyList<PaletteResponse> IPaginationResponse<PaletteResponse>.Results { get; }

    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int ItemsPerPage { get; set; }

    public IEnumerator<PaletteResponse> GetEnumerator()
    {
        return Results.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}