using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Presentation.Api.Requests;

public class CreatePaletteColorRequest
{
    [Range(0, 255, ErrorMessage = "Red value must be between 0 and 255")]
    public int R { get; set; }

    [Range(0, 255, ErrorMessage = "Green value must be between 0 and 255")]
    public int G { get; set; }

    [Range(0, 255, ErrorMessage = "Blue value must be between 0 and 255")]
    public int B { get; set; }

    [Range(0.0, 1.0, ErrorMessage = "Alpha value must be between 0.0 and 1.0")]
    public decimal A { get; set; } = 1.0m;
}