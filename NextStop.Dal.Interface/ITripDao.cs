using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface ITripDao
{
    Trip GetTripById(int id);
    IEnumerable<Trip> GetAllTrips();
    IEnumerable<Trip> GetTripsByRouteId(int routeId);
    IEnumerable<Trip> GetTripsByDate(DateTime date);
    IEnumerable<Trip> GetTripsWithCurrentDelay();
    void AddTrip(Trip trip);
    void UpdateTrip(Trip trip);
    void DeleteTrip(int id);
}
