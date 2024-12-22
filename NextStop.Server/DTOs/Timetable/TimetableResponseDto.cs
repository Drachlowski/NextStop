using System.ComponentModel.DataAnnotations;

namespace NextStop.Server.DTOs.Timetable;

public class TimetableResponseDto
{
    public required int TripId { get; set; }
    public required string RouteName { get; set; }
    public required int Delay { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "DepartureTime must be between 1753-01-01 and 9999-12-31.")]
    public required DateTime DepartureTime { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "ArrivalTime must be between 1753-01-01 and 9999-12-31.")]
    public required DateTime ArrivalTime { get; set; }

    public List<TimetableResponseDto>? Connections { get; set; }
}
