using BLL.Models.DTOs;
using FluentValidation;

namespace BLL.Models.Validators;

public class LoginDTOValidator : AbstractValidator<LogInDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Please, enter the password")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(15).WithMessage("Password must not exceed 15 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
}
