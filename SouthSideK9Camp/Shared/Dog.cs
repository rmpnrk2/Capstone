using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Dog : BaseModel
{
    [StringLength(50)] public string Name { get; set; } = string.Empty;
    [StringLength(50)] public string Breed { get; set; } = string.Empty;
    [StringLength(10)] public string Sex { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; } = DateTime.UtcNow;
    public string AvatarURL { get; set; } = string.Empty;
  
    // Vaccine attributes
    public string VaccineCardURL { get; set; } = string.Empty;
    [StringLength(100)] public string Clinic { get; set; } = string.Empty;
    public bool Rabies { get; set; }
    public bool Distemper { get; set; }
    public bool HepatitisAdenovirus { get; set; }
    public bool Parvovirus { get; set; }
    public bool Parainfluenza { get; set; }

    // Reservation attributes
    public string ReservationPaymentURL { get; set; } = string.Empty;
    public bool ReservationPaymentConfirmed { get; set; }

    // Relationships
    public Contract Contract { get; set; } = new();
    public ProgressReport ProgressReport { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = new();

    public int ClientID { get; set; }
    [JsonIgnore] public Client? Client { get; set; }
    public int ReservationID { get; set; }
    [JsonIgnore] public Reservation? Reservation { get; set; }
}

public class DogValidator : AbstractValidator<Dog>
{
    public DogValidator()
    {
        RuleFor(d => d.Name).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(d => d.Breed).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(d => d.Sex).NotEmpty().WithMessage("This field is required");
        RuleFor(d => d.Birthday).NotEmpty().WithMessage("This field is required");
    }
}