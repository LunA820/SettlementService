using SettlementService.Data;
using SettlementService.Models;
using System.Collections.Concurrent;

public class BookingRepositoryTests
{
    private readonly BookingRepository _repository;

    public BookingRepositoryTests()
    {
        _repository = new BookingRepository();
    }

    [Fact]
    public async Task GetBookingsByTime_ReturnsCorrectBookings()
    {
        // Arrange
        var bookings = new ConcurrentDictionary<Guid, Booking>();
        var testBookingId1 = Guid.NewGuid();
        var testBookingId2 = Guid.NewGuid();
        var testBookingId3 = Guid.NewGuid();

        bookings.TryAdd(testBookingId1, new Booking { BookingId = testBookingId1, BookingTime = new TimeSpan(10, 30, 0) }); 
        bookings.TryAdd(testBookingId2, new Booking { BookingId = testBookingId2, BookingTime = new TimeSpan(11, 29, 0) }); 
        bookings.TryAdd(testBookingId3, new Booking { BookingId = testBookingId3, BookingTime = new TimeSpan(12, 30, 0) }); 

        var repository = new BookingRepository(); 
        typeof(BookingRepository).GetField("_bookings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(repository, bookings);

        var targetTime = new TimeSpan(11, 00, 0); 

        // Act
        var results = await repository.GetBookingsByTime(targetTime);

        // Assert
        Assert.Contains(results, b => b.BookingId == testBookingId1);
        Assert.Contains(results, b => b.BookingId == testBookingId2);
        Assert.DoesNotContain(results, b => b.BookingId == testBookingId3);
    }
}
