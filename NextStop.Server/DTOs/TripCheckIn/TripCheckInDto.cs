using System.ComponentModel.DataAnnotations;

namespace NextStop.Server.DTOs.TripCheckIn;

public class TripCheckInDto
{
    public required int TripId { get; set; }
    public required int RouteStopId { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "CheckInTime must be between 1753-01-01 and 9999-12-31.")]

    public required DateTime CheckInTime { get; set; }
}
