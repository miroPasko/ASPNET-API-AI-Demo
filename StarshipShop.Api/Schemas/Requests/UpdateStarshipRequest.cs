using System.ComponentModel.DataAnnotations;

namespace StarshipShop.Api.Schemas.Requests;

public class UpdateStarshipRequest
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(200)]
    public string? Manufacturer { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }

    public int? EngineId { get; set; }

    public bool? FtlCapable { get; set; }

    public int? FtlDriveId { get; set; }

    [Range(0, int.MaxValue)]
    public int? TotalCrew { get; set; }

    [Range(0, int.MaxValue)]
    public int? TotalCapacity { get; set; }

    [MaxLength(500)]
    public string? IconPictureFilePath { get; set; }

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
