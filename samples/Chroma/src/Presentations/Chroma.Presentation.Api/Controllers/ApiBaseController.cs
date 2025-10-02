using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Chroma.Presentation.Api.Controllers;

public abstract class ApiBaseController : ControllerBase
{
    protected IActionResult ReturnActionResult<TData>(ApiResult<TData> response)
    {
        return response.StatusCode switch
        {
            (int)HttpStatusCode.OK => Ok(response),
            (int)HttpStatusCode.Created => Created(string.Empty, response),
            (int)HttpStatusCode.BadRequest => BadRequest(response),
            (int)HttpStatusCode.Unauthorized => Unauthorized(response),
            (int)HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, response),
            (int)HttpStatusCode.NotFound => NotFound(response),
            (int)HttpStatusCode.UnprocessableEntity => UnprocessableEntity(response),
            _ => StatusCode(500, response)
        };
    }
}