using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    public ICollection<PaymentDetails> PaymentDetails { get; set; } = [];
    public ICollection<Sale> Sales { get; set; } = [];
}
