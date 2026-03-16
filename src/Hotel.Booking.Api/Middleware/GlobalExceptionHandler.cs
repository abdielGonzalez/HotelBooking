using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Hotel.Booking.Api.Middleware
{
    internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // If the exception is a BadHttpRequestException with an inner JsonException,
            // handle it specifically.
            if (exception is BadHttpRequestException && exception.InnerException is JsonException jsonEx)
            {
                await HandleJsonExceptionAsync(context, jsonEx, cancellationToken);
                return true;
            }

            if (exception is BadHttpRequestException)
            {
                await HandleGenericBadRequest(context, cancellationToken);
                return true;
            }
            var traceId = context.Items["TraceId"]?.ToString();

            if (exception is ValidationException validationException)
            {
                await HandleValidationExceptionAsync(context, validationException, cancellationToken);
                return true;
            }

            // Log unhandled exceptions.
            logger.LogError(exception, "Unhandled exception occurred | TraceId: {TraceId}", traceId);

            // Write a generic server error response.
            await WriteServerErrorResponseAsync(context, cancellationToken);
            return true;
        }

        private static async Task HandleGenericBadRequest(HttpContext context, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "The request could not be processed because it contains invalid or missing data. Please review the request format and ensure all required parameters are provided correctly.",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        private static async Task HandleJsonExceptionAsync(HttpContext context, JsonException jsonEx, CancellationToken cancellationToken)
        {
            var problemDetails = CreateJsonProblemDetails(context, jsonEx);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        private static async Task HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException exception,
            CancellationToken cancellationToken)
        {
            var errors = exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            var problemDetails = new ValidationProblemDetails(errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }



        private static ProblemDetails CreateJsonProblemDetails(HttpContext context, JsonException jsonEx)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = jsonEx.Message,
                Instance = context.Request.Path
            };

            // Optionally add extra information if available.
            if (jsonEx.LineNumber.HasValue)
            {
                problemDetails.Extensions["lineNumber"] = jsonEx.LineNumber.Value;
            }

            if (jsonEx.BytePositionInLine.HasValue)
            {
                problemDetails.Extensions["bytePositionInLine"] = jsonEx.BytePositionInLine.Value;
            }

            if (!string.IsNullOrEmpty(jsonEx.Path))
            {
                problemDetails.Extensions["path"] = jsonEx.Path;
            }

            return problemDetails;
        }

        private static async Task WriteServerErrorResponseAsync(HttpContext context, CancellationToken cancellationToken)
        {
            var traceId = context.Items["TraceId"]?.ToString();

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Title = "Server failure"
            };
            problemDetails.Extensions["identifier"] = traceId;

            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
    }
}
