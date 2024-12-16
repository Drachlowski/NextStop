using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NextStop.Dal.Ado;

public class AdoRouteDao : IRouteDao
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly AdoTemplate _template;

    public AdoRouteDao(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _template = new AdoTemplate(connectionFactory);
    }

    private Route MapRowToRoute(IDataRecord row) => new Route(
        id: (int)row["Id"],
        routeName: (string)row["RouteName"],
        validityStartDate: (DateTime)row["ValidityStartDate"],
        validityEndDate: row["ValidityEndDate"] as DateTime?,
        daysOfOperation: (string)row["DaysOfOperation"]
    );

    public async Task<Route?> GetRouteByIdAsync(int id)
    {
        return await _template.QuerySingleAsync(
            "SELECT Id, RouteName, ValidityStartDate, ValidityEndDate, DaysOfOperation FROM Route WHERE Id = @id",
            MapRowToRoute,
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Route>> GetAllRoutesAsync()
    {
        return await _template.QueryAsync(
            "SELECT Id, RouteName, ValidityStartDate, ValidityEndDate, DaysOfOperation FROM Route",
            MapRowToRoute
        );
    }

    public async Task<IEnumerable<Route>> GetActiveRoutesAsync(DateTime date)
    {
        return await _template.QueryAsync(
            "SELECT Id, RouteName, ValidityStartDate, ValidityEndDate, DaysOfOperation FROM Route WHERE ValidityStartDate <= @date AND (ValidityEndDate IS NULL OR ValidityEndDate >= @date)",
            MapRowToRoute,
            new QueryParameter("date", date)
        );
    }

    public async Task AddRouteAsync(Route route)
    {
        await _template.ExecuteAsync(
            "INSERT INTO Route (Id, RouteName, ValidityStartDate, ValidityEndDate, DaysOfOperation) VALUES (@id, @routeName, @validityStartDate, @validityEndDate, @daysOfOperation)",
            new QueryParameter("id", route.Id),
            new QueryParameter("routeName", route.RouteName),
            new QueryParameter("validityStartDate", route.ValidityStartDate),
            new QueryParameter("validityEndDate", route.ValidityEndDate ?? (object)DBNull.Value),
            new QueryParameter("daysOfOperation", route.DaysOfOperation)
        );
    }

    public async Task UpdateRouteAsync(Route route)
    {
        await _template.ExecuteAsync(
            "UPDATE Route SET RouteName = @routeName, ValidityStartDate = @validityStartDate, ValidityEndDate = @validityEndDate, DaysOfOperation = @daysOfOperation WHERE Id = @id",
            new QueryParameter("id", route.Id),
            new QueryParameter("routeName", route.RouteName),
            new QueryParameter("validityStartDate", route.ValidityStartDate),
            new QueryParameter("validityEndDate", route.ValidityEndDate ?? (object)DBNull.Value),
            new QueryParameter("daysOfOperation", route.DaysOfOperation)
        );
    }

    public async Task DeleteRouteAsync(int id)
    {
        await _template.ExecuteAsync(
            "DELETE FROM Route WHERE Id = @id",
            new QueryParameter("id", id)
        );
    }
}