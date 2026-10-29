namespace GameCatalog.Presentation.Middleware
{
    public class IPMW
    {
        public readonly RequestDelegate _next;

        public IPMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var blockedIps = new List<string> {};

            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            Console.WriteLine($"Client IP: {clientIp}");
            if (clientIp != null && blockedIps.Contains(clientIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied.");
                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
