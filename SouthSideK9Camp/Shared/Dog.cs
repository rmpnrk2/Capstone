using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Dog : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; }
    public string AvatarURL { get; set; } = string.Empty;
  
    // Vaccine attributes
    public string VaccineCardURL { get; set; } = string.Empty;
    public string Clinic { get; set; } = string.Empty;
    public bool Rabies { get; set; }
    public bool Distemper { get; set; }
    public bool HepatitisAdenovirus { get; set; }
    public bool Parvovirus { get; set; }
    public bool Parainfluenza { get; set; }

    // Reservation attributes
    public string ReservationPaymentURL { get; set; } = string.Empty;
    public bool ReservationPaymentConfirmed { get; set; }

    // Relationships (Child)
    public Contract Contract { get; set; } = new();
    public  List<ProgressReport> ProgressReports { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = new();


    // Relationships (Parent)
    public int ClientID { get; set; }
    public Client? Client { get; set; }
    public int ReservationID { get; set; }
    public Reservation? Reservation { get; set; }
    public int? ReceiptID { get; set; }
    public Receipt? Receipt { get; set; }
}

public class DogValidator : AbstractValidator<Dog>
{
    public DogValidator()
    {
        RuleFor(d => d.Name).NotEmpty().WithMessage("Dog name field is required").MaximumLength(50).WithMessage("Dog name field should not exceed 50 characters");
        RuleFor(d => d.Breed).NotEmpty().WithMessage("Dog breed field is required").MaximumLength(50).WithMessage("Dog breed field should not exceed 50 characters");
        RuleFor(d => d.Sex).NotEmpty().WithMessage("Dog sex field is required");
        RuleFor(d => d.Birthday).NotEmpty().WithMessage("Dog birthday field is required");

        RuleFor(d => d.Clinic).NotEmpty().WithMessage("Dog clinic field is required").MaximumLength(100).WithMessage("Dog clinic field should not exceed 100 characters");

        RuleFor(d => d.Contract).SetValidator(new ContractValidator());
        RuleForEach(d => d.Invoices).SetValidator(new InvoiceValidator());
    }
}