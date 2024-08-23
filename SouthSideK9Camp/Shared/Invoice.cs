using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Invoice : BaseModel
{
    [StringLength(50)] public int Title { get; set; }
    [StringLength(50)] public string CompanyName { get; set; } = string.Empty;
    [StringLength(50)] public string CompanyAddress { get; set; } = string.Empty;
    [StringLength(10)] public string CompanyZIPCode { get; set; } = string.Empty;
    [StringLength(13, MinimumLength = 13)] public string CompanyPhone { get; set; } = string.Empty;
    [StringLength(50)] public string AccountName { get; set; } = string.Empty;
    [StringLength(50)] public string AccountAddress { get; set; } = string.Empty;

    // Hidden attributes
    public DateTime DateSent { get; set; } = new();
    public DateTime DateDue { get; set; } = new();

    public double Balance { get; set; }
    public bool IsEmailed { get; set; }
    public string ProofOfPaymentURL { get; set; } = string.Empty;
    public bool PaymentConfirmed { get; set; }

    // Methods
    public void CalculateBalance()
    {
        Balance = Items.Sum(item => item.Total);
    }

    // Relationships
    public List<Item> Items { get; set; } = new();

    public int DogID { get; set; }
    [JsonIgnore] public Dog? Dog { get; set; }
}
