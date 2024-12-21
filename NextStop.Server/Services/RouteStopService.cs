using NextStop.Dal.Interface;
using NextStop.Domain;

namespace NextStop.Server.Services;

public class RouteStopService : IRouteStopService
{
    private readonly IRouteStopDao _routeStopDao;

    public RouteStopService(IRouteStopDao routeStopDao)
    {
        _routeStopDao = routeStopDao;
    }

    public async Task<RouteStop?> GetRouteStopByIdAsync(int id)
    {
        return await _routeStopDao.GetRouteStopByIdAsync(id);
    }

    public async Task<IEnumerable<RouteStop>> GetAllRouteStopsAsync()
    {
        return await _routeStopDao.GetAllRouteStopsAsync();
    }

    public async Task AddRouteStopAsync(RouteStop routeStop)
    {
        await _routeStopDao.AddRouteStopAsync(routeStop);
    }

    public async Task UpdateRouteStopAsync(RouteStop routeStop)
    {
        await _routeStopDao.UpdateRouteStopAsync(routeStop);
    }

    public async Task DeleteRouteStopAsync(int id)
    {
        await _routeStopDao.DeleteRouteStopAsync(id);
    }

    public async Task<bool> RouteStopExistsAsync(int id)
    {
        return await _routeStopDao.GetRouteStopByIdAsync(id) is not null;
    }
}
