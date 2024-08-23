using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class User : BaseModel
{
    [StringLength(50)] public string Username { get; set; } = string.Empty;
    [JsonIgnore] public string Password { get; set; } = string.Empty;
    [EmailAddress] public string Email { get; set; } = string.Empty;
    [StringLength(50)] public string FullName { get; set; } = string.Empty;
    [StringLength(13, MinimumLength = 13)] public string Contact { get; set; } = string.Empty;
    [StringLength(10)] public string Role { get; set; } = string.Empty;
}
