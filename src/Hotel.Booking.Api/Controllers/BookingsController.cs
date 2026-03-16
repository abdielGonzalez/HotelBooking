using Hotel.Booking.Api.Controllers;
using Hotel.Booking.Domain.Entities;
using Hotel.Booking.Application.Interfaces;
using Hotel.Booking.Domain.Common;
using MediatR;
using Hotel.Booking.Api.Extensions;
using Hotel.Booking.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hotel.Booking.Api.Controllers
{
    [ApiController]
    [Route("api/v1/bookings")]
    public class BookingsController : ApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly CardsBdContext _db;

        public BookingsController(IUnitOfWork uow, CardsBdContext db)
        {
            _uow = uow;
            _db = db;
        }

        private IMediator GetMediator() => HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator ?? throw new InvalidOperationException();

        // GET /api/v1/bookings/availability?hotelId=1&from=2026-01-01&to=2026-01-05&guests=2
        [HttpGet("availability")]
        public async Task<IActionResult> Availability([FromQuery] int hotelId, [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int guests)
        {
            if (from.Date >= to.Date)
                return BadRequest("check-in must be before check-out");

            if ((to - from).TotalDays > 30)
                return BadRequest("range exceeds maximum allowed nights");

            var roomTypes = _db.RoomTypes!.Where(rt => rt.HotelId == hotelId && rt.IsActive).ToList();
            var results = new List<object>();

            foreach (var rt in roomTypes)
            {
                if (rt.Capacity < guests) continue;

                var inventories = _db.RoomInventories!
                    .Where(i => i.RoomTypeId == rt.RoomTypeId && i.Date >= from.Date && i.Date < to.Date)
                    .ToList();

                if (inventories.Count != (to - from).Days)
                    continue; // missing inventory

                var minAvailable = inventories.Min(i => i.Available);
                if (minAvailable <= 0) continue;

                var rate = _db.RatePlans!.Where(rp => rp.RoomTypeId == rt.RoomTypeId).FirstOrDefault();

                results.Add(new
                {
                    rt.RoomTypeId,
                    rt.Name,
                    rt.Capacity,
                    Available = minAvailable,
                    PricePerNight = rate?.PricePerNight ?? 0
                });
            }

            return Ok(results);
        }

        // POST /api/v1/bookings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {
            var mediator = HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator ?? throw new InvalidOperationException();
            var idempotencyKey = Request.Headers.ContainsKey("Idempotency-Key") ? Request.Headers["Idempotency-Key"].FirstOrDefault() : null;
            var cmd = new Hotel.Booking.Application.Features.Bookings.Commands.CreateBookingCommand(request.HotelId, request.RoomTypeId, request.GuestId, request.CheckIn, request.CheckOut, request.Quantity, idempotencyKey);
            var result = await mediator.Send(cmd);
            if (!result.IsSuccess) return this.ProblemFromResult(result);

            // After success, try to fetch the booking to return full represention
            var booking = await _db.Bookings!.FindAsync(result.Value);
            return CreatedAtAction(nameof(Get), new { id = result.Value }, booking);
        }

        public sealed record CreateBookingRequest(int HotelId, int RoomTypeId, int GuestId, DateTime CheckIn, DateTime CheckOut, int Quantity);

        // GET /api/v1/bookings?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _db.Bookings!.AsQueryable();
            var total = await query.CountAsync();
            var items = await query.OrderByDescending(b => b.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                data = items,
                pageNumber,
                pageSize,
                totalRecords = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        // GET /api/v1/bookings/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _db.Bookings!.FindAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // POST /api/v1/bookings/{id}/confirm
        [HttpPost("{id:int}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var booking = await _db.Bookings!.FindAsync(id);
            if (booking == null) return NotFound();

            await _uow.BeginTransactionAsync();
            try
            {
                booking.Confirm();
                _db.Bookings!.Update(booking);
                await _uow.CommitAsync();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                return FromResult(Result.Failure(Error.Problem("Booking.ConfirmFailed", ex.Message)));
            }
        }

        // POST /api/v1/bookings/{id}/cancel
        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _db.Bookings!.FindAsync(id);
            if (booking == null) return NotFound();

            await _uow.BeginTransactionAsync();
            try
            {
                booking.Cancel();
                _db.Bookings!.Update(booking);
                await _uow.CommitAsync();
                return Ok(booking);
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                return FromResult(Result.Failure(Error.Problem("Booking.CancelFailed", ex.Message)));
            }
        }
    }
}
