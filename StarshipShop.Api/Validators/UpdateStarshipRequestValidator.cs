using FluentValidation;
using StarshipShop.Api.Models;
using StarshipShop.Api.Schemas.Requests;

namespace StarshipShop.Api.Validators;

public class UpdateStarshipRequestValidator : AbstractValidator<UpdateStarshipRequest>
{
    public UpdateStarshipRequestValidator()
    {
        // Validate enum parsing for PrivateVessel fields
        When(x => x.VesselType != null, () =>
        {
            RuleFor(x => x.VesselType)
                .Must(BeValidVesselType)
                .WithMessage("VesselType must be one of: Fighter, Frigate, SmallCargo, SmallTransport, Custom");
        });

        // Validate enum parsing for PublicTransportVessel fields
        When(x => x.TransportClass != null, () =>
        {
            RuleFor(x => x.TransportClass)
                .Must(BeValidTransportClass)
                .WithMessage("TransportClass must be one of: Economy, Standard, Business, Luxury");
        });

        // Validate enum parsing for CargoVessel fields
        When(x => x.CargoType != null, () =>
        {
            RuleFor(x => x.CargoType)
                .Must(BeValidCargoType)
                .WithMessage("CargoType must be one of: Mixed, Liquid, RawMaterials, Vehicles, Hazardous");
        });

        // Validate numeric ranges for subtype fields
        When(x => x.TotalPassengers.HasValue, () =>
        {
            RuleFor(x => x.TotalPassengers)
                .GreaterThan(0)
                .WithMessage("TotalPassengers must be greater than 0");
        });

        When(x => x.TotalCargoCapacity.HasValue, () =>
        {
            RuleFor(x => x.TotalCargoCapacity)
                .GreaterThan(0)
                .WithMessage("TotalCargoCapacity must be greater than 0");
        });
    }

    private bool BeValidVesselType(string? vesselType)
    {
        if (string.IsNullOrEmpty(vesselType))
            return false;
        return Enum.TryParse<VesselType>(vesselType, true, out _);
    }

    private bool BeValidTransportClass(string? transportClass)
    {
        if (string.IsNullOrEmpty(transportClass))
            return false;
        return Enum.TryParse<TransportClass>(transportClass, true, out _);
    }

    private bool BeValidCargoType(string? cargoType)
    {
        if (string.IsNullOrEmpty(cargoType))
            return false;
        return Enum.TryParse<CargoType>(cargoType, true, out _);
    }
}
