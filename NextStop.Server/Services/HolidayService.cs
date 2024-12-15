using NextStop.Common.Database;
using NextStop.Dal.Ado;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Server.Services;

public class HolidayService(IConnectionFactory connectionFactory, string holidayTableName) : IHolidayService
{
    //private readonly IHolidayDao _dataSource = _dataSource;
    private readonly IHolidayDao _dataSource = new AdoHolidayDao(connectionFactory, holidayTableName);

    public async Task AddHolidayAsync(Holiday holiday)
    {
        await _dataSource.AddHolidayAsync(holiday);
    }

    public async Task DeleteHolidayAsync(int id)
    {
        await _dataSource.DeleteHolidayAsync(id);
    }

    public async Task<Holiday?> GetHolidayByIdAsync(int id)
    {
        return await _dataSource.GetHolidayByIdAsync(id);
    }

    public async Task<IEnumerable<Holiday>> GetHolidaysAsync()
    {
        return await _dataSource.GetAllHolidaysAsync();
    }

    public Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Holiday>> GetHolidaysByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Holiday>> GetSchoolHolidaysAsync(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HolidayExistsAsync(int id)
    {
        return await GetHolidayByIdAsync(id) is not null;
    }

    public async Task UpdateHolidayAsync(Holiday holiday)
    {
        await _dataSource.UpdateHolidayAsync(holiday);
    }
}
