using Microsoft.AspNetCore.Mvc;
using Moq;
using SettlementService.Constant;
using SettlementService.Controllers;
using SettlementService.Models;
using SettlementService.Services;
using YourProject.Common;

public class BookingControllerTests
{
    private readonly Mock<IBookingService> _mockService;
    private readonly BookingController _controller;

    public BookingControllerTests()
    {
        _mockService = new Mock<IBookingService>();
        _controller = new BookingController(_mockService.Object);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenServiceIndicatesFailure()
    {
        // Arrange
        var booking = new Booking { BookingTime = TimeSpan.FromHours(10), Name = "John Doe" };
        _mockService.Setup(s => s.CreateBooking(booking))
                    .ReturnsAsync(new ServiceResponse<Booking> { Success = false, ErrorMessage = "Generic error" });

        // Act
        var result = await _controller.Create(booking);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Generic error", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenBookingIsFullyBooked()
    {
        // Arrange
        var booking = new Booking { BookingTime = TimeSpan.FromHours(11), Name = "Jane Smith" };
        _mockService.Setup(s => s.CreateBooking(booking))
                    .ReturnsAsync(new ServiceResponse<Booking> { Success = false, ErrorMessage = ErrorMessages.FULL_BOOKED });

        // Act
        var result = await _controller.Create(booking);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(ErrorMessages.FULL_BOOKED, conflictResult.Value);
    }

    [Fact]
    public async Task GetBooking_ReturnsNotFound_WhenBookingDoesNotExist()
    {
        // Arrange
        Guid bookingId = Guid.NewGuid();
        _mockService.Setup(s => s.GetBooking(bookingId))
                    .ReturnsAsync(new ServiceResponse<Booking> { Success = false, ErrorMessage = "Not found" });

        // Act
        var result = await _controller.GetBooking(bookingId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetBooking_ReturnsOk_WhenBookingExists()
    {
        // Arrange
        Guid bookingId = Guid.NewGuid();
        var expectedBooking = new Booking { BookingId = bookingId, Name = "Test Name", BookingTime = TimeSpan.FromHours(12) };
        _mockService.Setup(s => s.GetBooking(bookingId))
                    .ReturnsAsync(new ServiceResponse<Booking> { Success = true, Data = expectedBooking });

        // Act
        var result = await _controller.GetBooking(bookingId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var bookingInResponse = Assert.IsType<Booking>(okResult.Value);
        Assert.Equal(expectedBooking.BookingId, bookingInResponse.BookingId);
    }
}
