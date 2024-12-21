using NextStop.Domain;

namespace NextStop.Server.DTOs;

public class RouteStopForUpdateDto
{
    public required int Id { get; set; }
    public required int RouteId { get; set; }
    public required int StopId { get; set; }
    public required int StopSequence { get; set; }
    public required int Scheduled { get; set; }
    public Domain.Route? Route { get; set; }
    public Stop? Stop { get; set; }
}
