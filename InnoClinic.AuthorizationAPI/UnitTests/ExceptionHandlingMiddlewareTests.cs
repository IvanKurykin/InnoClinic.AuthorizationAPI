using API.Controllers;
using API.Middleware;
using BLL.Exceptions;
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

        [Fact]
        public async Task InvokeAsyncReturns401()
        {
            var context = new DefaultHttpContext();

            await _middleware.InvokeAsync(context, _ => throw new UserNotFoundException());

            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }
    }
}