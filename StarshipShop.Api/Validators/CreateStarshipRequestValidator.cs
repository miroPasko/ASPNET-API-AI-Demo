using FluentValidation;
using StarshipShop.Api.Models;
using StarshipShop.Api.Schemas.Requests;

namespace StarshipShop.Api.Validators;

public class CreateStarshipRequestValidator : AbstractValidator<CreateStarshipRequest>
{
    public CreateStarshipRequestValidator()
    {
        RuleFor(x => x.StarshipType)
            .NotEmpty()
            .Must(type => type == "PrivateVessel" || type == "PublicTransportVessel" || type == "CargoVessel")
            .WithMessage("StarshipType must be one of: PrivateVessel, PublicTransportVessel, CargoVessel");

        RuleFor(x => x.FtlDriveId)
            .NotNull()
            .When(x => x.FtlCapable)
            .WithMessage("FtlDriveId is required when FtlCapable is true");

        // PrivateVessel validations
        When(x => x.StarshipType == "PrivateVessel", () =>
        {
            RuleFor(x => x.VesselType)
                .NotEmpty()
                .WithMessage("VesselType is required for PrivateVessel")
                .Must(BeValidVesselType)
                .WithMessage("VesselType must be one of: Fighter, Frigate, SmallCargo, SmallTransport, Custom");
        });

        // PublicTransportVessel validations
        When(x => x.StarshipType == "PublicTransportVessel", () =>
        {
            RuleFor(x => x.TransportClass)
                .NotEmpty()
                .WithMessage("TransportClass is required for PublicTransportVessel")
                .Must(BeValidTransportClass)
                .WithMessage("TransportClass must be one of: Economy, Standard, Business, Luxury");

            RuleFor(x => x.TotalPassengers)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("TotalPassengers must be greater than 0 for PublicTransportVessel");
        });

        // CargoVessel validations
        When(x => x.StarshipType == "CargoVessel", () =>
        {
            RuleFor(x => x.CargoType)
                .NotEmpty()
                .WithMessage("CargoType is required for CargoVessel")
                .Must(BeValidCargoType)
                .WithMessage("CargoType must be one of: Mixed, Liquid, RawMaterials, Vehicles, Hazardous");

            RuleFor(x => x.TotalCargoCapacity)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("TotalCargoCapacity must be greater than 0 for CargoVessel");
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
