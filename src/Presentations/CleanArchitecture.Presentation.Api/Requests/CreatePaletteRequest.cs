using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Presentation.Api.Requests;

public class CreatePaletteRequest
{
    [Required]
    [StringLength(100, ErrorMessage = "Name is too long.")]
    public string Name { get; set; }
}