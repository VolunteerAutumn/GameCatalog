namespace GameCatalog.Presentation.Middleware
{
    public class RequestCounterMW
    {
        private readonly RequestDelegate _next;
        private static int _requestCount = 0;

        public RequestCounterMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _requestCount++;
            Console.WriteLine($"Request #{_requestCount}: {context.Request.Method} {context.Request.Path}");
            await _next(context);
        }
    }
}
