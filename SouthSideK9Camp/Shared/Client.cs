using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace SouthSideK9Camp.Shared;

public class Client : BaseModel
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleInitial { get; set; } = string.Empty;
    public string Suffix { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; }


    // Relationships
    public Customer? Customer { get; set; }
    public Member? Member { get; set; }
    public List<Dog> Dogs { get; set; } = new();
}

public class ClientValidator : AbstractValidator<Client>
{
    public ClientValidator()
    {
        RuleFor(c => c.LastName).NotEmpty().WithMessage("Last name field is required").MaximumLength(50).WithMessage("Last name field should not exceed 50 characters");
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("First name field is required").MaximumLength(50).WithMessage("First name field should not exceed 50 characters");
        RuleFor(c => c.MiddleInitial).MaximumLength(20).WithMessage("Middle inititial field should not exceed 20 characters");
        RuleFor(c => c.Suffix).MaximumLength(20).WithMessage("Suffix field should not exceed 20 characters");
        RuleFor(c => c.Sex).NotEmpty().WithMessage("Sex field is required");
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email field is required").EmailAddress().MaximumLength(50).WithMessage("Enail field should not exceed 50 characters");
        RuleFor(c => c.Contact).NotEmpty().WithMessage("Contact field is required").MaximumLength(10).WithMessage("Invalid contact number").MinimumLength(10).WithMessage("Invalid contact number");
        RuleFor(c => c.Address).NotEmpty().WithMessage("Address field is required").MaximumLength(200).WithMessage("Address field should not exceed 200 characters");
        RuleFor(c => c.Birthday).NotEmpty().WithMessage("Birthday field is required");

        RuleFor(c => c.Member).SetValidator(new MemberValidatior()!).When(c => c.Member != null);
        RuleFor(c => c.Customer).SetValidator(new CustomerValidator()!).When(c => c.Customer != null);
        RuleForEach(c => c.Dogs).SetValidator(new DogValidator());
    }
}