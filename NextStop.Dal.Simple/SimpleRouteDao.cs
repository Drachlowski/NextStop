//using NextStop.Common.Exceptions;
//using NextStop.Dal.Interface;
//using NextStop.Domain;

//namespace NextStop.Dal.Simple;

//public class SimpleRouteDao : IRouteDao
//{
//    private readonly List<Route> _routes = [];


//    public void AddRoute(Route route)
//    {
//        if (GetRouteById(route.Id) is not null)
//        {
//            throw new DuplicateEntityException("Route", route.Id);
//        }
//        _routes.Add(route);
//    }

//    public void DeleteRoute(int route)
//    {
//        var routeToRemove = GetRouteById(route);
//        if (routeToRemove is not null)
//        {
//            _routes.Remove(routeToRemove);
//        }
//    }

//    public IEnumerable<Route> GetActiveRoutes(DateTime date)
//    {
//        return _routes.FindAll(route => 
//            route.ValidityStartDate.Date <= date.Date && 
//            (date.Date <= route.ValidityEndDate?.Date || route.ValidityEndDate is null)
//            );
//    }

//    public IEnumerable<Route> GetAllRoutes()
//    {
//        return _routes;
//    }

//    public Route? GetRouteById(int id)
//    {
//        return _routes.FirstOrDefault(route => id == route.Id);
//    }

//    public void UpdateRoute(Route route)
//    {
//        var existingRoute = GetRouteById(route.Id);
//        if (existingRoute is null)
//        {
//            AddRoute(route);
//        }
//        else
//        {
//            existingRoute.RouteName = route.RouteName;
//            existingRoute.ValidityStartDate = route.ValidityStartDate;
//            existingRoute.ValidityEndDate = route.ValidityEndDate;
//            existingRoute.DaysOfOperation = route.DaysOfOperation;
//        }
//    }
//}
