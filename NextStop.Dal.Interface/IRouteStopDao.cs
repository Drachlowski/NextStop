using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface IRouteStopDao
{
    RouteStop GetRouteStopById(int id);
    IEnumerable<RouteStop> GetAllRoutes();
    void AddRouteStop(RouteStop routeStop);
    void UpdateRouteStop(RouteStop routeStop);
    void RemoveRouteStop(int id);
}
