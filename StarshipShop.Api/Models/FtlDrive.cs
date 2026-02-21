using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

[Table("ftl_drives")]
public class FtlDrive
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [MaxLength(1000)]
    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("maximum_speed")]
    public double MaximumSpeed { get; set; }

    [Required]
    [Column("fuel_usage")]
    public double FuelUsage { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("manufacturer")]
    public string Manufacturer { get; set; } = null!;

    [Required]
    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public ICollection<Starship> Starships { get; set; } = [];
}
