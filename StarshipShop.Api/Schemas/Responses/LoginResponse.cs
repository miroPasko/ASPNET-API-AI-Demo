namespace StarshipShop.Api.Schemas.Responses;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
