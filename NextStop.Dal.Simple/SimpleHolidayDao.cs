using NextStop.Common.Exceptions;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Dal.Simple;

public class SimpleHolidayDao : IHolidayDao
{
    private readonly List<Holiday> _holidayList = [];

    public void AddHoliday(Holiday holiday)
    {
        if (GetHolidayById(holiday.Id) is not null)
        {
            throw new DuplicateEntityException("Holiday", holiday.Id);
        }
        _holidayList.Add(holiday);
    }

    public void DeleteHoliday(int id)
    {
        var holidayToRemove = GetHolidayById(id);
        if (holidayToRemove is not null)
        {
            _holidayList.Remove(holidayToRemove);
        }
    }

    public IEnumerable<Holiday> GetAllHolidays()
    {
        return _holidayList;
    }

    public Holiday? GetHolidayById(int id)
    {
        return _holidayList.FirstOrDefault(holiday => id == holiday.Id);
    }

    public IEnumerable<Holiday> GetHolidaysByDate(DateTime date)
    {
        return _holidayList.FindAll(holiday => 
            !holiday.IsSchoolHoliday &&
            (holiday.Date.Date == date.Date ||
            (holiday.EndDate is not null && holiday.Date <= date.Date && date.Date <= holiday.EndDate))
            );
    }

    public IEnumerable<Holiday> GetSchoolHolidays(DateTime startDate, DateTime endDate)
    {
        return _holidayList.FindAll(holiday =>
        {
            if (!holiday.IsSchoolHoliday) return false;

            DateTime holidayStartDate = holiday.Date.Date;
            DateTime holidayEndDate = holiday.EndDate?.Date ?? holidayStartDate;

            return (startDate <= holidayEndDate && holidayStartDate <= endDate);
        });
    }

    public void UpdateHoliday(Holiday holiday)
    {
        var existingHoliday = GetHolidayById(holiday.Id);
        if (existingHoliday is null)
        {
            AddHoliday(holiday);
        }
        else
        {
            existingHoliday.Name = holiday.Name;
            existingHoliday.Date = holiday.Date;
            existingHoliday.EndDate = holiday.EndDate;
            existingHoliday.IsSchoolHoliday = holiday.IsSchoolHoliday;
        }
    }
}
