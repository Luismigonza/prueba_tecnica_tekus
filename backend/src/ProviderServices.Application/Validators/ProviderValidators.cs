using FluentValidation;
using ProviderServices.Application.DTOs;

namespace ProviderServices.Application.Validators;

public class CreateProviderRequestValidator : AbstractValidator<CreateProviderRequest>
{
    public CreateProviderRequestValidator()
    {
        RuleFor(x => x.Nit).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Website).NotEmpty().Must(BeAValidUrl).WithMessage("Website must be a valid URL.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
    }

    private static bool BeAValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
}

public class UpdateProviderRequestValidator : AbstractValidator<UpdateProviderRequest>
{
    public UpdateProviderRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Website).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
    }
}