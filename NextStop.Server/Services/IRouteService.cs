namespace NextStop.Server.Services;

using NextStop.Domain;

public interface IRouteService
{
    Task<IEnumerable<Route>> GetAllRoutesAsync();
    Task<Route?> GetRouteByIdAsync(int id);
    Task AddRouteAsync(Route route);
    Task UpdateRouteAsync(Route route);
    Task DeleteRouteAsync(int id);
    Task<bool> HasLinkedStopsAsync(int routeId);
}
