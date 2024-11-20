using System.ComponentModel.DataAnnotations;

namespace SouthSideK9Camp.Shared;

public class Receipt : BaseModel
{
    public int receiptType { get; set; } // 0-Reservation Fee, 1-Statement of Account, 2-Membership Reservation, 3-Membership Due
    public double Balance { get; set; }


    // Relationships (Child)
    public Dog? Dog { get; set; }
    public Invoice? Invoice { get; set; }
    public Member? Member { get; set; }
    public MembershipDue? MembershipDue { get; set; }
}
