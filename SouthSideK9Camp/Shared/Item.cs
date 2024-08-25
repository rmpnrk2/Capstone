using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Item : BaseModel
{
     public string Name { get; set; } = string.Empty;
     public string MeasurementCount { get; set; } = string.Empty;
     public string MeasurementUnit { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be zero or less")] public int Ammount { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Value cannot be zero or less")] public double Price { get; set; }
    [StringLength(100)] public string Credit { get; set; } = string.Empty;

    // Hidden attributes
    public double Total { get; set; }

    // Methods
    public void CalculateTotal()
    {
        Total = Ammount * Price;
    }

    // Relationships
    public int InvoiceID { get; set; }
    public Invoice? Invoice { get; set; }
}
