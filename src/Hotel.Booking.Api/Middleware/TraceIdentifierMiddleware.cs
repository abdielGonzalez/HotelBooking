namespace Hotel.Booking.Api.Middleware
{
    public sealed class TraceIdentifierMiddleware
    {
        private const string HeaderName = "X-Trace-Id";
        private readonly RequestDelegate _next;

        public TraceIdentifierMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1️⃣ Tomar traceId entrante o generar uno
            var traceId = context.Request.Headers.TryGetValue(HeaderName, out var value)
                ? value.ToString()
                : context.TraceIdentifier;

            // 2️⃣ Guardarlo en HttpContext
            context.Items["TraceId"] = traceId;

            // 3️⃣ Devolverlo al cliente
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = traceId;
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
