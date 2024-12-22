namespace NextStop.Server.DTOs.Stop;

public class StopDto
{
    public required int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string ShortName { get; set; } = string.Empty;
    public required double Latitude { get; set; }
    public required double Longitude { get; set; }
}
