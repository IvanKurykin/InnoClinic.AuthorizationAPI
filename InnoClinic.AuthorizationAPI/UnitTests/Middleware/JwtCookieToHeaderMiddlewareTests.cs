using API.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Middleware
{
    public class JwtCookieToHeaderMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsyncAddsAuthorizationHeaderWhenJwtCookieExists()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Cookie = "jwt=test-token"; 

            var nextMock = new Mock<RequestDelegate>();
            var middleware = new JwtCookieToHeaderMiddleware(nextMock.Object);

            await middleware.InvokeAsync(context);

            context.Request.Headers.Should().ContainKey("Authorization");
            context.Request.Headers.Authorization.Should().AllBe("Bearer test-token");
            nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsyncDoesNotAddAuthorizationHeaderWhenJwtCookieIsMissing()
        {
            var context = new DefaultHttpContext();
            var nextMock = new Mock<RequestDelegate>();
            var middleware = new JwtCookieToHeaderMiddleware(nextMock.Object);

            await middleware.InvokeAsync(context);

            context.Request.Headers.Authorization.Should().BeNullOrEmpty();
            nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsyncCallsNextMiddleware()
        {
            var context = new DefaultHttpContext();
            var nextMock = new Mock<RequestDelegate>();

            var middleware = new JwtCookieToHeaderMiddleware(nextMock.Object);

            await middleware.InvokeAsync(context);

            nextMock.Verify(next => next(context), Times.Once);
        }
    }
}
