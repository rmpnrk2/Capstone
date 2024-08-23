using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SouthSideK9Camp.Shared;

public class EmailValidation
{
    public string EmailAddress { get; set; } = string.Empty;
}

public class EmailValidationValidator : AbstractValidator<EmailValidation>
{
    public EmailValidationValidator()
    {
        RuleFor(e => e.EmailAddress).NotEmpty().WithMessage("This field is required").MaximumLength(50)
        .WithMessage("This field should not exceed 50 characters").EmailAddress().WithMessage("Invalid email address");
    }
}
