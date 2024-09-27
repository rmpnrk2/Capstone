using FluentValidation;

namespace SouthSideK9Camp.Shared;

public class ReasonForRejection : BaseModel
{
    public string Reason { get; set; } = string.Empty;
}

public class ReasonForRejectionValidator : AbstractValidator<ReasonForRejection>
{
    public ReasonForRejectionValidator()
    {
        RuleFor(r => r.Reason).NotEmpty().WithMessage("This field is required").MaximumLength(400).WithMessage("This field should not exceed 400 characters");
    }
}