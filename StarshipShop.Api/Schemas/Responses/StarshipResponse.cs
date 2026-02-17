namespace StarshipShop.Api.Schemas.Responses;

public class StarshipResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public decimal Price { get; set; }
    public int EngineId { get; set; }
    public string EngineName { get; set; } = null!;
    public bool FtlCapable { get; set; }
    public int? FtlDriveId { get; set; }
    public string? FtlDriveName { get; set; }
    public int TotalCrew { get; set; }
    public int TotalCapacity { get; set; }
    public string? IconPictureFilePath { get; set; }
    public string StarshipType { get; set; } = null!;

    // PrivateVessel specific
    public string? VesselType { get; set; }

    // PublicTransportVessel specific
    public string? TransportClass { get; set; }
    public int? TotalPassengers { get; set; }

    // CargoVessel specific
    public string? CargoType { get; set; }
    public int? TotalCargoCapacity { get; set; }
}
