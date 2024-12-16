namespace NextStop.Server.DTOs;

public class RouteForUpdateDto
{
    public required int Id { get; set; } // ID hinzugefügt
    public required string RouteName { get; set; } = string.Empty;
    public required DateTime ValidityStartDate { get; set; }
    public DateTime? ValidityEndDate { get; set; }
    public required string DaysOfOperation { get; set; } = string.Empty;
}
