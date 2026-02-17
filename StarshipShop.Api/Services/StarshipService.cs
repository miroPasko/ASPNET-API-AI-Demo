using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StarshipShop.Api.Data;
using StarshipShop.Api.Models;
using StarshipShop.Api.Schemas.Requests;
using StarshipShop.Api.Schemas.Responses;
using StarshipShop.Api.Validators;

namespace StarshipShop.Api.Services;

public class StarshipService(AppDbContext context) : IStarshipService
{
    private readonly CreateStarshipRequestValidator _createValidator = new();
    private readonly UpdateStarshipRequestValidator _updateValidator = new();

    public async Task<PaginatedResponse<StarshipResponse>> GetAllAsync(PaginationQuery query)
    {
        var totalCount = await context.Starships.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        var starships = await context.Starships
            .Include(s => s.Engine)
            .Include(s => s.FtlDrive)
            .OrderBy(s => s.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PaginatedResponse<StarshipResponse>
        {
            Items = starships.Select(MapToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = query.Page > 1,
            HasNextPage = query.Page < totalPages
        };
    }

    public async Task<StarshipResponse> GetByIdAsync(int id)
    {
        var starship = await context.Starships
            .Include(s => s.Engine)
            .Include(s => s.FtlDrive)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (starship == null)
        {
            throw new KeyNotFoundException($"Starship with ID {id} not found");
        }

        return MapToResponse(starship);
    }

    public async Task<StarshipResponse> CreateAsync(CreateStarshipRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Verify Engine exists
        var engineExists = await context.Engines.AnyAsync(e => e.Id == request.EngineId);
        if (!engineExists)
        {
            throw new ArgumentException($"Engine with ID {request.EngineId} not found");
        }

        // Verify FtlDrive exists if provided
        if (request.FtlDriveId.HasValue)
        {
            var ftlDriveExists = await context.FtlDrives.AnyAsync(f => f.Id == request.FtlDriveId.Value);
            if (!ftlDriveExists)
            {
                throw new ArgumentException($"FtlDrive with ID {request.FtlDriveId} not found");
            }
        }

        Starship starship = request.StarshipType switch
        {
            "PrivateVessel" => new PrivateVessel
            {
                Name = request.Name,
                Manufacturer = request.Manufacturer,
                Price = request.Price,
                EngineId = request.EngineId,
                FtlCapable = request.FtlCapable,
                FtlDriveId = request.FtlDriveId,
                TotalCrew = request.TotalCrew,
                TotalCapacity = request.TotalCapacity,
                IconPictureFilePath = request.IconPictureFilePath,
                StarshipType = request.StarshipType,
                VesselType = Enum.Parse<VesselType>(request.VesselType!, true)
            },
            "PublicTransportVessel" => new PublicTransportVessel
            {
                Name = request.Name,
                Manufacturer = request.Manufacturer,
                Price = request.Price,
                EngineId = request.EngineId,
                FtlCapable = request.FtlCapable,
                FtlDriveId = request.FtlDriveId,
                TotalCrew = request.TotalCrew,
                TotalCapacity = request.TotalCapacity,
                IconPictureFilePath = request.IconPictureFilePath,
                StarshipType = request.StarshipType,
                TransportClass = Enum.Parse<TransportClass>(request.TransportClass!, true),
                TotalPassengers = request.TotalPassengers!.Value
            },
            "CargoVessel" => new CargoVessel
            {
                Name = request.Name,
                Manufacturer = request.Manufacturer,
                Price = request.Price,
                EngineId = request.EngineId,
                FtlCapable = request.FtlCapable,
                FtlDriveId = request.FtlDriveId,
                TotalCrew = request.TotalCrew,
                TotalCapacity = request.TotalCapacity,
                IconPictureFilePath = request.IconPictureFilePath,
                StarshipType = request.StarshipType,
                CargoType = Enum.Parse<CargoType>(request.CargoType!, true),
                TotalCargoCapacity = request.TotalCargoCapacity!.Value
            },
            _ => throw new ArgumentException($"Invalid StarshipType: {request.StarshipType}")
        };

        context.Starships.Add(starship);
        await context.SaveChangesAsync();

        // Reload with navigation properties
        await context.Entry(starship).Reference(s => s.Engine).LoadAsync();
        if (starship.FtlDriveId.HasValue)
        {
            await context.Entry(starship).Reference(s => s.FtlDrive).LoadAsync();
        }

        return MapToResponse(starship);
    }

    public async Task<StarshipResponse> UpdateAsync(int id, UpdateStarshipRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var starship = await context.Starships
            .Include(s => s.Engine)
            .Include(s => s.FtlDrive)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (starship == null)
        {
            throw new KeyNotFoundException($"Starship with ID {id} not found");
        }

        // Update base properties
        if (request.Name != null) starship.Name = request.Name;
        if (request.Manufacturer != null) starship.Manufacturer = request.Manufacturer;
        if (request.Price.HasValue) starship.Price = request.Price.Value;
        if (request.TotalCrew.HasValue) starship.TotalCrew = request.TotalCrew.Value;
        if (request.TotalCapacity.HasValue) starship.TotalCapacity = request.TotalCapacity.Value;
        if (request.IconPictureFilePath != null) starship.IconPictureFilePath = request.IconPictureFilePath;

        // Handle Engine update
        if (request.EngineId.HasValue)
        {
            var engineExists = await context.Engines.AnyAsync(e => e.Id == request.EngineId.Value);
            if (!engineExists)
            {
                throw new ArgumentException($"Engine with ID {request.EngineId} not found");
            }
            starship.EngineId = request.EngineId.Value;
        }

        // Handle FTL updates
        if (request.FtlCapable.HasValue)
        {
            starship.FtlCapable = request.FtlCapable.Value;
        }

        if (request.FtlDriveId.HasValue)
        {
            var ftlDriveExists = await context.FtlDrives.AnyAsync(f => f.Id == request.FtlDriveId.Value);
            if (!ftlDriveExists)
            {
                throw new ArgumentException($"FtlDrive with ID {request.FtlDriveId} not found");
            }
            starship.FtlDriveId = request.FtlDriveId.Value;
        }

        // Validate FTL consistency
        if (starship.FtlCapable && starship.FtlDriveId == null)
        {
            throw new ValidationException("FtlDriveId is required when FtlCapable is true");
        }

        // Update subtype-specific properties using pattern matching
        switch (starship)
        {
            case PrivateVessel pv:
                if (request.VesselType != null)
                {
                    pv.VesselType = Enum.Parse<VesselType>(request.VesselType, true);
                }
                break;

            case PublicTransportVessel ptv:
                if (request.TransportClass != null)
                {
                    ptv.TransportClass = Enum.Parse<TransportClass>(request.TransportClass, true);
                }
                if (request.TotalPassengers.HasValue)
                {
                    ptv.TotalPassengers = request.TotalPassengers.Value;
                }
                break;

            case CargoVessel cv:
                if (request.CargoType != null)
                {
                    cv.CargoType = Enum.Parse<CargoType>(request.CargoType, true);
                }
                if (request.TotalCargoCapacity.HasValue)
                {
                    cv.TotalCargoCapacity = request.TotalCargoCapacity.Value;
                }
                break;
        }

        await context.SaveChangesAsync();

        // Reload navigation properties
        await context.Entry(starship).Reference(s => s.Engine).LoadAsync();
        if (starship.FtlDriveId.HasValue)
        {
            await context.Entry(starship).Reference(s => s.FtlDrive).LoadAsync();
        }

        return MapToResponse(starship);
    }

    public async Task DeleteAsync(int id)
    {
        var starship = await context.Starships.FindAsync(id);
        if (starship == null)
        {
            throw new KeyNotFoundException($"Starship with ID {id} not found");
        }

        context.Starships.Remove(starship);
        await context.SaveChangesAsync();
    }

    private StarshipResponse MapToResponse(Starship starship)
    {
        var response = new StarshipResponse
        {
            Id = starship.Id,
            Name = starship.Name,
            Manufacturer = starship.Manufacturer,
            Price = starship.Price,
            EngineId = starship.EngineId,
            EngineName = starship.Engine?.Name ?? string.Empty,
            FtlCapable = starship.FtlCapable,
            FtlDriveId = starship.FtlDriveId,
            FtlDriveName = starship.FtlDrive?.Name,
            TotalCrew = starship.TotalCrew,
            TotalCapacity = starship.TotalCapacity,
            IconPictureFilePath = starship.IconPictureFilePath,
            StarshipType = starship.StarshipType
        };

        // Map subtype-specific properties using pattern matching
        switch (starship)
        {
            case PrivateVessel pv:
                response.VesselType = pv.VesselType.ToString();
                break;

            case PublicTransportVessel ptv:
                response.TransportClass = ptv.TransportClass.ToString();
                response.TotalPassengers = ptv.TotalPassengers;
                break;

            case CargoVessel cv:
                response.CargoType = cv.CargoType.ToString();
                response.TotalCargoCapacity = cv.TotalCargoCapacity;
                break;
        }

        return response;
    }
}
