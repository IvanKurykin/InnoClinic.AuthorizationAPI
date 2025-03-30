using System.Text.RegularExpressions;
using BLL.DTO;
using FluentValidation;

namespace BLL.Validators;

public class LoginDtoValidator : AbstractValidator<LogInDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .Must(BeValidEmail).WithMessage("Invalid email format");

        RuleFor(x => x.Password)
             .NotEmpty().WithMessage("Please, enter the password");
    }
    private bool BeValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }
}
