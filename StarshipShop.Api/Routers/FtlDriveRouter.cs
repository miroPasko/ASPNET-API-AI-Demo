using Microsoft.AspNetCore.Mvc;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Services;

namespace StarshipShop.Api.Routers;

public static class FtlDriveRouter
{
    public static RouteGroupBuilder MapFtlDriveRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/", async ([AsParameters] PaginationQuery query, IFtlDriveService ftlDriveService) =>
        {
            var response = await ftlDriveService.GetAllAsync(query);
            return Results.Ok(response);
        })
        .WithName("GetAllFtlDrives");

        group.MapGet("/{id:int}", async (int id, IFtlDriveService ftlDriveService) =>
        {
            var response = await ftlDriveService.GetByIdAsync(id);
            return Results.Ok(response);
        })
        .WithName("GetFtlDriveById");

        group.MapPost("/create", async ([FromBody] CreateFtlDriveRequest request, IFtlDriveService ftlDriveService) =>
        {
            var response = await ftlDriveService.CreateAsync(request);
            return Results.Created($"/api/ftl-drives/{response.Id}", response);
        })
        .WithName("CreateFtlDrive");

        group.MapPut("/{id:int}", async (int id, [FromBody] UpdateFtlDriveRequest request, IFtlDriveService ftlDriveService) =>
        {
            var response = await ftlDriveService.UpdateAsync(id, request);
            return Results.Ok(response);
        })
        .WithName("UpdateFtlDrive");

        group.MapDelete("/{id:int}", async (int id, IFtlDriveService ftlDriveService) =>
        {
            await ftlDriveService.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteFtlDrive");

        return group;
    }
}
