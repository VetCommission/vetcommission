using FluentValidation;

namespace VetCommission.Application.Features.Professionals;

public sealed class CreateProfessionalValidator : AbstractValidator<CreateProfessionalCommand>
{
    public CreateProfessionalValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Role).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email)).MaximumLength(254);
    }
}

public sealed class UpdateProfessionalValidator : AbstractValidator<UpdateProfessionalCommand>
{
    public UpdateProfessionalValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Role).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email)).MaximumLength(254);
    }
}
