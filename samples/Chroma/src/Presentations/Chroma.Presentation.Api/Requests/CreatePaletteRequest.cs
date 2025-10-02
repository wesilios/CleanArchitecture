using System.ComponentModel.DataAnnotations;

namespace Chroma.Presentation.Api.Requests;

public class CreatePaletteRequest
{
    [Required]
    [StringLength(100, ErrorMessage = "Name is too long.")]
    public string Name { get; set; }
}