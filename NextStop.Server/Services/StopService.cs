using NextStop.Dal.Interface;
using NextStop.Domain;

namespace NextStop.Server.Services;

public class StopService : IStopService
{
    private readonly IStopDao _stopDao;

    public StopService(IStopDao stopDao)
    {
        _stopDao = stopDao;
    }

    public async Task<Stop?> GetStopByIdAsync(int id)
    {
        return await _stopDao.GetStopByIdAsync(id);
    }

    public async Task<IEnumerable<Stop>> GetAllStopsAsync()
    {
        return await _stopDao.GetAllStopsAsync();
    }

    public async Task<IEnumerable<Stop>> GetStopsByNameAsync(string name)
    {
        return await _stopDao.GetStopsByNameAsync(name);
    }

    public async Task<IEnumerable<Stop>> GetNextStopsByCoordinatesAsync(double latitude, double longitude)
    {
        return await _stopDao.GetNextStopsByCoordinatesAsync(latitude, longitude);
    }

    public async Task AddStopAsync(Stop stop)
    {
        await _stopDao.AddStopAsync(stop);
    }

    public async Task UpdateStopAsync(Stop stop)
    {
        await _stopDao.UpdateStopAsync(stop);
    }

    public async Task DeleteStopAsync(int id)
    {
        await _stopDao.DeleteStopAsync(id);
    }
}
