using System.ComponentModel.DataAnnotations;

namespace SouthSideK9Camp.Shared;

public class Reservation : BaseModel
{
    [StringLength(20)] public string Name { get; set; } = string.Empty;
     public DateTime StartingDate { get; set; } = DateTime.UtcNow;
     public DateTime EndingDate { get; } = DateTime.UtcNow.AddDays(42);
     public int Slots { get; set; }

    // Relationships
    public List<Dog> Dogs { get; set; } = new();
}
