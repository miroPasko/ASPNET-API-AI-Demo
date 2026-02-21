using FluentValidation;
using StarshipShop.Api.Schemas.Requests;

namespace StarshipShop.Api.Validators;

public class UpdateFtlDriveRequestValidator : AbstractValidator<UpdateFtlDriveRequest>
{
    public UpdateFtlDriveRequestValidator()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(1000);
        });

        When(x => x.MaximumSpeed.HasValue, () =>
        {
            RuleFor(x => x.MaximumSpeed)
                .GreaterThan(0)
                .WithMessage("MaximumSpeed must be greater than 0");
        });

        When(x => x.FuelUsage.HasValue, () =>
        {
            RuleFor(x => x.FuelUsage)
                .GreaterThan(0)
                .WithMessage("FuelUsage must be greater than 0");
        });

        When(x => x.Manufacturer != null, () =>
        {
            RuleFor(x => x.Manufacturer)
                .NotEmpty()
                .MaximumLength(200);
        });

        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");
        });
    }
}
