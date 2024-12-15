namespace NextStop.Server.DTOs;

public class HolidayForCreationDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required DateTime Date { get; set; }
    public DateTime? EndDate { get; set; }
    public required bool IsSchoolHoliday { get; set; }
}
