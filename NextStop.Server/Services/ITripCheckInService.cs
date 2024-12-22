using NextStop.Dal.Interface;
using NextStop.Domain;

namespace NextStop.Server.Services;

public interface ITripCheckInService
{
    Task AddCheckInAsync(Trip trip, RouteStop routeStop, DateTime checkInTime);

    Task<IEnumerable<TripCheckIn>> GetCheckInsForTripAsync(int tripId);

    Task<TripCheckIn?> GetLatestCheckInForTripAsync(int tripId);
}
