//using NextStop.Common.Exceptions;
//using NextStop.Dal.Interface;
//using NextStop.Domain;

//namespace NextStop.Dal.Simple;

//public class SimpleRouteStopDao : IRouteStopDao
//{
//    private readonly List<RouteStop> _routeStopList = [];

//    public void AddRouteStop(RouteStop routeStop)
//    {
//        if (GetRouteStopById(routeStop.Id) is not null)
//        {
//            throw new DuplicateEntityException("RouteStop", routeStop.Id);
//        }
//        _routeStopList.Add(routeStop);
//    }

//    public IEnumerable<Route> GetAllRoutesForStop(int stopId)
//    {
//        return _routeStopList
//            .Where(routeStop => routeStop.StopId.Equals(stopId))
//            .Select(routeStop => routeStop.Route)
//            .Distinct()
//            .ToList();
//    }

//    public IEnumerable<RouteStop> GetAllRouteStops()
//    {
//        return _routeStopList;
//    }

//    public IEnumerable<Stop> GetAllStopsForRoute(int routeId)
//    {
//        return _routeStopList
//            .Where(routeStop => routeStop.RouteId.Equals(routeId))
//            .Select(routeStop => routeStop.Stop)
//            .Distinct()
//            .ToList();
//    }

//    public RouteStop? GetRouteStopById(int id)
//    {
//        return _routeStopList.Find(item => item.Id == id);
//    }

//    public void DeleteRouteStop(int id)
//    {
//        var routeStopToRemove = GetRouteStopById(id);
//        if (routeStopToRemove is not null)
//        {
//            _routeStopList.Remove(routeStopToRemove);
//        }
//    }

//    public void UpdateRouteStop(RouteStop routeStop)
//    {
//        var existingRouteStop = GetRouteStopById(routeStop.Id);
//        if (existingRouteStop is null)
//        {
//            AddRouteStop(routeStop);
//        }
//        else
//        {
//            existingRouteStop.RouteId = routeStop.RouteId;
//            existingRouteStop.StopId = routeStop.StopId;
//            existingRouteStop.StopSequence = routeStop.StopSequence;
//            existingRouteStop.Scheduled = routeStop.Scheduled;
//            existingRouteStop.Route = routeStop.Route;
//            existingRouteStop.Stop = routeStop.Stop;
//        }
//    }
//}
