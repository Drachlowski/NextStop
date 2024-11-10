using NextStop.Common.Exceptions;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Dal.Simple;

public class SimpleHolidayDao : IHolidayDao
{
    private List<Holiday> holidayList = new List<Holiday>();

    public void AddHoliday(Holiday holiday)
    {
        if (GetHolidayById(holiday.Id) is not null)
        {
            throw new DuplicateEntityException("Holiday", holiday.Id);
        }
        holidayList.Add(holiday);
    }

    public void DeleteHoliday(int id)
    {
        var holidayToRemove = holidayList.Find(h => h.Id == id);
        if (holidayToRemove != null)
        {
            holidayList.Remove(holidayToRemove);
        }
    }

    public IEnumerable<Holiday> GetAllHolidays()
    {
        return holidayList;
    }

    public Holiday? GetHolidayById(int id)
    {
        return holidayList.FirstOrDefault(holiday => id == holiday.Id);
    }

    public IEnumerable<Holiday> GetHolidaysByDate(DateTime date)
    {
        return holidayList.FindAll(holiday => holiday.Date.Date == date.Date);
    }

    public IEnumerable<Holiday> GetSchoolHolidays(DateTime startDate, DateTime endDate)
    {
        return holidayList.FindAll(holiday =>
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
