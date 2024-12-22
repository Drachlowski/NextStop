namespace NextStop.Domain;

public class TripCheckIn
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public int RouteStopId { get; set; }
    public DateTime CheckInTime { get; set; }
    public int CurrentDelay { get; set; }
}
