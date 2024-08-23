using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Log : BaseModel
{
    [StringLength(50)] public string Subject { get; set; } = string.Empty;
    [StringLength(50)] public string Message { get; set; } = string.Empty;
    [StringLength(10)] public string Severity { get; set; } = string.Empty;
}
