using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StarshipShop.Api.Data;
using StarshipShop.Api.Models;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;
using StarshipShop.Api.Validators;

namespace StarshipShop.Api.Services;

public class FtlDriveService(AppDbContext context) : IFtlDriveService
{
    private readonly CreateFtlDriveRequestValidator _createValidator = new();
    private readonly UpdateFtlDriveRequestValidator _updateValidator = new();

    public async Task<PaginatedResponse<FtlDriveResponse>> GetAllAsync(PaginationQuery query)
    {
        var totalCount = await context.FtlDrives.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        var ftlDrives = await context.FtlDrives
            .OrderBy(f => f.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PaginatedResponse<FtlDriveResponse>
        {
            Items = ftlDrives.Select(MapToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = query.Page > 1,
            HasNextPage = query.Page < totalPages
        };
    }

    public async Task<FtlDriveResponse> GetByIdAsync(int id)
    {
        var ftlDrive = await context.FtlDrives.FindAsync(id);
        if (ftlDrive == null)
        {
            throw new KeyNotFoundException($"FtlDrive with ID {id} not found");
        }

        return MapToResponse(ftlDrive);
    }

    public async Task<FtlDriveResponse> CreateAsync(CreateFtlDriveRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var ftlDrive = new FtlDrive
        {
            Name = request.Name,
            Description = request.Description,
            MaximumSpeed = request.MaximumSpeed,
            FuelUsage = request.FuelUsage,
            Manufacturer = request.Manufacturer,
            Price = request.Price
        };

        context.FtlDrives.Add(ftlDrive);
        await context.SaveChangesAsync();

        return MapToResponse(ftlDrive);
    }

    public async Task<FtlDriveResponse> UpdateAsync(int id, UpdateFtlDriveRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var ftlDrive = await context.FtlDrives.FindAsync(id);
        if (ftlDrive == null)
        {
            throw new KeyNotFoundException($"FtlDrive with ID {id} not found");
        }

        if (request.Name != null) ftlDrive.Name = request.Name;
        if (request.Description != null) ftlDrive.Description = request.Description;
        if (request.MaximumSpeed.HasValue) ftlDrive.MaximumSpeed = request.MaximumSpeed.Value;
        if (request.FuelUsage.HasValue) ftlDrive.FuelUsage = request.FuelUsage.Value;
        if (request.Manufacturer != null) ftlDrive.Manufacturer = request.Manufacturer;
        if (request.Price.HasValue) ftlDrive.Price = request.Price.Value;

        await context.SaveChangesAsync();

        return MapToResponse(ftlDrive);
    }

    public async Task DeleteAsync(int id)
    {
        var ftlDrive = await context.FtlDrives.FindAsync(id);
        if (ftlDrive == null)
        {
            throw new KeyNotFoundException($"FtlDrive with ID {id} not found");
        }

        context.FtlDrives.Remove(ftlDrive);
        await context.SaveChangesAsync();
    }

    private static FtlDriveResponse MapToResponse(FtlDrive ftlDrive) => new()
    {
        Id = ftlDrive.Id,
        Name = ftlDrive.Name,
        Description = ftlDrive.Description,
        MaximumSpeed = ftlDrive.MaximumSpeed,
        FuelUsage = ftlDrive.FuelUsage,
        Manufacturer = ftlDrive.Manufacturer,
        Price = ftlDrive.Price
    };
}
