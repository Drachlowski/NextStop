namespace NextStop.Domain;

public class Trip(int id, int routeId, DateTime startTime, int currentDelay, Route route)
{
    public int Id { get; set; } = id;
    public int RouteId { get; set; } = routeId;
    public DateTime StartTime { get; set; } = startTime;
    public int CurrentDelay { get; set; } = currentDelay;

    public Route Route { get; set; } = route;
}
