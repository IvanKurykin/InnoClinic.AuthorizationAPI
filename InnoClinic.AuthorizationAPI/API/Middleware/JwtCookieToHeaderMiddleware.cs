namespace API.Middleware;

public class JwtCookieToHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public JwtCookieToHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue("jwt", out var token))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }

        await _next(context);
    }
}
