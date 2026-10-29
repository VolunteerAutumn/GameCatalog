namespace GameCatalog.Presentation.Middleware
{
    public class BasicInfoMW
    {
        private readonly RequestDelegate _next;

        public BasicInfoMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var HTTPMethod = context.Request.Method;
            var URL = context.Request.Path;

            await _next(context);

            var statusCode = context.Response.StatusCode;

            Console.WriteLine("================================");
            Console.WriteLine($"HTTP Method: {HTTPMethod}");
            Console.WriteLine($"URL: {URL}");
            Console.WriteLine($"Status Code: {statusCode}");
        }
    }
}
