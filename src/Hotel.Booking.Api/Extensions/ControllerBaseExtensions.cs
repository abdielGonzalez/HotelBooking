using Hotel.Booking.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Booking.Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult ProblemFromResult(
            this ControllerBase controller,
            Result result)
        {
            var error = result.Error;

            // ✅ DECLARACIÓN CORRECTA
            var extensions = new Dictionary<string, object?>();



            return error.Type switch
            {
                ErrorType.Validation => controller.Problem(
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title: "Bad Request",
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: error.Description
                ),

                ErrorType.NotFound => controller.Problem(
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    title: error.Code,
                    statusCode: StatusCodes.Status404NotFound,
                    detail: error.Description
                ),

                ErrorType.Conflict => controller.Problem(
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    title: error.Code,
                    statusCode: StatusCodes.Status409Conflict,
                    detail: error.Description
                ),

                ErrorType.UnprocessableEntity => controller.Problem(
                    type: "https://tools.ietf.org/html/rfc4918#section-11.2",
                    title: error.Code,
                    statusCode: StatusCodes.Status422UnprocessableEntity,
                    detail: error.Description
                ),

                ErrorType.Problem => controller.Problem(
                    type: "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title: error.Code,
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: error.Description
                ),

                ErrorType.Failure or _ => controller.Problem(
                    type: "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title: "Internal Server Error",
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: error.Description
                )
            };
        }
    }
}