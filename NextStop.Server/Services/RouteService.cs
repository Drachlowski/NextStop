namespace NextStop.Server.Services;


using NextStop.Dal.Interface;
using NextStop.Domain;

public class RouteService : IRouteService
{
    private readonly IRouteDao _routeDao;
    private readonly IRouteStopDao _routeStopDao;

    public RouteService(IRouteDao routeDao, IRouteStopDao routeStopDao)
    {
        _routeDao = routeDao;
        _routeStopDao = routeStopDao;
    }

    public async Task<IEnumerable<Route>> GetAllRoutesAsync()
    {
        return await _routeDao.GetAllRoutesAsync();
    }

    public async Task<Route?> GetRouteByIdAsync(int id)
    {
        return await _routeDao.GetRouteByIdAsync(id);
    }

    public async Task AddRouteAsync(Route route)
    {
        await _routeDao.AddRouteAsync(route);
    }

    public async Task UpdateRouteAsync(Route route)
    {
        await _routeDao.UpdateRouteAsync(route);
    }

    public async Task DeleteRouteAsync(int id)
    {
        await _routeDao.DeleteRouteAsync(id);
    }

    public async Task<bool> HasLinkedStopsAsync(int routeId)
    {
        var stops = await _routeStopDao.GetAllStopsForRouteAsync(routeId);
        return stops.Any();
    }
}
