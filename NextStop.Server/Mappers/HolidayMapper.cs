using NextStop.Dal.Domain;
using NextStop.Server.DTOs;
using Riok.Mapperly.Abstractions;

namespace NextStop.Server.Mappers;

[Mapper]
public static partial class HolidayMapper
{
    public static partial HolidayDto ToHolidayDto(this Holiday holiday);
    public static partial Holiday ToHoliday(this HolidayForCreationDto holiday);

    [MapperIgnoreTarget(nameof(Holiday.Id))]
    public static partial void UpdateHoliday(this HolidayForUpdateDto holidayForUpdateDto, Holiday holiday);
}
