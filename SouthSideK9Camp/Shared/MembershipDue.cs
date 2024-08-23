using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class MembershipDue : BaseModel
{
    public string ProofOfPaymentURL { get; set; } = string.Empty;
    public bool PaymentConfirmed { get; set; }
    public DateTime DateTimeDue { get; set; } = DateTime.UtcNow.AddDays(30);

    // Relationships
    public int MemberID { get; set; }
    [JsonIgnore] public Member? Member { get; set; }
}
