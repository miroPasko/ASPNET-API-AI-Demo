using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

public enum TransportClass
{
    Economy,
    Standard,
    Business,
    Luxury
}

[Table("public_transport_vessels")]
public class PublicTransportVessel : Starship
{
    [Column("transport_class")]
    public TransportClass TransportClass { get; set; }

    [Required]
    [Column("total_passengers")]
    public int TotalPassengers { get; set; }
}
