using FluentValidation;

namespace SouthSideK9Camp.Shared;

public class Reservation : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime? StartingDate { get; set; }
    public DateTime? EndingDate { get; set; }
    public int Slots { get; set; } = 8;

    // Relationships
    public List<Dog> Dogs { get; set; } = new();
}
// validation
public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(r => r.StartingDate).NotEmpty().WithMessage("This field is required");
        RuleFor(r => r.Slots).NotEmpty().WithMessage("This field is required").GreaterThan(0).WithMessage("This field sould be a posive number");
    }
}