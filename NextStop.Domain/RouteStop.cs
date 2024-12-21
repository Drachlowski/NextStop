namespace NextStop.Domain;

public class RouteStop(int id, int routeId, int stopId, int stopSequence, int scheduled, Route route, Stop stop)
{
    public int Id { get; set; } = id;
    public int RouteId { get; set; } = routeId;
    public int StopId { get; set; } = stopId;
    public int StopSequence { get; set; } = stopSequence;
    public int Scheduled { get; set; } = scheduled;

    public Route Route { get; set; } = route;
    public Stop Stop { get; set; } = stop;
}
