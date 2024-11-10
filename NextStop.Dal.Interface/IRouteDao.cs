using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface IRouteDao
{
    Route? GetRouteById(int id);
    IEnumerable<Route> GetAllRoutes();
    IEnumerable<Route> GetActiveRoutes(DateTime date);
    void AddRoute(Route route);
    void UpdateRoute(Route route);
    void DeleteRoute(int id);
}
