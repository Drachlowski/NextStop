using NextStop.Domain;

namespace NextStop.Server.DTOs;

public class StopForCreationDto
{

    public required int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string ShortName { get; set; } = string.Empty;
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}
