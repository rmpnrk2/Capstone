using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Contract : BaseModel
{
    [StringLength(50)] public string TrainingType { get; set; } = string.Empty;
    [StringLength(200)] public string TrainingGoal { get; set; } = string.Empty;
    [StringLength(200)] public string Restrictions { get; set; } = string.Empty;
    [StringLength(200)] public string FeedingRoutine { get; set; } = string.Empty;
    [StringLength(200)] public string SleepingRoutine { get; set; } = string.Empty;
    public bool ProtectiveOverToys { get; set; }
    public bool ProtectiveOverFood { get; set; }
    public bool ProtectiveOverTreats { get; set; }
    public bool ProtectiveOverSpot { get; set; }
    public bool ProtectiveOverPerson { get; set; }
    [StringLength(50)] public string ProtectiveOverOther { get; set; } = string.Empty;
    public bool DiscomfortOverPaws { get; set; }
    public bool DiscomfortOverTails { get; set; }
    public bool DiscomfortOverEars { get; set; }
    public bool DiscomfortOverMuzzle { get; set; }
    public bool DiscomfortOverHead { get; set; }
    public bool DiscomfortOverRump { get; set; }
    [StringLength(50)] public string DiscomfortOverOther { get; set; } = string.Empty;
    public bool FearOrAggressionToHuman { get; set; }
    public bool FearOrAggressionToDogs { get; set; }
    [StringLength(300)] public string AnythingElseToShare { get; set; } = string.Empty;

    // Hidden attributes
    public string ReservationPaymentURL { get; set; } = string.Empty;
    public bool PaymendConfirmed { get; set; }

    // Relationships
    public int DogID { get; set; }
    [JsonIgnore] public Dog? Dog { get; set; }
}
