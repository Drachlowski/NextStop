using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Interface;

public interface IStopDao
{
    Stop? GetStopById(int id);
    IEnumerable<Stop> GetAllStops();
    IEnumerable<Stop> GetStopsByName(string name);
    IEnumerable<Stop> GetNextStopsByCoordinates(double latitude, double longitude);
    void AddStop(Stop stop);
    void UpdateStop(Stop stop);
    void DeleteStop(int id);
}
