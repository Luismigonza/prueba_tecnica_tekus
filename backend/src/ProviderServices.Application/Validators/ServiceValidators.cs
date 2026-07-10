using FluentValidation;
using ProviderServices.Application.DTOs;

namespace ProviderServices.Application.Validators;

public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.HourlyRateUsd).GreaterThan(0);
    }
}