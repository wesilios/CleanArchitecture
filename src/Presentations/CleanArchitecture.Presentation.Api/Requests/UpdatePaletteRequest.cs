using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Presentation.Api.Requests;

public class UpdatePaletteRequest
{
    [Required(ErrorMessage = "Palette name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Palette name must be between 1 and 100 characters")]
    public string Name { get; set; } = string.Empty;
}