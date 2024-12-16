using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface ITripDao
{
    Task<Trip?> GetTripByIdAsync(int id);
    Task<IEnumerable<Trip>> GetAllTripsAsync();
    Task<IEnumerable<Trip>> GetTripsByRouteIdAsync(int routeId);
    Task<IEnumerable<Trip>> GetTripsByDateAsync(DateTime date);
    Task<IEnumerable<Trip>> GetTripsWithCurrentDelayAsync();
    Task AddTripAsync(Trip trip);
    Task UpdateTripAsync(Trip trip);
    Task DeleteTripAsync(int id);
}
