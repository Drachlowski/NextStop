using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface IRouteDao
{
    Task<Route?> GetRouteByIdAsync(int id);
    Task<IEnumerable<Route>> GetAllRoutesAsync();
    Task<IEnumerable<Route>> GetActiveRoutesAsync(DateTime date);
    Task AddRouteAsync(Route route);
    Task UpdateRouteAsync(Route route);
    Task DeleteRouteAsync(int id);
}
