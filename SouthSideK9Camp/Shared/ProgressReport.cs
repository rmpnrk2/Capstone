using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class ProgressReport : BaseModel
{
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public DateTime? DateTraining { get; set; }
    public TimeSpan? SpanDuration { get; set; }
    public int ScoreFocus { get; set; }
    public int ScoreObedience { get; set; }
    public int ScoreProtection { get; set; }

    // Relationships
    public int DogID { get; set; }
    public Dog? Dog { get; set; }
}

public class ProgressReportValidator : AbstractValidator<ProgressReport>
{
    public ProgressReportValidator()
    {
        RuleFor(d => d.Subject).NotEmpty().WithMessage("This field is required").MaximumLength(200).WithMessage("This field should not exceed 200 characters");
        RuleFor(d => d.Message).NotEmpty().WithMessage("This field is required").MaximumLength(500).WithMessage("This field should not exceed 500 characters");
        RuleFor(d => d.DateTraining).NotEmpty().WithMessage("This field is required");
        RuleFor(d => d.SpanDuration).NotEmpty().WithMessage("This field is required");
    }
}
