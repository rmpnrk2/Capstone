using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Invoice : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public string CompanyZIPCode { get; set; } = string.Empty;
    public string CompanyPhone { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountAddress { get; set; } = string.Empty;
    public DateTime? DateDue { get; set; }

    // Hidden attributes
    public DateTime? DateSent { get; set; }

    public double Balance { get; set; }
    public bool IsEmailed { get; set; }
    public string ProofOfPaymentURL { get; set; } = string.Empty;
    public bool PaymentConfirmed { get; set; }

    public bool isDefault { get; set; }

    // Methods
    public void CalculateBalance()
    {
        Balance = Items.Sum(i => i.Total);
    }

    // Relationships (Child)
    public List<Item> Items { get; set; } = new();

    // Relationships (Parent)
    public int DogID { get; set; }
    public Dog? Dog { get; set; }
    public int? ReceiptID { get; set; }
    public Receipt? Receipt { get; set; }
}

public class InvoiceValidator : AbstractValidator<Invoice>
{
    public InvoiceValidator()
    {
        RuleFor(i => i.Title).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(i => i.CompanyName).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(i => i.CompanyAddress).NotEmpty().WithMessage("This field is required").MaximumLength(200).WithMessage("This field should not exceed 200 characters");
        RuleFor(i => i.CompanyZIPCode).NotEmpty().WithMessage("This field is required").MaximumLength(4).WithMessage("Invalid ZIP code").MinimumLength(4).WithMessage("Invalid ZIP code");
        RuleFor(i => i.CompanyPhone).NotEmpty().WithMessage("This field is required").MaximumLength(11).WithMessage("Invalid phone number").MinimumLength(11).WithMessage("Invalid phone number");
        RuleFor(i => i.CompanyZIPCode).NotEmpty().WithMessage("This field is required").MaximumLength(200).WithMessage("This field should not exceed 200 characters");
        RuleFor(i => i.AccountName).NotEmpty().WithMessage("This field is required").MaximumLength(200).WithMessage("This field should not exceed 200 characters");
        RuleFor(i => i.AccountAddress).NotEmpty().WithMessage("This field is required").MaximumLength(200).WithMessage("This field should not exceed 200 characters");
        RuleFor(i => i.AccountAddress).NotEmpty().WithMessage("This field is required");

        RuleForEach(i => i.Items).SetValidator(new ItemValidator());
    }
}