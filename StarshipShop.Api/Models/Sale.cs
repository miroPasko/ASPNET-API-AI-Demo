using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarshipShop.Api.Models;

[Table("sales")]
public class Sale
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("starship_id")]
    public int StarshipId { get; set; }

    [Required]
    [Column("payment_details_id")]
    public int PaymentDetailsId { get; set; }

    [Required]
    [Column("purchased_at")]
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(StarshipId))]
    public Starship Starship { get; set; } = null!;

    [ForeignKey(nameof(PaymentDetailsId))]
    public PaymentDetails PaymentDetails { get; set; } = null!;
}
