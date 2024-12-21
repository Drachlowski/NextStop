namespace NextStop.Domain;

public class Trip(int id, int routeId, TimeSpan startTime, int currentDelay, Route route)
{
    public int Id { get; set; } = id;
    public int RouteId { get; set; } = routeId;
    public TimeSpan StartTime { get; set; } = startTime;
    public int CurrentDelay { get; set; } = currentDelay;

    public Route Route { get; set; } = route;
}
