using Hotel.Booking.Api.Extensions;
using Hotel.Booking.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Booking.Api.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult FromResult(Result result)
            => this.ProblemFromResult(result);
    }
}
