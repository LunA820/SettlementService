# SettlementService

## Overview

This repository contains the source code for the Booking API, designed to manage booking times within a business context. The API ensures bookings are within office hours and checks for booking conflicts.

## Technology Stack

- .NET 8.0
- ASP.NET Core
- xUnit for testing
- Moq for mocking in tests

## Getting Started

Use VisualStudio or any IDE to run the project. The app will listen to https://localhost:7026.
- Create new booking: https://localhost:7026/SettlementService/Booking
- Get all bookings: https://localhost:7025/SettlementService/Booking/
- Get booking by BookingId: https://localhost:7025/SettlementService/Booking/{bookingId}

Here's an example body (raw json) to POST new booking:
{
 "bookingTime": "11:30",
  "name":"Luna"
}

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- An IDE like [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Testing

If you cannot run the auto test, try to add Project reference of SettlementServive to SettlementServiveTest, then run again.