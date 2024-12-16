using NextStop.Domain;

namespace NextStop.Server.Services;

public interface IStopService
{
    Task<Stop?> GetStopByIdAsync(int id);
    Task<IEnumerable<Stop>> GetAllStopsAsync();
    Task<IEnumerable<Stop>> GetStopsByNameAsync(string name);
    Task<IEnumerable<Stop>> GetNextStopsByCoordinatesAsync(double latitude, double longitude);
    Task AddStopAsync(Stop stop);
    Task UpdateStopAsync(Stop stop);
    Task DeleteStopAsync(int id);
}
