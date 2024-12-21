using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Dal.Ado;

public class AdoTripDao : ITripDao
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly AdoTemplate _template;

    public AdoTripDao(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _template = new AdoTemplate(connectionFactory);
    }

    private Trip MapRowToTrip(IDataRecord row) => new Trip(
        id: (int)row["Id"],
        routeId: (int)row["RouteId"],
        startTime: (TimeSpan)row["StartTime"],
        currentDelay: (int)row["CurrentDelay"],
        route: new Route(
            id: (int)row["RouteId"],
            routeName: (string)row["RouteName"],
            validityStartDate: (DateTime)row["ValidityStartDate"],
            validityEndDate: row["ValidityEndDate"] as DateTime?,
            daysOfOperation: (string)row["DaysOfOperation"]
        )
    );

    public async Task<Trip?> GetTripByIdAsync(int id)
    {
        return await _template.QuerySingleAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE t.Id = @id",
            MapRowToTrip,
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Trip>> GetAllTripsAsync()
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id",
            MapRowToTrip
        );
    }

    public async Task AddTripAsync(Trip trip)
    {
        await _template.ExecuteAsync(
            """
            SET IDENTITY_INSERT Trip ON;
            INSERT INTO Trip (RouteId, StartTime, CurrentDelay) VALUES (@routeId, @startTime, @currentDelay);
            SET IDENTITY_INSERT Trip OFF;
            """,
            new QueryParameter("routeId", trip.RouteId),
            new QueryParameter("startTime", trip.StartTime),
            new QueryParameter("currentDelay", trip.CurrentDelay)
        );
    }

    public async Task UpdateTripAsync(Trip trip)
    {
        await _template.ExecuteAsync(
            "UPDATE Trip SET RouteId = @routeId, StartTime = @startTime, CurrentDelay = @currentDelay WHERE Id = @id",
            new QueryParameter("id", trip.Id),
            new QueryParameter("routeId", trip.RouteId),
            new QueryParameter("startTime", trip.StartTime),
            new QueryParameter("currentDelay", trip.CurrentDelay)
        );
    }

    public async Task DeleteTripAsync(int id)
    {
        await _template.ExecuteAsync(
            "DELETE FROM Trip WHERE Id = @id",
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsByRouteIdAsync(int routeId)
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE t.RouteId = @routeId",
            MapRowToTrip,
            new QueryParameter("routeId", routeId)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date)
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE CAST(t.StartTime AS DATE) = @date",
            MapRowToTrip,
            new QueryParameter("date", date)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsWithCurrentDelayAsync()
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE t.CurrentDelay > 0",
            MapRowToTrip
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsByTimeRangeAsync(DateTime startTime, DateTime endTime)
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE t.StartTime BETWEEN @startTime AND @endTime",
            MapRowToTrip,
            new QueryParameter("startTime", startTime),
            new QueryParameter("endTime", endTime)
        );
    }

    public async Task<IEnumerable<Trip>> GetDelayedTripsByRouteIdAsync(int routeId)
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE t.RouteId = @routeId AND t.CurrentDelay > 0",
            MapRowToTrip,
            new QueryParameter("routeId", routeId)
        );
    }

    public async Task<double> GetAverageDelayForRouteAsync(int routeId)
    {
        return await _template.QuerySingleAsync(
            "SELECT AVG(CAST(CurrentDelay AS FLOAT)) FROM Trip WHERE RouteId = @routeId",
            row => (double)row[0],
            new QueryParameter("routeId", routeId)
        );
    }

    public async Task<IEnumerable<Trip>> GetActiveTripsAsync(DateTime currentTime)
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id WHERE t.StartTime <= @currentTime AND DATEADD(MINUTE, t.CurrentDelay, t.StartTime) > @currentTime",
            MapRowToTrip,
            new QueryParameter("currentTime", currentTime)
        );
    }

    public async Task<IEnumerable<Trip>> GetTripsPaginatedAsync(int page, int pageSize)
    {
        return await _template.QueryAsync(
            "SELECT t.*, r.RouteName, r.ValidityStartDate, r.ValidityEndDate, r.DaysOfOperation FROM Trip t JOIN Route r ON t.RouteId = r.Id ORDER BY t.Id OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY",
            MapRowToTrip,
            new QueryParameter("offset", (page - 1) * pageSize),
            new QueryParameter("pageSize", pageSize)
        );
    }
}
