using SettlementService.Models;
using System.Collections.Concurrent;

namespace SettlementService.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ConcurrentDictionary<Guid, Booking> _bookings = new ConcurrentDictionary<Guid, Booking>();

        public async Task<Booking?> CreateBooking(Booking booking)
        {
            if (_bookings.TryAdd(booking.BookingId, booking))
                return booking;

            return null;
        }

        public async Task<Booking?> GetBooking(Guid bookingId)
        {
            if (_bookings.TryGetValue(bookingId, out Booking booking))
                return booking;

            return null;
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return _bookings.Values.ToList();
        }

        public async Task<List<Booking>> GetBookingsByTime(TimeSpan bookingTime)
        {
            var bookingDuration = new TimeSpan(0, 59, 0);
            var earliestOverlapTime = bookingTime - bookingDuration;
            var latestOverlapTime = bookingTime + bookingDuration;

            var filteredBookings = _bookings.Values
                                            .Where(b => b.BookingTime >= earliestOverlapTime && b.BookingTime <= latestOverlapTime)
                                            .ToList();
            return filteredBookings;
        }
    }
}
