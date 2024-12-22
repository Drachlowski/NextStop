using NextStop.Domain;
using NextStop.Server.DTOs.Timetable;

namespace NextStop.Server.Services;

public interface ITripService
{
    Task<Trip?> GetTripByIdAsync(int id);
    Task<IEnumerable<Trip>> GetAllTripsAsync();
    Task AddTripAsync(Trip trip);
    Task UpdateTripAsync(Trip trip);
    Task DeleteTripAsync(int id);
    Task<bool> TripExistsAsync(int id);
    Task<IEnumerable<Trip>> GetTripsByRouteIdAsync(int routeId);
    Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date);
    Task<IEnumerable<Trip>> GetTripsWithCurrentDelayAsync();
    Task<IEnumerable<Trip>> GetTripsByTimeRangeAsync(DateTime startTime, DateTime endTime);
    Task<IEnumerable<Trip>> GetDelayedTripsByRouteIdAsync(int routeId);
    Task<double> GetAverageDelayForRouteAsync(int routeId);
    Task<IEnumerable<Trip>> GetActiveTripsAsync(DateTime currentTime);
    Task<IEnumerable<Trip>> GetTripsPaginatedAsync(int page, int pageSize);
    Task<IEnumerable<(Trip Trip, RouteStop RouteStop)>> GetNextDeparturesForStopAsync(int stopId, DateTime currentTime, int limit = 5);
    Task<IEnumerable<TimetableResponseDto>> GetTimetableAsync(int startStopId, int endStopId, DateTime? departureTime);
}