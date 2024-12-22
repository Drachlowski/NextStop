using System.ComponentModel.DataAnnotations;

namespace NextStop.Server.DTOs.Holiday;

public class HolidayForUpdateDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "Date must be between 1753-01-01 and 9999-12-31.")]
    public required DateTime Date { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "EndDate must be between 1753-01-01 and 9999-12-31.")]
    public DateTime? EndDate { get; set; }
    public required bool IsSchoolHoliday { get; set; }
}
