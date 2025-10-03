using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.DataObjects;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Api.Controllers;

[Route("[controller]")]
public class PalettesController : ApiBaseController
{
    private readonly IDispatcher _dispatcher;

    public PalettesController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaletteAsync([FromBody] CreatePaletteRequest request)
    {
        var command = new CreatePaletteCommand { Name = request.Name };
        await _dispatcher.SendAsync(command);
        return ReturnActionResult(ApiResult<object>.Created(ApiMessages.Palette.Created));
    }

    [HttpGet]
    public async Task<IActionResult> GetPalettesAsync([FromQuery] PalettePaginationRequest request)
    {
        var query = new GetAllPalettesSearchQuery
        {
            PaginationParameters = new PaginationParameters
                { PageNumber = request.PageNumber, PageSize = request.PageSize },
            SearchTerm = request.SearchTerm
        };
        var palettesList = await _dispatcher.QueryAsync<GetAllPalettesSearchQuery, IPagedList<IPaletteDto>>(query);
        var response = PalettePaginationResponse.MapToResponse(palettesList);
        return ReturnActionResult(ApiResult<PalettePaginationResponse>.Ok(response, ApiMessages.Palette.AllRetrieved));
    }

    [HttpGet("{paletteId:long}")]
    public async Task<IActionResult> GetPaletteAsync(long paletteId)
    {
        var query = new GetPaletteByIdQuery { PaletteId = paletteId };
        var dto = await _dispatcher.QueryAsync<GetPaletteByIdQuery, IPaletteDto>(query);

        var response = MapToResponse(dto);

        if (response.Empty)
            return ReturnActionResult(ApiResult.NotFound(ApiMessages.Palette.NotFound));
        return ReturnActionResult(ApiResult<IPaletteResponse>.Ok(response, ApiMessages.Palette.Retrieved));
    }

    [HttpPut("{paletteId:long}")]
    public async Task<IActionResult> UpdatePaletteAsync(long paletteId, [FromBody] UpdatePaletteRequest request)
    {
        var command = new UpdatePaletteCommand { PaletteId = paletteId, Name = request.Name };
        await _dispatcher.SendAsync(command);
        return ReturnActionResult(ApiResult.Ok());
    }
    
    [HttpDelete("{paletteId:long}")]
    public async Task<IActionResult> DeletePaletteAsync(long paletteId)
    {
        var command = new DeletePaletteCommand { PaletteId = paletteId };
        await _dispatcher.SendAsync(command);
        return ReturnActionResult(ApiResult.Ok());
    }

    [HttpPost("{paletteId:long}/colors")]
    public async Task<IActionResult> CreatePaletteColorAsync(long paletteId,
        [FromBody] CreatePaletteColorRequest request)
    {
        var command = new CreateColorToPaletteCommand
            { PaletteId = paletteId, R = request.R, G = request.G, B = request.B, A = request.A };
        await _dispatcher.SendAsync(command);
        return ReturnActionResult(ApiResult<object>.Created(ApiMessages.Palette.ColorAdded));
    }

    private static IPaletteResponse MapToResponse(IPaletteDto dto)
    {
        if (dto.Empty) return NullPaletteResponse.Instance;

        return PaletteResponse.CreateInstance(dto);
    }
}