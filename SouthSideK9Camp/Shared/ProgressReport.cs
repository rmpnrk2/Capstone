using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class ProgressReport : BaseModel
{
    [StringLength(200)] public string Subject { get; set; } = string.Empty;
    [StringLength(200)] public string Message { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public DateTime DateTraining { get; set; } = DateTime.UtcNow;
    public TimeSpan SpanDuration { get; set; } = TimeSpan.Zero;
    [Range(0, 10)] public int ScoreFocus { get; set; }
    [Range(0, 10)] public int ScoreObedience { get; set; }
    [Range(0, 10)] public int ScoreProtection { get; set; }

    // Relationships
    public int DogID { get; set; }
    public Dog? Dog { get; set; }
}
