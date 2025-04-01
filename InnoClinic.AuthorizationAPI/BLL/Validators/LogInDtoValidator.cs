using BLL.DTO;
using BLL.Helpers;
using FluentValidation;

namespace BLL.Validators;

public class LoginDtoValidator : AbstractValidator<LogInDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .Must(ValidateEmail.BeValidEmail).WithMessage("Invalid email format");

        RuleFor(x => x.Password)
             .NotEmpty().WithMessage("Please, enter the password");
    }
}
