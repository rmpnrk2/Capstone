using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SouthSideK9Camp.Shared;

public class Client : BaseModel
{
    [StringLength(50)] public string LastName { get; set; } = string.Empty;
    [StringLength(50)] public string FirstName { get; set; } = string.Empty;
    [StringLength(1)] public string MiddleInitial { get; set; } = string.Empty;
    [StringLength(10)] public string Sex { get; set; } = string.Empty;
    [EmailAddress][StringLength(50)] public string Email { get; set; } = string.Empty;
    [StringLength(11)] public string Contact { get; set; } = string.Empty;
    [StringLength(200)] public string Address { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; } = DateTime.Today;


    // Relationships
    public Customer? Customer { get; set; }
    public Member? Member { get; set; }
    public List<Dog> Dogs { get; set; } = new();
}

public class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(c => c.LastName).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(c => c.MiddleInitial).MaximumLength(1).WithMessage("Invalid Initial");
        RuleFor(c => c.Sex).NotEmpty().WithMessage("This field is required");
        RuleFor(c => c.Email).NotEmpty().WithMessage("This field is required").EmailAddress().MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(c => c.Contact).NotEmpty().WithMessage("This field is required").MaximumLength(11).WithMessage("Invalid contact number").MinimumLength(11).WithMessage("Invalid contact number");
        RuleFor(c => c.Address).NotEmpty().WithMessage("This field is required").MaximumLength(200).WithMessage("Should not exceed 200 characters");
        RuleFor(c => c.Birthday).NotEmpty().WithMessage("This field is required");

        RuleFor(c => c.Member).SetValidator(new MemberValidatior()!).When(c => c.Member != null);
        RuleFor(c => c.Customer).SetValidator(new CustomerValidator()!).When(c => c.Customer != null);
        RuleForEach(c => c.Dogs).SetValidator(new DogValidator());
    }
}