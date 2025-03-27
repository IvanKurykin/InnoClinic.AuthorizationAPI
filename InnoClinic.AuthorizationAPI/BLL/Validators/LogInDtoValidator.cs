using BLL.DTO;
using FluentValidation;

namespace BLL.Validators;

public class LoginDTOValidator : AbstractValidator<LogInDto>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.Password)
             .NotEmpty().WithMessage("Please, enter the password");
    }
}
