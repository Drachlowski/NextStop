using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface IRouteStopDao
{
    Task<RouteStop?> GetRouteStopByIdAsync(int id);
    Task<RouteStop?> GetRouteStopByRouteIdAndStopIdAsync(int routeId, int stopId);
    Task<IEnumerable<RouteStop>> GetAllRouteStopsAsync();
    Task<IEnumerable<Route>> GetAllRoutesForStopAsync(int stopId);
    Task<IEnumerable<Stop>> GetAllStopsForRouteAsync(int routeId);
    Task AddRouteStopAsync(RouteStop routeStop);
    Task UpdateRouteStopAsync(RouteStop routeStop);
    Task DeleteRouteStopAsync(int id);
}
