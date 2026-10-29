namespace GameCatalog.Presentation.Middleware
{
    public class ExceptionHandleMW
    {
        private readonly RequestDelegate _next;
        public ExceptionHandleMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    message = "An unexpected error occurred.",
                    details = ex.Message
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
