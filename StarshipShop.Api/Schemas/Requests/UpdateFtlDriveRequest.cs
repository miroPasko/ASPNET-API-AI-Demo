using System.ComponentModel.DataAnnotations;

namespace StarshipShop.Api.Schemas.Requests;

public class UpdateFtlDriveRequest
{
    [MaxLength(150)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public double? MaximumSpeed { get; set; }

    [Range(0.01, double.MaxValue)]
    public double? FuelUsage { get; set; }

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }
}
