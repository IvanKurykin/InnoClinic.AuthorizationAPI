using BLL.DTO;
using BLL.Validators;
using DAL.Constants;
using FluentValidation.TestHelper;

namespace UnitTests
{
    public class LoginDtoValidatorTests
    {
        private readonly LoginDtoValidator _validator = new();

        [Theory]
        [InlineData("", false)]
        [InlineData("invalid", false)]
        [InlineData("test@example.com", true)]
        [InlineData("user@domain.com", true)]
        public void EmailValidation(string email, bool expectedValid)
        {
            var model = new LogInDto { Email = email, Password = TestConstans.TestUserPassword };
            var result = _validator.TestValidate(model);

            if (expectedValid)
                result.ShouldNotHaveValidationErrorFor(x => x.Email);
            else
                result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("password", true)] 
        public void PasswordValidation(string password, bool expectedValid)
        {
            var model = new LogInDto { Email = TestConstans.TestUserEmail, Password = password };
            var result = _validator.TestValidate(model);

            if (expectedValid)
                result.ShouldNotHaveValidationErrorFor(x => x.Password);
            else
                result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}