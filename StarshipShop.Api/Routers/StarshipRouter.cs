using Microsoft.AspNetCore.Mvc;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Services;

namespace StarshipShop.Api.Routers;

public static class StarshipRouter
{
    public static RouteGroupBuilder MapStarshipRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/", async ([AsParameters] PaginationQuery query, IStarshipService starshipService) =>
        {
            var response = await starshipService.GetAllAsync(query);
            return Results.Ok(response);
        })
        .WithName("GetAllStarships");

        group.MapGet("/{id:int}", async (int id, IStarshipService starshipService) =>
        {
            var response = await starshipService.GetByIdAsync(id);
            return Results.Ok(response);
        })
        .WithName("GetStarshipById");

        group.MapPost("/create", async ([FromBody] CreateStarshipRequest request, IStarshipService starshipService) =>
        {
            var response = await starshipService.CreateAsync(request);
            return Results.Created($"/api/starships/{response.Id}", response);
        })
        .WithName("CreateStarship");

        group.MapPut("/{id:int}", async (int id, [FromBody] UpdateStarshipRequest request, IStarshipService starshipService) =>
        {
            var response = await starshipService.UpdateAsync(id, request);
            return Results.Ok(response);
        })
        .WithName("UpdateStarship");

        group.MapDelete("/{id:int}", async (int id, IStarshipService starshipService) =>
        {
            await starshipService.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteStarship");

        return group;
    }
}
