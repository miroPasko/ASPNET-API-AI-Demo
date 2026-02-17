using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

[Table("payment_details")]
public class PaymentDetails
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [MaxLength(19)]
    [Column("card_number")]
    public string CardNumber { get; set; } = null!;

    [Required]
    [MaxLength(3)]
    [Column("currency")]
    public string Currency { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("country")]
    public string Country { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public ICollection<Sale> Sales { get; set; } = [];
}
