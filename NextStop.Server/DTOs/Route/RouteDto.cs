using NextStop.Domain;

namespace NextStop.Server.DTOs.Route;

public class RouteDto
{
    public required int Id { get; set; }
    public required string RouteName { get; set; } = string.Empty;
    public required DateTime ValidityStartDate { get; set; }
    public DateTime? ValidityEndDate { get; set; }
    public required string DaysOfOperation { get; set; } = string.Empty;
}
