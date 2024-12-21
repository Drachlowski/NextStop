using NextStop.Domain;

namespace NextStop.Server.Services;

public interface IRouteStopService
{
    Task<RouteStop?> GetRouteStopByIdAsync(int id);
    Task<IEnumerable<RouteStop>> GetAllRouteStopsAsync();
    Task AddRouteStopAsync(RouteStop routeStop);
    Task UpdateRouteStopAsync(RouteStop routeStop);
    Task DeleteRouteStopAsync(int id);
    Task<bool> RouteStopExistsAsync(int id);
}
