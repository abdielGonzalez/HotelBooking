using System.Text.Json;

namespace Hotel.Booking.Api.Middleware
{
    public class BadRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public BadRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            // Verifica si la respuesta es un 400 y aún no ha sido escrita
            if (context.Response.StatusCode == StatusCodes.Status400BadRequest && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "Bad Request",
                    status = 400,
                    detail = "Invalid JSON format or missing required fields.",
                    errors = new[]
                    {
                    new
                    {
                        code = "InvalidJson",
                        description = "The request body does not match the expected format.",
                        type = 2
                    }
                }
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
