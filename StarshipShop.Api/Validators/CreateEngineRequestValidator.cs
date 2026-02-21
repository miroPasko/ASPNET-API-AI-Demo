using FluentValidation;
using StarshipShop.Api.Schemas.Requests;

namespace StarshipShop.Api.Validators;

public class CreateEngineRequestValidator : AbstractValidator<CreateEngineRequest>
{
    public CreateEngineRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.MaximumSpeed)
            .GreaterThan(0)
            .WithMessage("MaximumSpeed must be greater than 0");

        RuleFor(x => x.FuelUsage)
            .GreaterThan(0)
            .WithMessage("FuelUsage must be greater than 0");

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}
