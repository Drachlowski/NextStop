using NextStop.Dal.Interface;
using NextStop.Domain;
using NextStop.Server.DTOs.Timetable;
using NextStop.Server.Mappers;

namespace NextStop.Server.Services;

public class TripService : ITripService
{
    private readonly ITripDao _tripDao;
    private readonly IRouteStopDao _routeStopDao;
    private readonly IRouteDao _routeDao;

    public TripService(ITripDao tripDao, IRouteStopDao routeStopDao, IRouteDao routeDao)
    {
        _tripDao = tripDao;
        _routeStopDao = routeStopDao;
        _routeDao = routeDao;
    }

    public async Task<Trip?> GetTripByIdAsync(int id)
    {
        return await _tripDao.GetTripByIdAsync(id);
    }

    public async Task<IEnumerable<Trip>> GetAllTripsAsync()
    {
        return await _tripDao.GetAllTripsAsync();
    }

    public async Task AddTripAsync(Trip trip)
    {
        await _tripDao.AddTripAsync(trip);
    }

    public async Task UpdateTripAsync(Trip trip)
    {
        await _tripDao.UpdateTripAsync(trip);
    }

    public async Task DeleteTripAsync(int id)
    {
        await _tripDao.DeleteTripAsync(id);
    }

    public async Task<IEnumerable<Trip>> GetTripsByRouteIdAsync(int routeId)
    {
        return await _tripDao.GetTripsByRouteIdAsync(routeId);
    }

    public async Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date)
    {
        return await _tripDao.GetTripsByDateAsync(date);
    }

    public async Task<IEnumerable<Trip>> GetTripsWithCurrentDelayAsync()
    {
        return await _tripDao.GetTripsWithCurrentDelayAsync();
    }

    public async Task<IEnumerable<Trip>> GetTripsByTimeRangeAsync(DateTime startTime, DateTime endTime)
    {
        return await _tripDao.GetTripsByTimeRangeAsync(startTime, endTime);
    }

    public async Task<IEnumerable<Trip>> GetDelayedTripsByRouteIdAsync(int routeId)
    {
        return await _tripDao.GetDelayedTripsByRouteIdAsync(routeId);
    }

    public async Task<double> GetAverageDelayForRouteAsync(int routeId)
    {
        return await _tripDao.GetAverageDelayForRouteAsync(routeId);
    }

    public async Task<IEnumerable<Trip>> GetActiveTripsAsync(DateTime currentTime)
    {
        return await _tripDao.GetActiveTripsAsync(currentTime);
    }

    public async Task<IEnumerable<Trip>> GetTripsPaginatedAsync(int page, int pageSize)
    {
        return await _tripDao.GetTripsPaginatedAsync(page, pageSize);
    }

    public async Task<bool> TripExistsAsync(int id)
    {
        return await _tripDao.GetTripByIdAsync(id) is not null;
    }

    public async Task<IEnumerable<(Trip Trip, RouteStop RouteStop)>> GetNextDeparturesForStopAsync(int stopId, DateTime currentTime, int limit = 5)
    {
        return await _tripDao.GetNextTripsForStopAsync(stopId, currentTime, limit);
    }
    
    public async Task<IEnumerable<TimetableResponseDto>> GetTimetableAsync(int startStopId, int endStopId, DateTime? departureTime)
    {
        departureTime ??= DateTime.Now;

        var startRouteStops = await _routeStopDao.GetAllRouteStopsAsync();
        var endRouteStops = await _routeStopDao.GetAllRouteStopsAsync();

        var matchingRoutes = startRouteStops
            .Where(start => start.StopId == startStopId)
            .Join(
                endRouteStops.Where(end => end.StopId == endStopId),
                start => start.RouteId,
                end => end.RouteId,
                (start, end) => new { RouteId = start.RouteId, Start = start, End = end }
            )
            .Where(route => route.Start.StopSequence < route.End.StopSequence);

        var results = new List<TimetableResponseDto>();

        foreach (var routeInfo in matchingRoutes)
        {
            var route = await _routeDao.GetRouteByIdAsync(routeInfo.RouteId);
            if (route == null) continue;

            var trips = await _tripDao.GetTripsByRouteIdAsync(routeInfo.RouteId);

            foreach (var trip in trips)
            {
                var departureTimeForStop = trip.StartTime.AddMinutes(routeInfo.Start.Scheduled);
                if (departureTimeForStop < departureTime.Value) continue;

                var arrivalTimeForStop = trip.StartTime.AddMinutes(routeInfo.End.Scheduled);

                results.Add(trip.ToTimetableResponseDto(route, departureTimeForStop, arrivalTimeForStop));
            }
        }

        return results.OrderBy(r => r.DepartureTime);
    }
}
