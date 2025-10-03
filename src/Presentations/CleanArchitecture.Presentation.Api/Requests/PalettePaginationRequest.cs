using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Presentation.Api.Requests;

public class PalettePaginationRequest : PaginationRequest
{
    [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z0-9\s\-_]*$", ErrorMessage = "Search term contains invalid characters")]
    public string? SearchTerm { get; set; }
}