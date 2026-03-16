using Hotel.Booking.Infrastructure.Database;
using Hotel.Booking.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Booking.Api.Middleware;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;

    public IdempotencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, CardsBdContext db)
    {
        if (!context.Request.Headers.TryGetValue("Idempotency-Key", out var values))
        {
            await _next(context);
            return;
        }

        var key = values.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(key))
        {
            await _next(context);
            return;
        }

        // Only handle POST/PUT/DELETE for idempotency
        if (!(HttpMethods.IsPost(context.Request.Method) || HttpMethods.IsPut(context.Request.Method) || HttpMethods.IsDelete(context.Request.Method)))
        {
            await _next(context);
            return;
        }

        var now = DateTime.UtcNow;
        var existing = await db.IdempotencyRecords!.FirstOrDefaultAsync(r => r.Key == key && r.ExpiresAt > now);
        if (existing != null)
        {
            // Return stored response exactly
            context.Response.StatusCode = existing.ResponseStatus;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(existing.ResponseBody);
            return;
        }

        // No saving here — handlers are responsible for persisting the idempotent response.
        await _next(context);
    }
}