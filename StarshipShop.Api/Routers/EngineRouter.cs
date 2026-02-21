using Microsoft.AspNetCore.Mvc;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Services;

namespace StarshipShop.Api.Routers;

public static class EngineRouter
{
    public static RouteGroupBuilder MapEngineRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/", async ([AsParameters] PaginationQuery query, IEngineService engineService) =>
        {
            var response = await engineService.GetAllAsync(query);
            return Results.Ok(response);
        })
        .WithName("GetAllEngines");

        group.MapGet("/{id:int}", async (int id, IEngineService engineService) =>
        {
            var response = await engineService.GetByIdAsync(id);
            return Results.Ok(response);
        })
        .WithName("GetEngineById");

        group.MapPost("/create", async ([FromBody] CreateEngineRequest request, IEngineService engineService) =>
        {
            var response = await engineService.CreateAsync(request);
            return Results.Created($"/api/engines/{response.Id}", response);
        })
        .WithName("CreateEngine");

        group.MapPut("/{id:int}", async (int id, [FromBody] UpdateEngineRequest request, IEngineService engineService) =>
        {
            var response = await engineService.UpdateAsync(id, request);
            return Results.Ok(response);
        })
        .WithName("UpdateEngine");

        group.MapDelete("/{id:int}", async (int id, IEngineService engineService) =>
        {
            await engineService.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteEngine");

        return group;
    }
}
