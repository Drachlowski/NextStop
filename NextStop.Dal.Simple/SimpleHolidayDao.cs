using NextStop.Common.Exceptions;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Dal.Simple;

public class SimpleHolidayDao : IHolidayDao
{
    private readonly List<Holiday> _holidayList = [];

    public async Task AddHolidayAsync(Holiday holiday)
    {
        if (await GetHolidayByIdAsync(holiday.Id) is not null)
        {
            throw new DuplicateEntityException("Holiday", holiday.Id);
        }
        _holidayList.Add(holiday);
    }

    public async Task DeleteHolidayAsync(int id)
    {
        var holidayToRemove = await GetHolidayByIdAsync(id);
        if (holidayToRemove is not null)
        {
            _holidayList.Remove(holidayToRemove);
        }
    }

    public async Task<IEnumerable<Holiday>> GetAllHolidaysAsync()
    {
        return await Task.FromResult(_holidayList);
    }

    public async Task<Holiday?> GetHolidayByIdAsync(int id)
    {
        return await Task.FromResult(_holidayList.FirstOrDefault(holiday => id == holiday.Id));
    }

    public async Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(DateTime date)
    {
        return await Task.FromResult(_holidayList.FindAll(holiday => 
            !holiday.IsSchoolHoliday &&
            (holiday.Date.Date == date.Date ||
            (holiday.EndDate is not null && holiday.Date <= date.Date && date.Date <= holiday.EndDate))
            ));
    }

    public async Task<IEnumerable<Holiday>> GetSchoolHolidaysAsync(DateTime startDate, DateTime endDate)
    {
        return await Task.FromResult(_holidayList.FindAll(holiday =>
        {
            if (!holiday.IsSchoolHoliday) return false;

            DateTime holidayStartDate = holiday.Date.Date;
            DateTime holidayEndDate = holiday.EndDate?.Date ?? holidayStartDate;

            return (startDate <= holidayEndDate && holidayStartDate <= endDate);
        }));
    }

    public async Task UpdateHolidayAsync(Holiday holiday)
    {
        var existingHoliday = await GetHolidayByIdAsync(holiday.Id);
        if (existingHoliday is null)
        {
            await AddHolidayAsync(holiday);
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
