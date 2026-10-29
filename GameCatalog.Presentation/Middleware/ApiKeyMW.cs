namespace GameCatalog.Presentation.Middleware;

public class ApiKeyMW
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMW(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;

        // Registration and login must be accessible without an API key.
        // 1. Registration and login must be accessible without an API key.
        // 2. Email links clicked from a browser must also bypass the API key.
        var isAuthEndpoint =
            HttpMethods.IsPost(method) &&
            (path.Equals("/api/Auth/register", StringComparison.OrdinalIgnoreCase) ||
             path.Equals("/api/Auth/login", StringComparison.OrdinalIgnoreCase) ||
             path.Equals("/api/Auth/forgot-password", StringComparison.OrdinalIgnoreCase)) ||
            (HttpMethods.IsGet(method) &&
             (path.Equals("/api/Auth/confirm-email", StringComparison.OrdinalIgnoreCase) ||
              path.Equals("/api/Auth/confirm-email-change", StringComparison.OrdinalIgnoreCase) ||
              path.Equals("/api/Auth/reset-password", StringComparison.OrdinalIgnoreCase)));

        if (isAuthEndpoint)
        {
            await _next(context);
            return;
        }

        var configuredApiKey = _configuration["ApiKey"];

        if (string.IsNullOrWhiteSpace(configuredApiKey))
        {
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "API key is not configured."
            });

            return;
        }

        if (!context.Request.Headers.TryGetValue(
                "X-API-KEY", out var providedApiKey) ||
            providedApiKey.Count != 1 ||
            !string.Equals(
                providedApiKey.ToString(),
                configuredApiKey,
                StringComparison.Ordinal))
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Invalid or missing API key."
            });

            return;
        }

        await _next(context);
    }
}
