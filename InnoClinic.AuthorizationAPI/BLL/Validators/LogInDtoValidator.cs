using System.Text.RegularExpressions;
using BLL.DTO;
using BLL.Helpers.Constants;
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
    private static bool BeValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        
        return Regex.IsMatch(email,
            ValidationPatterns.EmailRegex,
            RegexOptions.IgnoreCase);
    }
}
