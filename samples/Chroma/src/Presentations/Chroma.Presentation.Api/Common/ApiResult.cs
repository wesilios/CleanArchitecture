using System.Net;

namespace Chroma.Presentation.Api.Common;

public class ApiResult<TResponseData>
{
    public TResponseData Data { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public static ApiResult<TResponseData> Ok(TResponseData data, string message) =>
        Success(data, message, HttpStatusCode.OK);

    public static ApiResult<TResponseData> Created(TResponseData data, string message) =>
        Success(data, message, HttpStatusCode.Created);
    
    public static ApiResult<TResponseData> NotFound(TResponseData data, string message) =>
        Success(data, message, HttpStatusCode.NotFound);

    private static ApiResult<TResponseData> Success(TResponseData data, string message, HttpStatusCode statusCode)
    {
        return new ApiResult<TResponseData>
        {
            Data = data,
            Message = message,
            StatusCode = (int)statusCode
        };
    }
}