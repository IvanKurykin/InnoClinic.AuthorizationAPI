using System.Text.RegularExpressions;
using BLL.DTO;
using BLL.Helpers.Constants;
using FluentValidation;

namespace BLL.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .Must(BeValidEmail).WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Please, enter the password")
            .Length(6, 15).WithMessage("Password must be 6-15 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$")
            .WithMessage("Password must contain at least one uppercase, lowercase, digit and special character");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Please, enter the user name")
            .Length(6, 20).WithMessage("User name must be 6-20 characters");

        RuleFor(x => x.ReEnteredPassword)
            .NotEmpty().WithMessage("Please, reenter the password")
            .Must((model, field) => field == model.Password);
    }
    private static bool BeValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        return Regex.IsMatch(email,
            ValidationPatterns.EmailRegex,
            RegexOptions.IgnoreCase);
    }
}
