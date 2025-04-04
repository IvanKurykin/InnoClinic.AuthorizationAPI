using BLL.DTO;
using BLL.Validators;
using DAL.Constants;
using FluentValidation.TestHelper;

namespace UnitTests
{
    public class RegisterDtoValidatorTests
    {
        private readonly RegisterDtoValidator _validator = new();

        [Theory]
        [InlineData("", false)]
        [InlineData("invalid", false)]
        [InlineData("test@example.com", true)]
        public void EmailValidation(string email, bool expectedValid)
        {
            var model = new RegisterDto { Email = email, Role = Roles.Patient};
            var result = _validator.TestValidate(model);

            if (expectedValid)
                result.ShouldNotHaveValidationErrorFor(x => x.Email);
            else
                result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void PasswordMismatch()
        {
            var model = new RegisterDto
            {
                Password = TestConstans.TestUserPassword,
                ReEnteredPassword = "Different1!"
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ReEnteredPassword);
        }
    }
}