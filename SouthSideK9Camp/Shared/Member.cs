using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Member : BaseModel
{
    [StringLength(50)] public string Occupation { get; set; } = string.Empty;
    [StringLength(50)] public string WhereDidYouHereAboutUs { get; set; } = string.Empty;
    [StringLength(50)] public string PurposeOfJoining { get; set; } = string.Empty;
    [StringLength(200)] public string DogClinicAddress { get; set; } = string.Empty;
    [StringLength(50)] public string WhoTrainYourDog { get; set; } = string.Empty;


    // Hidden properties
    public string RegistrationPaymentURL { get; set; } = string.Empty;
    public bool RegistrationConfirmed { get; set; }

    // Relationships (Child)
    public List<MembershipDue> MembershipDues { get; set; } = new();

    // Relationships (Parent)
    public int ClientID { get; set; }
    public Client? Client { get; set; }
    public int? ReceiptID { get; set; }
    public Receipt? Receipt { get; set; }
}

public class MemberValidatior : AbstractValidator<Member>
{
    public MemberValidatior()
    {
        RuleFor(m => m.Occupation).NotEmpty().WithMessage("Occupation field is required").MaximumLength(50).WithMessage("Occupation field should not exceed 50 characters");
        RuleFor(m => m.WhereDidYouHereAboutUs).NotEmpty().WithMessage("Where did you here about us question field is required").MaximumLength(50).WithMessage("Where did you here about us question field should not exceed 50 characters");
        RuleFor(m => m.PurposeOfJoining).NotEmpty().WithMessage("Purpose of joining question field is required").MaximumLength(50).WithMessage("Purpose of joining question field should not exceed 50 characters");
        RuleFor(m => m.DogClinicAddress).NotEmpty().WithMessage("Dog's clinic address field is required").MaximumLength(200).WithMessage("Dog's clinic address  field should not exceed 200 characters");
        RuleFor(m => m.WhoTrainYourDog).NotEmpty().WithMessage("Who will train your dog question field is required").MaximumLength(50).WithMessage("Who will train your dog question field should not exceed 50 characters");
    }
}