namespace NextStop.Server.DTOs.Timetable;

public class TimetableRequestDto
{
    public required int StartStopId { get; set; }
    public required int EndStopId { get; set; }
    public DateTime? DepartureTime { get; set; }
}
