using NextStop.Dal.Domain;

namespace NextStop.Dal.Interface;

public interface IHolidayDao
{
    Task<Holiday?> GetHolidayByIdAsync(int id);
    Task<IEnumerable<Holiday>> GetAllHolidaysAsync();
    Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(DateTime date);
    Task<IEnumerable<Holiday>> GetHolidaysByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Holiday>> GetSchoolHolidaysByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task AddHolidayAsync(Holiday holiday);
    Task UpdateHolidayAsync(Holiday holiday);
    Task DeleteHolidayAsync(int id);
}
