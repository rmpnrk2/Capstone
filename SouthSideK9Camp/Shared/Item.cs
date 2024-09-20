using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Item : BaseModel
{
    public DateTime? Date { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Credit { get; set; } = string.Empty;

    // Hidden attributes
    public double Total { get; set; }
    public bool isModel { get; set; }

    // Methods
    public void CalculateTotal()
    {
        Total = Quantity * Price;
    }

    // Relationships
    public int? InvoiceID { get; set; }
    public Invoice? Invoice { get; set; }
}

public class ItemValidator : AbstractValidator<Item>
{
    public ItemValidator()
    {
        RuleFor(i => i.Name).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(i => i.Description).MaximumLength(200).WithMessage("This field should not exceed 200 characters");
        RuleFor(i => i.Quantity).NotEmpty().WithMessage("This field is required").GreaterThan(0).WithMessage("Invalid value");
        RuleFor(i => i.Unit).NotEmpty().WithMessage("This field is required").MaximumLength(10).WithMessage("This field should not exceed 10 characters");
        RuleFor(i => i.Price).NotEmpty().WithMessage("This field is required").GreaterThan(0).WithMessage("Invalid value");
        RuleFor(i => i.Credit).MaximumLength(100).WithMessage("This field should not exceed 100 characters");
    }
}
