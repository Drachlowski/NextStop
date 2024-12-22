using NextStop.Domain;

namespace NextStop.Dal.Interface;

public interface ITripCheckInDao
{
    Task AddCheckInAsync(TripCheckIn checkIn);
    Task<IEnumerable<TripCheckIn>> GetCheckInsForTripAsync(int tripId);
    Task<TripCheckIn?> GetLatestCheckInForTripAsync(int tripId);
}
