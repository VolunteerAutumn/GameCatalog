namespace GameCatalog.Presentation.Middleware
{
    public class TimestampMW
    {
        private readonly RequestDelegate _next;

        public TimestampMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestTime = DateTime.UtcNow;
            var requestLog = $"Request received at {requestTime:O}";

            await _next(context);

            var responseTime = DateTime.UtcNow;
            var responseLog = $"Response sent at {responseTime:O}";

            var duration = responseTime - requestTime;
            var result = $"Request processed in {duration.TotalMilliseconds} ms";

            Console.WriteLine("================================");
            Console.WriteLine(requestLog);
            Console.WriteLine(responseLog);
            Console.WriteLine(result);
        }
    }
}
