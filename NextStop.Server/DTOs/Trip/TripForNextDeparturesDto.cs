using NextStop.Domain;

namespace NextStop.Server.DTOs.Trip;

public class TripForNextDeparturesDto(Domain.Trip trip, Domain.RouteStop routeStop)
{
    public int Id { get; set; } = trip.Id;
    public int RouteId { get; set; } = trip.RouteId;

    public DateTime DepartureTime { get; set; } = trip.StartTime.AddMinutes(routeStop.Scheduled);
    public int CurrentDelay { get; set; } = trip.CurrentDelay;
    public Domain.RouteStop RouteStop = routeStop;
}
