using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Dal.Ado;

public class AdoTripDao(IConnectionFactory connectionFactory, string tripTableName, string routeTableName, string stopTableName, string routeStopTableName) : ITripDao
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly AdoTemplate _template = new AdoTemplate(connectionFactory);
    private readonly string tripTableName = tripTableName;
    private readonly string routeTableName = routeTableName;
    private readonly string stopTableName = stopTableName;
    private readonly string routeStopTableName = routeStopTableName;

    private Trip MapRowToTrip(IDataRecord row) => new Trip(
        id: (int)row["Id"],
        routeId: (int)row["RouteId"],
        startTime: (DateTime)row["StartTime"],
        currentDelay: (int)row["CurrentDelay"],
        route: new Route(
            id: (int)row["RouteId"],
            routeName: (string)row["RouteName"],
            validityStartDate: (DateTime)row["ValidityStartDate"],
            validityEndDate: row["ValidityEndDate"] as DateTime?,
            daysOfOperation: (string)row["DaysOfOperation"]
        )
    );

    private RouteStop MapRowToRouteStop(IDataRecord row) => new RouteStop(
        id: (int)row["RouteStopId"],
        routeId: (int)row["RouteId"],
        stopId: (int)row["StopId"],
        stopSequence: (int)row["StopSequence"],
        scheduled: (int)row["Scheduled"],
        route: new Route(
            id: (int)row["RouteId"],
            routeName: (string)row["RouteName"],
            validityStartDate: (DateTime)row["ValidityStartDate"],
            validityEndDate: row["ValidityEndDate"] as DateTime?,
            daysOfOperation: (string)row["DaysOfOperation"]
        ),
        stop: new Stop(
            id: (int)row["StopId"],
            name: (string)row["StopName"],
            shortName: (string)row["ShortName"],
            latitude: (double)row["Latitude"],
            longitude: (double)row["Longitude"]
        )
    );


    public async Task<Trip?> GetTripByIdAsync(int id)
    {
        return await _template.QuerySingleAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE t.Id = @id
            """,
            MapRowToTrip,
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Trip>> GetAllTripsAsync()
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id;
            """,
            MapRowToTrip
        );
    }

    public async Task AddTripAsync(Trip trip)
    {
        await _template.ExecuteAsync(
            $"""
            SET IDENTITY_INSERT {tripTableName} ON;
            INSERT INTO {tripTableName} (Id, RouteId, StartTime, CurrentDelay) VALUES (@id, @routeId, @startTime, @currentDelay);
            SET IDENTITY_INSERT {tripTableName} OFF;
            """,
            new QueryParameter("id", trip.Id),
            new QueryParameter("routeId", trip.RouteId),
            new QueryParameter("startTime", trip.StartTime),
            new QueryParameter("currentDelay", trip.CurrentDelay)
        );
    }

    public async Task UpdateTripAsync(Trip trip)
    {
        await _template.ExecuteAsync(
            $"UPDATE {tripTableName} SET RouteId = @routeId, StartTime = @startTime, CurrentDelay = @currentDelay WHERE Id = @id",
            new QueryParameter("id", trip.Id),
            new QueryParameter("routeId", trip.RouteId),
            new QueryParameter("startTime", trip.StartTime),
            new QueryParameter("currentDelay", trip.CurrentDelay)
        );
    }

    public async Task DeleteTripAsync(int id)
    {
        await _template.ExecuteAsync(
            $"DELETE FROM {tripTableName} WHERE Id = @id",
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsByRouteIdAsync(int routeId)
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE t.RouteId = @routeId
            """,
            MapRowToTrip,
            new QueryParameter("routeId", routeId)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date)
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE CAST(t.StartTime AS DATE) = @date
            """,
            MapRowToTrip,
            new QueryParameter("date", date)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsWithCurrentDelayAsync()
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE t.CurrentDelay > 0
            """,
            MapRowToTrip
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsByTimeRangeAsync(DateTime startTime, DateTime endTime)
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE t.StartTime BETWEEN @startTime AND @endTime
            """,
            MapRowToTrip,
            new QueryParameter("startTime", startTime),
            new QueryParameter("endTime", endTime)
        );
    }

    public async Task<IEnumerable<Trip>> GetDelayedTripsByRouteIdAsync(int routeId)
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE t.RouteId = @routeId AND t.CurrentDelay > 0
            """,
            MapRowToTrip,
            new QueryParameter("routeId", routeId)
        );
    }

    public async Task<double> GetAverageDelayForRouteAsync(int routeId)
    {
        return await _template.QuerySingleAsync(
            $"""
                SELECT AVG(CAST(CurrentDelay AS FLOAT)) 
                FROM {tripTableName} WHERE RouteId = @routeId
            """,
            row => (double)row[0],
            new QueryParameter("routeId", routeId)
        );
    }

    public async Task<IEnumerable<Trip>> GetActiveTripsAsync(DateTime currentTime)
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                WHERE t.StartTime <= @currentTime AND DATEADD(MINUTE, t.CurrentDelay, t.StartTime) > @currentTime
            """,
            MapRowToTrip,
            new QueryParameter("currentTime", currentTime)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsPaginatedAsync(int page, int pageSize)
    {
        return await _template.QueryAsync(
            $"""
                SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation 
                FROM {tripTableName} t 
                JOIN {routeTableName} r ON t.RouteId = r.Id 
                ORDER BY t.Id 
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY
            """,
            MapRowToTrip,
            new QueryParameter("offset", (page - 1) * pageSize),
            new QueryParameter("pageSize", pageSize)
        );
    }

    public async Task<IEnumerable<(Trip Trip, RouteStop RouteStop)>> GetNextTripsForStopAsync(int stopId, DateTime currentTimestamp, int limit)
    {
        return await _template.QueryAsync(
            $"""
            SELECT 
                Trip.Id AS Id,
                Trip.StartTime AS StartTime,
                Trip.CurrentDelay AS CurrentDelay,

                RouteStop.Id AS RouteStopId,
                RouteStop.StopSequence AS StopSequence,
                RouteStop.Scheduled AS Scheduled,

                Route.Id AS RouteId,
                Route.RouteName AS RouteName,
                Route.ValidityStartDate AS ValidityStartDate,
                Route.ValidityEndDate AS ValidityEndDate,
                Route.DaysOfOperation AS DaysOfOperation,

                Stop.Id AS StopId,
                Stop.Name AS StopName,
                Stop.ShortName AS ShortName,
                Stop.Latitude AS Latitude,
                Stop.Longitude AS Longitude
            FROM {stopTableName} Stop
                JOIN {routeStopTableName} RouteStop ON RouteStop.StopId = Stop.Id
                JOIN {tripTableName} Trip ON Trip.RouteId = RouteStop.RouteId
                JOIN {routeTableName} Route ON (
                    RouteStop.RouteId = Route.Id AND 
                    (validityEndDate < @currentTimestamp OR validityEndDate IS NULL)
                )
                WHERE Stop.Id = @stopId AND DATEADD(minute, Scheduled + CurrentDelay, StartTime) >= @currentTimestamp
                ORDER BY DATEADD(minute, Scheduled + CurrentDelay, StartTime)
                OFFSET 0 ROWS FETCH FIRST @limit ROW ONLY;
            """,
            row => (MapRowToTrip(row), MapRowToRouteStop(row)),
            new QueryParameter("stopId", stopId),
            new QueryParameter("currentTimestamp", currentTimestamp),
            new QueryParameter("limit", limit)
        );
    }
}
