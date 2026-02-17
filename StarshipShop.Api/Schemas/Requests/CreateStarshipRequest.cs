using System.ComponentModel.DataAnnotations;
using StarshipShop.Api.Models;

namespace StarshipShop.Api.Schemas.Requests;

public class CreateStarshipRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Manufacturer { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public int EngineId { get; set; }

    [Required]
    public bool FtlCapable { get; set; }

    public int? FtlDriveId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int TotalCrew { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int TotalCapacity { get; set; }

    [MaxLength(500)]
    public string? IconPictureFilePath { get; set; }

    [Required]
    [MaxLength(50)]
    public string StarshipType { get; set; } = null!;

    // PrivateVessel specific
    public string? VesselType { get; set; }

    // PublicTransportVessel specific
    public string? TransportClass { get; set; }

    [Range(0, int.MaxValue)]
    public int? TotalPassengers { get; set; }

    // CargoVessel specific
    public string? CargoType { get; set; }

    [Range(0, int.MaxValue)]
    public int? TotalCargoCapacity { get; set; }
}
