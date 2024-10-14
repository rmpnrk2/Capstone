using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class Log : BaseModel
{
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
}
