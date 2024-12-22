using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;
using System.Data;

namespace NextStop.Dal.Ado;

public class AdoStatisticDao(IConnectionFactory connectionFactory, string tripCheckInTableName, string tripTableName, string routeStopTableName, string routeTableName) : IStatisticDao
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly AdoTemplate _template = new AdoTemplate(connectionFactory);
    private readonly string tripCheckInTableName = tripCheckInTableName;
    private readonly string tripTableName = tripTableName;
    private readonly string routeStopTableName = routeStopTableName;
    private readonly string routeTableName = routeTableName;

    private DelayStatistic MapRowToTripCheckIn(IDataRecord row) => new()
    {
        RouteId = (int)row["RouteId"],
        RouteName = (string)row["RouteName"],
        AverageDelay = (double)row["AverageDelay"],
        OnTimePercentage = (double)row["OnTimePercentage"],
        SlightlyDelayedPercentage = (double)row["SlightlyDelayedPercentage"],
        DelayedPercentage = (double)row["DelayedPercentage"],
        HeavilyDelayedPercentage = (double)row["HeavilyDelayedPercentage"]
    };

    public async Task<IEnumerable<DelayStatistic>> GetRouteDelayStatisticsAsync(DateTime startDate, DateTime endDate, int? routeId = null)
    {
        if (routeId is not null)
        {
            return await _template.QueryAsync(
                $"""
                SELECT
                    Trip.RouteId,
                    RouteName,
                    AVG(CAST(TripCheckIn.CurrentDelay AS FLOAT)) AS AverageDelay, -- Umwandlung in FLOAT
                    SUM(CASE WHEN TripCheckIn.CurrentDelay <= 2 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS OnTimePercentage,
                    SUM(CASE WHEN TripCheckIn.CurrentDelay > 2 AND TripCheckIn.CurrentDelay <= 5 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS SlightlyDelayedPercentage,
                    SUM(CASE WHEN TripCheckIn.CurrentDelay > 5 AND TripCheckIn.CurrentDelay <= 10 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS DelayedPercentage,
                    SUM(CASE WHEN TripCheckIn.CurrentDelay > 10 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS HeavilyDelayedPercentage
                    FROM {tripCheckInTableName} TripCheckIn
                    JOIN {routeStopTableName} RouteStop on RouteStop.Id = TripCheckIn.RouteStopId
                    JOIN {routeTableName} Route ON RouteStop.RouteId = Route.Id
                    JOIN {tripTableName} Trip ON TripCheckIn.TripId = Trip.Id AND Trip.StartTime BETWEEN @startDate AND @endDate
                    WHERE Trip.RouteId = @routeId
                    GROUP BY Trip.RouteId, RouteName;
                """, MapRowToTripCheckIn,
                new QueryParameter("routeId", routeId),
                new QueryParameter("startDate", startDate),
                new QueryParameter("endDate", endDate)
                );
        }
        return await _template.QueryAsync($"""
            SELECT
                    Trip.RouteId,
                    RouteName,
                    AVG(CAST(TripCheckIn.CurrentDelay AS FLOAT)) AS AverageDelay, -- Umwandlung in FLOAT
                    SUM(CASE WHEN TripCheckIn.CurrentDelay <= 2 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS OnTimePercentage,
                    SUM(CASE WHEN TripCheckIn.CurrentDelay > 2 AND TripCheckIn.CurrentDelay <= 5 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS SlightlyDelayedPercentage,
                    SUM(CASE WHEN TripCheckIn.CurrentDelay > 5 AND TripCheckIn.CurrentDelay <= 10 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS DelayedPercentage,
                    SUM(CASE WHEN TripCheckIn.CurrentDelay > 10 THEN 1 ELSE 0 END) * 100.0 / CAST(COUNT(*) AS FLOAT) AS HeavilyDelayedPercentage
                    FROM {tripCheckInTableName} TripCheckIn
                    JOIN {routeStopTableName} RouteStop on RouteStop.Id = TripCheckIn.RouteStopId
                    JOIN {routeTableName} Route ON RouteStop.RouteId = Route.Id
                    JOIN {tripTableName} Trip ON TripCheckIn.TripId = Trip.Id AND Trip.StartTime BETWEEN @startDate AND @endDate
                    GROUP BY Trip.RouteId, RouteName;
            """, MapRowToTripCheckIn,
                new QueryParameter("startDate", startDate),
                new QueryParameter("endDate", endDate));
    }
}
