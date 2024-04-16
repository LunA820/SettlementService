using SettlementService.Data;
using SettlementService.Models;
using SettlementService.Constant;
using YourProject.Common;

namespace SettlementService.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<ServiceResponse<Booking>> CreateBooking(Booking booking)
        {
            if(string.IsNullOrEmpty(booking.Name))
                return getServiceResponse(null, false, ErrorMessages.NAME_MISSING);

            if (booking.BookingTime > new TimeSpan(16, 00, 0) || booking.BookingTime < new TimeSpan(9, 0, 0))
                return getServiceResponse(null, false, ErrorMessages.AFTER_OFFICE_HOUR);

            var existingBookings = await _bookingRepository.GetBookingsByTime(booking.BookingTime);
            if (existingBookings.Count >= 4)
                return getServiceResponse(null, false, ErrorMessages.FULL_BOOKED);
        
            booking.BookingId = Guid.NewGuid();
            var newBooking = await _bookingRepository.CreateBooking(booking);
            var errMessage = (newBooking == null) ? ErrorMessages.ERROR : string.Empty;

            return getServiceResponse(newBooking, newBooking != null, errMessage);
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return await _bookingRepository.GetAllBookings();
        }

        public async Task<ServiceResponse<Booking>> GetBooking(Guid bookingId)
        {
            var booking = await _bookingRepository.GetBooking(bookingId);

            return new ServiceResponse<Booking>
            {
                Data = booking,
                Success = (booking != null),
                ErrorMessage = (booking == null) ? ErrorMessages.BOOKING_NOT_FOUND : string.Empty
            };
        }

        private ServiceResponse<Booking> getServiceResponse(Booking booking, bool success, string err)
        {
            return new ServiceResponse<Booking>
            {
                Data = booking,
                Success = success,
                ErrorMessage = err
            };
        }
    }
}
