using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SouthSideK9Camp.Shared;

public class User : BaseModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(c => c.Username).NotEmpty().WithMessage("This field is required").MaximumLength(20).WithMessage("This field should not exceed 20 characters");
        RuleFor(c => c.Password).NotEmpty().WithMessage("This field is required").MaximumLength(20).WithMessage("This field should not exceed 20 characters");
        RuleFor(c => c.Email).EmailAddress().WithMessage("Invalid email address").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(c => c.FullName).NotEmpty().WithMessage("This field is required").MaximumLength(50).WithMessage("This field should not exceed 50 characters");
        RuleFor(c => c.Contact).NotEmpty().WithMessage("This field is required").MaximumLength(11).WithMessage("Invalid contact number").MinimumLength(11).WithMessage("Invalid contact number");
        RuleFor(c => c.Role).NotEmpty().WithMessage("This field is required");
    }
}