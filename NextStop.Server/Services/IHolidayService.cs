using NextStop.Dal.Domain;

namespace NextStop.Server.Services;

public interface IHolidayService
{
    Task<Holiday?> GetHolidayByIdAsync(int id);
    Task<IEnumerable<Holiday>> GetHolidaysAsync();
    Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(DateTime date);
    Task<IEnumerable<Holiday>> GetHolidaysByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Holiday>> GetSchoolHolidaysAsync(DateTime startDate, DateTime endDate);
    Task AddHolidayAsync(Holiday holiday);
    Task UpdateHolidayAsync(Holiday holiday);
    Task DeleteHolidayAsync(int id);
    Task<bool> HolidayExistsAsync(int id);
}
