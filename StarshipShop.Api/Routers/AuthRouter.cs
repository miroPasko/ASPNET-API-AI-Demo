using Microsoft.AspNetCore.Mvc;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Services;

namespace StarshipShop.Api.Routers;

public static class AuthRouter
{
    public static RouteGroupBuilder MapAuthRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async ([FromBody] LoginRequest request, IAuthService authService) =>
        {
            var response = await authService.LoginAsync(request);
            return Results.Ok(response);
        })
        .AllowAnonymous()
        .WithName("Login");

        return group;
    }
}
