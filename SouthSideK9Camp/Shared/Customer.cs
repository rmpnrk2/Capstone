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
        RuleFor(c => c.WhereWillYouBeStating).NotEmpty().WithMessage("Where will you be staying field is required").MaximumLength(100).WithMessage("Where will you be staying Where will you be staying field should not exceed 100 characters");
        RuleFor(c => c.EmergencyVet).NotEmpty().WithMessage("Emergency veterianian field is required").MaximumLength(100).WithMessage("Emergency veterianian field should not exceed 100 characters");
        RuleFor(c => c.EmergencyVetNumber).NotEmpty().WithMessage("Emergency veterianian contact number field is required").MinimumLength(11).WithMessage("Invalid contact number").MaximumLength(11).WithMessage("Invalid contact number");
        RuleFor(c => c.EmergencyContactName).NotEmpty().WithMessage("Emergency contact name field is required").MaximumLength(50).WithMessage("Emergency contact name field should not exceed 50 characters");
        RuleFor(c => c.EmergencyContactNumber).NotEmpty().WithMessage("Emergency contact field is required").MinimumLength(11).WithMessage("Invalid contact number").MaximumLength(11).WithMessage("Invalid contact number");
        RuleFor(c => c.EmergencyContactEmail).EmailAddress().WithMessage("Emergency contact email is not a valid email address.").NotEmpty().WithMessage("Emergency contact email field is required").EmailAddress().MaximumLength(50).WithMessage("Emergency contact email field should not exceed 50 characters");
    }
}