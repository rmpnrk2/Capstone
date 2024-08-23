using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Customer : BaseModel
{
    [StringLength(100)] public string WhereWillYouBeStating { get; set; } = string.Empty;
    [StringLength(100)] public string EmergencyVet { get; set; } = string.Empty;
    [StringLength(100)] public string EmergencyVetNumber { get; set; } = string.Empty;
    [StringLength(100)] public string EmergencyContactName { get; set; } = string.Empty;
    [StringLength(100)] public string EmergencyContactNumber { get; set; } = string.Empty;
    [StringLength(100)] public string EmergencyContactEmail { get; set; } = string.Empty;

    // Relationships
    public int ClientID { get; set; }
    [JsonIgnore] public Client? Client { get; set; }
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.WhereWillYouBeStating).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyVet).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyVetNumber).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyContactName).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyContactNumber).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
        RuleFor(c => c.EmergencyContactEmail).NotEmpty().WithMessage("This field is required").MaximumLength(100).WithMessage("This field should not exceed 100 characters");
    }
}