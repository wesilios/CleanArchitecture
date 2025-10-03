namespace CleanArchitecture.Presentation.Api.Common;

public static class ApiMessages
{
    public const string Success = "Operation completed successfully";
    public const string CreatedSuccessfully = "Resource created successfully";
    public const string ResourceNotFound = "Resource not found";
    public const string BadRequest = "Invalid request data";
    public const string Unauthorized = "Unauthorized access";
    public const string Forbidden = "Access forbidden";
    public const string InternalServerError = "An internal server error occurred";

    // Palette specific messages
    public static class Palette
    {
        public const string Created = "Palette created successfully";
        public const string Retrieved = "Palette retrieved successfully";
        public const string AllRetrieved = "Palettes retrieved successfully";
        public const string NotFound = "Palette not found";
        public const string ColorAdded = "Color added to palette successfully";
    }
}