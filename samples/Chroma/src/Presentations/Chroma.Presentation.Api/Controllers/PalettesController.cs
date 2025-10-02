using Chroma.Application.Commands;
using Chroma.Application.DataObjects;
using Chroma.Application.Interfaces;
using Chroma.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Chroma.Presentation.Api.Controllers;

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
        var command = new AddPaletteCommand { Name = request.Name };
        await _dispatcher.SendAsync(command);
        return ReturnActionResult(ApiResult<object>.Created(default, "Created successfully."));
    }

    [HttpGet("{paletteId:long}")]
    public async Task<IActionResult> Get(long paletteId)
    {
        var query = new GetPaletteByIdQuery { PaletteId = paletteId };
        var dto = await _dispatcher.QueryAsync<GetPaletteByIdQuery, IPaletteDto>(query);

        var response = MapToResponse(dto);

        if (response.Empty)
            return ReturnActionResult(ApiResult<IPaletteResponse>.NotFound(response, dto.Note));
        return ReturnActionResult(ApiResult<IPaletteDto>.Ok(dto, "Get successfully."));
    }

    [HttpPost("{paletteId:long}/colors")]
    public async Task<IActionResult> CreatePaletteColorAsync(long paletteId,
        [FromBody] CreatePaletteColorRequest request)
    {
        var command = new AddColorToPaletteCommand
            { PaletteId = paletteId, R = request.R, G = request.G, B = request.B, A = request.A };
        await _dispatcher.SendAsync(command);
        return ReturnActionResult(ApiResult<object>.Created(default, "Created successfully."));
    }

    private static IPaletteResponse MapToResponse(IPaletteDto dto)
    {
        if (dto.Empty) return NullPaletteResponse.Instance;

        return new PaletteResponse
        {
            PaletteId = dto.PaletteId,
            Name = dto.Name,
            Colors = dto.Colors.Select(c => new ColorResponse
            {
                R = c.R,
                G = c.G,
                B = c.B,
                A = c.A,
                Hex = c.Hex
            }).ToList()
        };
    }
}