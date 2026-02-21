using System.ComponentModel.DataAnnotations;

namespace StarshipShop.Api.Schemas.Requests;

public class CreateEngineRequest
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public double MaximumSpeed { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public double FuelUsage { get; set; }

    [Required]
    [MaxLength(200)]
    public string Manufacturer { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}
