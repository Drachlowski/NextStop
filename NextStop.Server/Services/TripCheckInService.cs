using NextStop.Domain;
using NextStop.Dal.Interface;
using Azure.Core;

namespace NextStop.Server.Services;

public class TripCheckInService : ITripCheckInService
{
    private readonly ITripCheckInDao _tripCheckInDao;
    private readonly ITripDao _tripDao;

    public TripCheckInService(ITripCheckInDao tripCheckInDao, ITripDao tripDao)
    {
        _tripCheckInDao = tripCheckInDao;
        _tripDao = tripDao;
    }

    public async Task AddCheckInAsync(Trip trip, RouteStop routeStop, DateTime checkInTime)
    {

        DateTime scheduled = trip.StartTime.AddMinutes(routeStop.Scheduled);
        trip.CurrentDelay = (int)((checkInTime - scheduled).TotalMinutes);

        var checkIn = new TripCheckIn
        {
            TripId = trip.Id,
            RouteStopId = routeStop.Id,
            CheckInTime = checkInTime,
            CurrentDelay = trip.CurrentDelay
        };
        await _tripCheckInDao.AddCheckInAsync(checkIn);
        await _tripDao.UpdateTripAsync(trip);
    }

    public async Task<IEnumerable<TripCheckIn>> GetCheckInsForTripAsync(int tripId)
    {
        return await _tripCheckInDao.GetCheckInsForTripAsync(tripId);
    }

    public async Task<TripCheckIn?> GetLatestCheckInForTripAsync(int tripId)
    {
        return await _tripCheckInDao.GetLatestCheckInForTripAsync(tripId);
    }
}
