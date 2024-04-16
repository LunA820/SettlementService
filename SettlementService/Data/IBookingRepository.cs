using SettlementService.Models;

namespace SettlementService.Data
{
    public interface IBookingRepository
    {
        public Task<Booking?> CreateBooking(Booking booking);
        public Task<Booking> GetBooking(Guid bookingId);
        public Task<List<Booking>> GetAllBookings();
        public Task<List<Booking>> GetBookingsByTime(TimeSpan bookingTime);
    }
}
