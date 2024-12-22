using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NextStop.Dal.Ado
{
    public class AdoRouteStopDao(IConnectionFactory connectionFactory, string routeStopTableName, string routeTableName, string stopTableName) : IRouteStopDao
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory;
        private readonly AdoTemplate _template = new AdoTemplate(connectionFactory);
        private readonly string routeStopTableName = routeStopTableName;
        private readonly string routeTableName = routeTableName;
        private readonly string stopTableName = stopTableName;


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
                $"""
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
                
                    FROM {routeStopTableName} RouteStop
                    JOIN {routeTableName} Route ON Route.Id = RouteStop.RouteId
                    JOIN {stopTableName} Stop ON Stop.Id = RouteStop.StopId
                    WHERE RouteStop.Id = @id;
                """,
                MapRowToRouteStop,
                new QueryParameter("id", id)
            );
        }

        public async Task<IEnumerable<RouteStop>> GetAllRouteStopsAsync()
        {
            return await _template.QueryAsync(
                $"""
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

                    FROM {routeStopTableName} RouteStop
                    JOIN {routeTableName} Route ON Route.Id = RouteStop.RouteId
                    JOIN {stopTableName} Stop ON Stop.Id = RouteStop.StopId
                """,
                MapRowToRouteStop
            );
        }

        public async Task<IEnumerable<Route>> GetAllRoutesForStopAsync(int stopId)
        {
            return await _template.QueryAsync(
                $"SELECT DISTINCT Route.* FROM {routeTableName} JOIN {routeStopTableName} RouteStop ON Route.Id = RouteStop.RouteId WHERE RouteStop.StopId = @stopId",
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
                $"SELECT DISTINCT Stop.* FROM {stopTableName} Stop JOIN {routeStopTableName} RouteStop ON Stop.Id = RouteStop.StopId WHERE RouteStop.RouteId = @routeId",
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
                $"""
                    SET IDENTITY_INSERT {routeStopTableName} ON;

                    INSERT INTO {routeStopTableName} (Id, RouteId, StopId, StopSequence, Scheduled) VALUES (@id, @routeId, @stopId, @stopSequence, @scheduled)

                    SET IDENTITY_INSERT {routeStopTableName} OFF;
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
                $"UPDATE {routeStopTableName} SET RouteId = @routeId, StopId = @stopId, StopSequence = @stopSequence, Scheduled = @scheduled WHERE Id = @id",
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
                $"DELETE FROM {routeStopTableName} WHERE Id = @id",
                new QueryParameter("id", id)
            );
        }

        public async Task<RouteStop?> GetRouteStopByRouteIdAndStopIdAsync(int routeId, int stopId)
        {
            return await _template.QuerySingleAsync(
                $"""
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
                
                    FROM {routeStopTableName} RouteStop
                    JOIN {routeTableName} Route ON Route.Id = RouteStop.RouteId AND RouteId = @routeId
                    JOIN {stopTableName} Stop ON Stop.Id = RouteStop.StopId AND StopId = @stopId;
                """,
                MapRowToRouteStop,
                new QueryParameter("routeId", routeId),
                new QueryParameter("stopId", stopId)
            );
        }
    }
}
