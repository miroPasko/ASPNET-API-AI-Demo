using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

public enum VesselType
{
    Fighter,
    Frigate,
    SmallCargo,
    SmallTransport,
    Custom
}

[Table("private_vessels")]
public class PrivateVessel : Starship
{
    [Column("vessel_type")]
    public VesselType VesselType { get; set; }
}
