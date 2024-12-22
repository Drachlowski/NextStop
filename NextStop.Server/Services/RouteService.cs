namespace NextStop.Server.Services;


using NextStop.Dal.Interface;
using NextStop.Domain;
using NextStop.Server.DTOs.TripCheckIn;

public class RouteService : IRouteService
{
    private readonly IRouteDao _routeDao;
    private readonly ITripCheckInDao _tripCheckInDao;
    private readonly IRouteStopDao _routeStopDao;
    private readonly IStatisticDao _statisticDao;

    public RouteService(IRouteDao routeDao, ITripCheckInDao tripCheckInDao, IRouteStopDao routeStopDao, IStatisticDao statisticDao)
    {
        _routeDao = routeDao;
        _tripCheckInDao = tripCheckInDao;
        _routeStopDao = routeStopDao;
        _statisticDao = statisticDao;
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
        //return await Task.FromResult(false);
        var stops = await _routeStopDao.GetAllStopsForRouteAsync(routeId);
        return stops.Any();
    }

    public async Task<bool> RouteExistsAsync(int id)
    {
        return await GetRouteByIdAsync(id) is not null;
    }

    public async Task<IEnumerable<DelayStatistic>> GetRouteDelayStatisticsAsync(DateTime startDate, DateTime endDate, int? routeId = null)
    {
        return await _statisticDao.GetRouteDelayStatisticsAsync(startDate, endDate, routeId);
    }

}
