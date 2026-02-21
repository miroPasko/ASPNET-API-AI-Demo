using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StarshipShop.Api.Data;
using StarshipShop.Api.Models;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;
using StarshipShop.Api.Validators;

namespace StarshipShop.Api.Services;

public class EngineService(AppDbContext context) : IEngineService
{
    private readonly CreateEngineRequestValidator _createValidator = new();
    private readonly UpdateEngineRequestValidator _updateValidator = new();

    public async Task<PaginatedResponse<EngineResponse>> GetAllAsync(PaginationQuery query)
    {
        var totalCount = await context.Engines.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        var engines = await context.Engines
            .OrderBy(e => e.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PaginatedResponse<EngineResponse>
        {
            Items = engines.Select(MapToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = query.Page > 1,
            HasNextPage = query.Page < totalPages
        };
    }

    public async Task<EngineResponse> GetByIdAsync(int id)
    {
        var engine = await context.Engines.FindAsync(id);
        if (engine == null)
        {
            throw new KeyNotFoundException($"Engine with ID {id} not found");
        }

        return MapToResponse(engine);
    }

    public async Task<EngineResponse> CreateAsync(CreateEngineRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var engine = new Engine
        {
            Name = request.Name,
            Description = request.Description,
            MaximumSpeed = request.MaximumSpeed,
            FuelUsage = request.FuelUsage,
            Manufacturer = request.Manufacturer,
            Price = request.Price
        };

        context.Engines.Add(engine);
        await context.SaveChangesAsync();

        return MapToResponse(engine);
    }

    public async Task<EngineResponse> UpdateAsync(int id, UpdateEngineRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var engine = await context.Engines.FindAsync(id);
        if (engine == null)
        {
            throw new KeyNotFoundException($"Engine with ID {id} not found");
        }

        if (request.Name != null) engine.Name = request.Name;
        if (request.Description != null) engine.Description = request.Description;
        if (request.MaximumSpeed.HasValue) engine.MaximumSpeed = request.MaximumSpeed.Value;
        if (request.FuelUsage.HasValue) engine.FuelUsage = request.FuelUsage.Value;
        if (request.Manufacturer != null) engine.Manufacturer = request.Manufacturer;
        if (request.Price.HasValue) engine.Price = request.Price.Value;

        await context.SaveChangesAsync();

        return MapToResponse(engine);
    }

    public async Task DeleteAsync(int id)
    {
        var engine = await context.Engines.FindAsync(id);
        if (engine == null)
        {
            throw new KeyNotFoundException($"Engine with ID {id} not found");
        }

        context.Engines.Remove(engine);
        await context.SaveChangesAsync();
    }

    private static EngineResponse MapToResponse(Engine engine) => new()
    {
        Id = engine.Id,
        Name = engine.Name,
        Description = engine.Description,
        MaximumSpeed = engine.MaximumSpeed,
        FuelUsage = engine.FuelUsage,
        Manufacturer = engine.Manufacturer,
        Price = engine.Price
    };
}
