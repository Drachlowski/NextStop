using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;
using System.Data;

namespace NextStop.Dal.Ado;

public class AdoTripCheckInDao(IConnectionFactory connectionFactory, string tripCheckInTableName) : ITripCheckInDao
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly AdoTemplate _template = new AdoTemplate(connectionFactory);
    private readonly string tripCheckInTableName = tripCheckInTableName;

    private TripCheckIn MapRowToTripCheckIn(IDataRecord row) => new TripCheckIn
    {
        Id = (int)row["Id"],
        TripId = (int)row["TripId"],
        RouteStopId = (int)row["RouteStopId"],
        CheckInTime = (DateTime)row["CheckInTime"],
        CurrentDelay = (int)row["CurrentDelay"]
    };

    public async Task AddCheckInAsync(TripCheckIn checkIn)
    {
        await _template.ExecuteAsync(
            $"INSERT INTO {tripCheckInTableName} (TripId, RouteStopId, CheckInTime, CurrentDelay) VALUES (@tripId, @RouteStopId, @checkInTime, @currentDelay)",
            new QueryParameter("tripId", checkIn.TripId),
            new QueryParameter("RouteStopId", checkIn.RouteStopId),
            new QueryParameter("checkInTime", checkIn.CheckInTime),
            new QueryParameter("currentDelay", checkIn.CurrentDelay)
        );
    }

    public async Task<IEnumerable<TripCheckIn>> GetCheckInsForTripAsync(int tripId)
    {
        return await _template.QueryAsync(
            $"SELECT * FROM {tripCheckInTableName} WHERE TripId = @tripId ORDER BY CheckInTime ASC",
            MapRowToTripCheckIn,
            new QueryParameter("tripId", tripId)
        );
    }

    public async Task<TripCheckIn?> GetLatestCheckInForTripAsync(int tripId)
    {
        return await _template.QuerySingleAsync(
            $"SELECT TOP 1 * FROM {tripCheckInTableName} WHERE TripId = @tripId ORDER BY CheckInTime DESC",
            MapRowToTripCheckIn,
            new QueryParameter("tripId", tripId)
        );
    }
}
