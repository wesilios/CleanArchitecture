using System.Net;

namespace CleanArchitecture.Presentation.Api.Common;

public interface IApiResult
{
    string Message { get; }
    int StatusCode { get; }
}

public class ApiResult : IApiResult
{
    public string Message { get; }
    public int StatusCode { get; }

    private ApiResult(string message, int statusCode)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        StatusCode = statusCode;
    }

    public static ApiResult Ok(string message = ApiMessages.Success) =>
        new(message, (int)HttpStatusCode.OK);

    public static ApiResult Created(string message = ApiMessages.CreatedSuccessfully) =>
        new(message, (int)HttpStatusCode.Created);

    public static ApiResult NotFound(string message = ApiMessages.ResourceNotFound) =>
        new(message, (int)HttpStatusCode.NotFound);

    public static ApiResult BadRequest(string message = ApiMessages.BadRequest) =>
        new(message, (int)HttpStatusCode.BadRequest);
}

public interface IApiResult<out T> : IApiResult
{
    T Data { get; }
}

public class ApiResult<TResponseData> : IApiResult<TResponseData>
{
    public TResponseData Data { get; }
    public string Message { get; }
    public int StatusCode { get; }

    private ApiResult(TResponseData data, string message, int statusCode)
    {
        Data = data;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        StatusCode = statusCode;
    }

    public static ApiResult<TResponseData> Ok(TResponseData data, string message = ApiMessages.Success) =>
        new(data, message, (int)HttpStatusCode.OK);

    public static ApiResult<TResponseData>
        Created(TResponseData data, string message = ApiMessages.CreatedSuccessfully) =>
        new(data, message, (int)HttpStatusCode.Created);

    public static ApiResult<TResponseData>
        NotFound(TResponseData data, string message = ApiMessages.ResourceNotFound) =>
        new(data, message, (int)HttpStatusCode.NotFound);

    public static ApiResult<TResponseData> BadRequest(TResponseData data, string message = ApiMessages.BadRequest) =>
        new(data, message, (int)HttpStatusCode.BadRequest);
}