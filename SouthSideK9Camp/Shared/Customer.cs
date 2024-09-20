using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Customer : BaseModel
{
    public string WhereWillYouBeStating { get; set; } = string.Empty;
    public string EmergencyVet { get; set; } = string.Empty;
    public string EmergencyVetNumber { get; set; } = string.Empty;
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactNumber { get; set; } = string.Empty;
    public string EmergencyContactEmail { get; set; } = string.Empty;

    // Relationships
    public int ClientID { get; set; }
    public Client? Client { get; set; }
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.WhereWillYouBeStating).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyVet).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyVetNumber).NotEmpty().WithMessage("This field is required").MinimumLength(11).WithMessage("Invalid contact number").MaximumLength(11).WithMessage("Invalid contact number");
        RuleFor(c => c.EmergencyContactName).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(c => c.EmergencyContactNumber).NotEmpty().WithMessage("This field is required").MinimumLength(11).WithMessage("Invalid contact number").MaximumLength(11).WithMessage("Invalid contact number");
        RuleFor(c => c.EmergencyContactEmail).NotEmpty().WithMessage("This field is required").EmailAddress().MaximumLength(50).WithMessage("This field should not exceed 50 characters");
    }
}