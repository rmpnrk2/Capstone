using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Contract : BaseModel
{
    public string TrainingType { get; set; } = string.Empty;
    public string TrainingGoal { get; set; } = string.Empty;
    public string Restrictions { get; set; } = string.Empty;
    public string FeedingRoutine { get; set; } = string.Empty;
    public string SleepingRoutine { get; set; } = string.Empty;
    public bool ProtectiveOverToys { get; set; }
    public bool ProtectiveOverFood { get; set; }
    public bool ProtectiveOverTreats { get; set; }
    public bool ProtectiveOverSpot { get; set; }
    public bool ProtectiveOverPerson { get; set; }
    public string ProtectiveOverOther { get; set; } = string.Empty;
    public bool DiscomfortOverPaws { get; set; }
    public bool DiscomfortOverTails { get; set; }
    public bool DiscomfortOverEars { get; set; }
    public bool DiscomfortOverMuzzle { get; set; }
    public bool DiscomfortOverHead { get; set; }
    public bool DiscomfortOverRump { get; set; }
    public string DiscomfortOverOther { get; set; } = string.Empty;
    public bool FearOrAggressionToHuman { get; set; }
    public bool FearOrAggressionToDogs { get; set; }
    public string AnythingElseToShare { get; set; } = string.Empty;

    // Hidden attributes
    public string ReservationPaymentURL { get; set; } = string.Empty;
    public bool PaymendConfirmed { get; set; }

    // Relationships
    public int DogID { get; set; }
    public Dog? Dog { get; set; }
}

public class ContractValidator : AbstractValidator<Contract>
{
    public ContractValidator()
    {
        RuleFor(d => d.TrainingType).NotEmpty().WithMessage("Training type field is required").MaximumLength(50).WithMessage("Training type field should not exceed 50 characters");
        RuleFor(d => d.TrainingGoal).NotEmpty().WithMessage("Training goal field is required").MaximumLength(200).WithMessage("Training goal field should not exceed 200 characters");
        RuleFor(d => d.Restrictions).NotEmpty().WithMessage("Dog restrictions field is required").MaximumLength(200).WithMessage("Dog restrictions field should not exceed 200 characters");
        RuleFor(d => d.FeedingRoutine).NotEmpty().WithMessage("Dog feeding routine field is required").MaximumLength(200).WithMessage("Dog feeding routine field should not exceed 200 characters");
        RuleFor(d => d.SleepingRoutine).NotEmpty().WithMessage("Dog sleeping routine field is required").MaximumLength(200).WithMessage("Dog sleeping routine  field should not exceed 200 characters");

        RuleFor(d => d.ProtectiveOverOther).MaximumLength(200).WithMessage("Dog protective behavior field should not exceed 200 characters");
        RuleFor(d => d.DiscomfortOverOther).MaximumLength(200).WithMessage("Dog discomfort behavior field should not exceed 200 characters");
        RuleFor(d => d.AnythingElseToShare).MaximumLength(200).WithMessage("Anything else to share field should not exceed 200 characters");
    }
}