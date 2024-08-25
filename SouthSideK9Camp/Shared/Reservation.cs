﻿using FluentValidation;

namespace SouthSideK9Camp.Shared;

public class Reservation : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime? StartingDate { get; set; } = DateTime.Today;
    public DateTime? EndingDate { get; set; } = DateTime.Today;
    public int Slots { get; set; }

    // Relationships
    public List<Dog> Dogs { get; set; } = new();
}
// validation
public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("This field is required").MaximumLength(20).WithMessage("This field should not exceed 10 characters");
        RuleFor(r => r.StartingDate).NotEmpty().WithMessage("This field is required");
        RuleFor(r => r.EndingDate).NotEmpty().WithMessage("This field is required");
        RuleFor(r => r.Slots).NotEmpty().WithMessage("This field is required");
    }
}