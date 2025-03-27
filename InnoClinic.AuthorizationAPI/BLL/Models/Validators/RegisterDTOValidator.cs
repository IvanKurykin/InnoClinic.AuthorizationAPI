using BLL.Models.DTOs;
using FluentValidation;

namespace BLL.Models.Validators;

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Please, enter the password")
            .Length(6, 15).WithMessage("Password must be 6-15 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$")
            .WithMessage("Password must contain at least one uppercase, lowercase, digit and special character");

        RuleFor(x => x.ReEnteredPassword)
            .NotEmpty().WithMessage("Please, reenter the password")
            .Equal(x => x.Password).WithMessage("The passwords you've entered don't coincide");
    }
}
