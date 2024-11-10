using NextStop.Dal.Domain;

namespace NextStop.Dal.Interface;

public interface IHolidayDao
{
    Holiday? GetHolidayById(int id);
    IEnumerable<Holiday> GetAllHolidays();
    IEnumerable<Holiday> GetHolidaysByDate(DateTime date);
    IEnumerable<Holiday> GetSchoolHolidays(DateTime startDate, DateTime endDate);
    void AddHoliday(Holiday holiday);
    void UpdateHoliday(Holiday holiday);
    void DeleteHoliday(int id);
}
