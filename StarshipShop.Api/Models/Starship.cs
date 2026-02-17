using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

[Table("starships")]
public class Starship
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    [Column("manufacturer")]
    public string Manufacturer { get; set; } = null!;

    [Required]
    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    [Column("engine_id")]
    public int EngineId { get; set; }

    [Required]
    [Column("ftl_capable")]
    public bool FtlCapable { get; set; }

    [Column("ftl_drive_id")]
    public int? FtlDriveId { get; set; }

    [Required]
    [Column("total_crew")]
    public int TotalCrew { get; set; }

    [Required]
    [Column("total_capacity")]
    public int TotalCapacity { get; set; }

    [MaxLength(500)]
    [Column("icon_picture_file_path")]
    public string? IconPictureFilePath { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("starship_type")]
    public string StarshipType { get; set; } = null!;

    [ForeignKey(nameof(EngineId))]
    public Engine Engine { get; set; } = null!;

    [ForeignKey(nameof(FtlDriveId))]
    public FtlDrive? FtlDrive { get; set; }

    public ICollection<Sale> Sales { get; set; } = [];
}
