using API.Controllers;
using API.Middleware;
using BLL.Exceptions;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<AuthController>> _loggerMock = new();
        private readonly ExceptionHandlingMiddleware _middleware;

        public ExceptionHandlingMiddlewareTests()
        {
            _middleware = new ExceptionHandlingMiddleware(_loggerMock.Object);
        }

        [Fact]
        public async Task InvokeAsyncReturns400WithDetails()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var validationException = new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Email", "Invalid email"),
                new FluentValidation.Results.ValidationFailure("Password", "Too short")
            });

            await _middleware.InvokeAsync(context, _ => throw validationException);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();

            Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
            Assert.Contains("Validation failed", responseBody);
            Assert.Contains("Invalid email", responseBody);
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException), StatusCodes.Status400BadRequest)]
        [InlineData(typeof(InvalidOperationException), StatusCodes.Status400BadRequest)]
        [InlineData(typeof(UserNotFoundException), StatusCodes.Status401Unauthorized)]
        [InlineData(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public async Task InvokeAsyncExceptionHandlingReturnsCorrectStatusCode(Type exceptionType, int expectedStatusCode)
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var exception = (Exception)Activator.CreateInstance(exceptionType, "Test message")!;

            await _middleware.InvokeAsync(context, _ => throw exception);

            context.Response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}