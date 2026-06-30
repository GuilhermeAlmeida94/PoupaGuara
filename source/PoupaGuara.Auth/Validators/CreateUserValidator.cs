using FluentValidation;
using PoupaGuara.Auth.Models;

namespace PoupaGuara.Auth.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode("name-null")
            .MaximumLength(100).WithErrorCode("name-too-long");

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("email-null")
            .EmailAddress().WithErrorCode("email-invalid-pattern");

        RuleFor(x => x.BirthDate)
            .NotNull().WithErrorCode("birthday-null")
            .Must(d => d < DateOnly.FromDateTime(DateTime.Today))
                .WithErrorCode("birthday-invalid")
                // ApplyConditionTo.CurrentValidator scopes When only to this Must,
                // keeping birthday-null active when BirthDate is absent.
                .When(x => x.BirthDate is not null, ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("password-null")
            .MinimumLength(8).WithErrorCode("password-too-short")
                .When(x => !string.IsNullOrEmpty(x.Password), ApplyConditionTo.CurrentValidator);
    }
}
