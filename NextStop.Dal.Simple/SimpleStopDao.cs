using NextStop.Common.Exceptions;
using NextStop.Dal.Interface;
using NextStop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Dal.Simple;

public class SimpleStopDao : IStopDao
{
    private readonly List<Stop> _stops = [];

    public void AddStop(Stop stop)
    {
        if (GetStopById(stop.Id) is not null)
        {
            throw new DuplicateEntityException("Stop", stop.Id);
        }
        _stops.Add(stop);
    }

    public void DeleteStop(int id)
    {
        var stopToRemove = GetStopById(id);
        if (stopToRemove is not null)
        {
            _stops.Remove(stopToRemove);
        }
    }

    public IEnumerable<Stop> GetAllStops()
    {
        return _stops;
    }

    public IEnumerable<Stop> GetNextStopsByCoordinates(double latitude, double longitude)
    {
        return _stops
            .Select(stop => new
            {
                Stop = stop,
                Distance = CalculateDistance(latitude, longitude, stop.Latitude, stop.Longitude)
            })
            .OrderBy(x => x.Distance)
            .Take(5)
            .Select(x => x.Stop);
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var lat = (lat2 - lat1) * Math.PI / 180;
        var lon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(lon / 2) * Math.Sin(lon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }


    public Stop? GetStopById(int id)
    {
        return _stops.Find(stop => id == stop.Id);
    }

    public IEnumerable<Stop> GetStopsByName(string name)
    {
        return _stops.Where(stop => stop.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    public void UpdateStop(Stop stop)
    {
        var existingStop = GetStopById(stop.Id);
        if (existingStop is null)
        {
            AddStop(stop);
        }
        else
        {
            existingStop.Name = stop.Name;
            existingStop.ShortName = stop.ShortName;
            existingStop.Latitude = stop.Latitude;
            existingStop.Longitude = stop.Longitude;
        }
    }
}
