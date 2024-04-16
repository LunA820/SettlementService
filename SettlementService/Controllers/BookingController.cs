using Microsoft.AspNetCore.Mvc;
using SettlementService.Constant;
using SettlementService.Models;
using SettlementService.Services;

namespace SettlementService.Controllers
{
    [ApiController]
    [Route("SettlementService/Booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Booking booking)
        {
            var response = await _bookingService.CreateBooking(booking);

            if (!response.Success)
            {
                switch (response.ErrorMessage)
                {
                    case ErrorMessages.FULL_BOOKED:
                        return Conflict(response.ErrorMessage);
                   
                    default:
                        return BadRequest(response.ErrorMessage);
                }
            }

            return Ok(new { bookingId = response.Data.BookingId });
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBooking(Guid bookingId)
        {
            var response = await _bookingService.GetBooking(bookingId);
            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }

            return Ok(response.Data);
        }


    }
}
