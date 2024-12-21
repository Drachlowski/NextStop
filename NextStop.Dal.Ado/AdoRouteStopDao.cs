using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NextStop.Dal.Ado
{
    public class AdoRouteStopDao : IRouteStopDao
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly AdoTemplate _template;

        public AdoRouteStopDao(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _template = new AdoTemplate(connectionFactory);
        }

        private RouteStop MapRowToRouteStop(IDataRecord row) => new RouteStop(
            id: (int)row["Id"],
            routeId: (int)row["RouteId"],
            stopId: (int)row["StopId"],
            stopSequence: (int)row["StopSequence"],
            scheduled: (int)row["Scheduled"],
            route: new Route(
                id: (int)row["RouteId"],
                routeName: (string)row["RouteName"],
                validityStartDate: (DateTime)row["RouteValidityStartDate"],
                validityEndDate: row["RouteValidityEndDate"] as DateTime?,
                daysOfOperation: (string)row["RouteDaysOfOperation"]
            ),
            stop: new Stop(
                id: (int)row["StopId"],
                name: (string)row["StopName"],
                shortName: (string)row["StopShortName"],
                latitude: (double)row["StopLatitude"],
                longitude: (double)row["StopLongitude"]
            )
        );

        public async Task<RouteStop?> GetRouteStopByIdAsync(int id)
        {
            return await _template.QuerySingleAsync(
                """
                    SELECT
                        RouteStop.Id AS Id,
                        RouteStop.StopId AS StopId,
                        RouteStop.RouteId AS RouteId,
                        RouteStop.StopSequence AS StopSequence,
                        RouteStop.Scheduled AS Scheduled,
                
                        Route.RouteName AS RouteName,
                        Route.ValidityStartDate AS RouteValidityStartDate,
                        Route.ValidityEndDate AS RouteValidityEndDate,
                        Route.DaysOfOperation AS RouteDaysOfOperation,
                
                        Stop.Name AS StopName,
                        Stop.ShortName AS StopShortName,
                        Stop.Latitude AS StopLatitude,
                        Stop.Longitude AS StopLongitude
                
                    FROM RouteStop
                    JOIN Route ON Route.Id = RouteStop.RouteId
                    JOIN Stop ON Stop.Id = RouteStop.StopId
                    WHERE RouteStop.Id = @id;
                """,
                MapRowToRouteStop,
                new QueryParameter("id", id)
            );
        }

        public async Task<IEnumerable<RouteStop>> GetAllRouteStopsAsync()
        {
            return await _template.QueryAsync(
                """
                    SELECT
                        RouteStop.Id AS Id,
                        RouteStop.StopId AS StopId,
                        RouteStop.RouteId AS RouteId,
                        RouteStop.StopSequence AS StopSequence,
                        RouteStop.Scheduled AS Scheduled,

                        Route.RouteName AS RouteName,
                        Route.ValidityStartDate AS RouteValidityStartDate,
                        Route.ValidityEndDate AS RouteValidityEndDate,
                        Route.DaysOfOperation AS RouteDaysOfOperation,

                        Stop.Name AS StopName,
                        Stop.ShortName AS StopShortName,
                        Stop.Latitude AS StopLatitude,
                        Stop.Longitude AS StopLongitude

                    FROM RouteStop
                    JOIN Route ON Route.Id = RouteStop.RouteId
                    JOIN Stop ON Stop.Id = RouteStop.StopId;
                """,
                MapRowToRouteStop
            );
        }

        public async Task<IEnumerable<Route>> GetAllRoutesForStopAsync(int stopId)
        {
            return await _template.QueryAsync(
                "SELECT DISTINCT r.* FROM Routes r JOIN RouteStop rs ON r.Id = rs.RouteId WHERE rs.StopId = @stopId",
                row => new Route(
                    id: (int)row["Id"],
                    routeName: (string)row["RouteName"],
                    validityStartDate: (DateTime)row["ValidityStartDate"],
                    validityEndDate: row["ValidityEndDate"] as DateTime?,
                    daysOfOperation: (string)row["DaysOfOperation"]
                ),
                new QueryParameter("stopId", stopId)
            );
        }

        public async Task<IEnumerable<Stop>> GetAllStopsForRouteAsync(int routeId)
        {
            return await _template.QueryAsync(
                "SELECT DISTINCT s.* FROM Stops s JOIN RouteStop rs ON s.Id = rs.StopId WHERE rs.RouteId = @routeId",
                row => new Stop(
                    id: (int)row["Id"],
                    name: (string)row["Name"],
                    shortName: (string)row["ShortName"],
                    latitude: (double)row["Latitude"],
                    longitude: (double)row["Longitude"]
                ),
                new QueryParameter("routeId", routeId)
            );
        }

        public async Task AddRouteStopAsync(RouteStop routeStop)
        {
            await _template.ExecuteAsync(
                """
                    SET IDENTITY_INSERT RouteStop ON;

                    INSERT INTO RouteStop (Id, RouteId, StopId, StopSequence, Scheduled) VALUES (@id, @routeId, @stopId, @stopSequence, @scheduled)

                    SET IDENTITY_INSERT RouteStop OFF;
                """,
                new QueryParameter("id", routeStop.Id),
                new QueryParameter("routeId", routeStop.RouteId),
                new QueryParameter("stopId", routeStop.StopId),
                new QueryParameter("stopSequence", routeStop.StopSequence),
                new QueryParameter("scheduled", routeStop.Scheduled)
            );
        }

        public async Task UpdateRouteStopAsync(RouteStop routeStop)
        {
            await _template.ExecuteAsync(
                "UPDATE RouteStop SET RouteId = @routeId, StopId = @stopId, StopSequence = @stopSequence, Scheduled = @scheduled WHERE Id = @id",
                new QueryParameter("id", routeStop.Id),
                new QueryParameter("routeId", routeStop.RouteId),
                new QueryParameter("stopId", routeStop.StopId),
                new QueryParameter("stopSequence", routeStop.StopSequence),
                new QueryParameter("scheduled", routeStop.Scheduled)
            );
        }

        public async Task DeleteRouteStopAsync(int id)
        {
            await _template.ExecuteAsync(
                "DELETE FROM RouteStop WHERE Id = @id",
                new QueryParameter("id", id)
            );
        }
    }
}
