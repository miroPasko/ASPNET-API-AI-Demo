using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;

namespace StarshipShop.Api.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
