using SettlementService.Models;
using YourProject.Common;

namespace SettlementService.Services
{
    public interface IBookingService
    {
        Task<ServiceResponse<Booking>> CreateBooking(Booking booking);
        Task<ServiceResponse<Booking>> GetBooking(Guid bookingId);
        Task<List<Booking>> GetAllBookings();
    }
}
