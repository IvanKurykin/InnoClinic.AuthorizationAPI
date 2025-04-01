using BLL.DTO;
using BLL.Validators;
using FluentValidation.TestHelper;

namespace UnitTests
{
    public class RegisterDtoValidatorTests
    {
        private readonly RegisterDtoValidator _validator = new();

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("invalid", false)]
        [InlineData("test@example.com", true)]
        public void EmailValidation(string email, bool expectedValid)
        {
            var model = new RegisterDto { Email = email };
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
                Password = "P@ssword1",
                ReEnteredPassword = "Different1!"
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ReEnteredPassword);
        }
    }
}