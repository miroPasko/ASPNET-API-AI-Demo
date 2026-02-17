using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

public enum CargoType
{
    Mixed,
    Liquid,
    RawMaterials,
    Vehicles,
    Hazardous
}

[Table("cargo_vessels")]
public class CargoVessel : Starship
{
    [Column("cargo_type")]
    public CargoType CargoType { get; set; }

    [Required]
    [Column("total_cargo_capacity")]
    public int TotalCargoCapacity { get; set; }
}
