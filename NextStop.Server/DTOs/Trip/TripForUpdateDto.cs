using System.ComponentModel.DataAnnotations;

namespace NextStop.Server.DTOs.Trip;

public class TripForUpdateDto
{
    public required int Id { get; set; }
    public required int RouteId { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "StartTime must be between 1753-01-01 and 9999-12-31.")]

    public required DateTime StartTime { get; set; }
    public required int CurrentDelay { get; set; }

    public Domain.Route? Route { get; set; }
}