using Xunit;
using Moq;
using SettlementService.Data;
using SettlementService.Models;
using SettlementService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourProject.Common;
using SettlementService.Constant;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockRepo;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _mockRepo = new Mock<IBookingRepository>();
        _service = new BookingService(_mockRepo.Object);
    }

    [Theory]
    [InlineData(16, 1)]
    [InlineData(8, 59)]
    public async Task CreateBooking_ReturnsError_WhenTimeIsAfterOfficeHours(int hour, int min)
    {
        // Arrange
        var booking = new Booking { Name = "Luna", BookingTime = new TimeSpan(hour, min, 0) };

        // Act & Assert
        var result = await _service.CreateBooking(booking);

        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.AFTER_OFFICE_HOUR, result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_ReturnsError_WhenBookingIsFullyBooked()
    {
        // Arrange
        var bookingTime = new TimeSpan(10, 00, 0);
        var booking = new Booking { Name = "Luna", BookingTime = bookingTime };
        var existingBookings = new List<Booking> { new Booking(), new Booking(), new Booking(), new Booking() };

        _mockRepo.Setup(repo => repo.GetBookingsByTime(bookingTime))
                 .ReturnsAsync(existingBookings);

        // Act & Assert
        var result = await _service.CreateBooking(booking);

        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.FULL_BOOKED, result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_ReturnsError_WhenNameIsEmptyString()
    {
        // Arrange
        var bookingTime = new TimeSpan(10, 00, 0);
        var booking = new Booking { Name = string.Empty, BookingTime = bookingTime };
        var existingBookings = new List<Booking> { new Booking(), new Booking(), new Booking(), new Booking() };

        _mockRepo.Setup(repo => repo.GetBookingsByTime(bookingTime))
                 .ReturnsAsync(existingBookings);

        // Act & Assert
        var result = await _service.CreateBooking(booking);

        Assert.False(result.Success);
        Assert.Equal(ErrorMessages.NAME_MISSING, result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_CreatesSuccessfully_WhenNotFullyBooked()
    {
        // Arrange
        var bookingTime = new TimeSpan(11, 00, 0);
        var booking = new Booking { Name = "Luna", BookingTime = bookingTime };
        var existingBookings = new List<Booking> { new Booking(), new Booking() };

        _mockRepo.Setup(repo => repo.GetBookingsByTime(bookingTime))
                 .ReturnsAsync(existingBookings);
        _mockRepo.Setup(repo => repo.CreateBooking(It.IsAny<Booking>()))
                 .ReturnsAsync((Booking b) => b);

        // Act & Assert
        var result = await _service.CreateBooking(booking);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }
}
