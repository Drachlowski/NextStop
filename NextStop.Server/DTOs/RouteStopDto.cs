using NextStop.Domain;

namespace NextStop.Server.DTOs;

public class RouteStopDto
{
    public required int Id { get; set; }
    public required int RouteId { get; set; }
    public required int StopId { get; set; }
    public required int StopSequence { get; set; }
    public required int Scheduled { get; set; }
    public required Domain.Route Route { get; set; }
    public required Stop Stop { get; set; }
}
