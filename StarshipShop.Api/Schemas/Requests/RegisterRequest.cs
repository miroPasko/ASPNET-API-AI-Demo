using System.ComponentModel.DataAnnotations;

namespace StarshipShop.Api.Schemas.Requests;

public class RegisterRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
