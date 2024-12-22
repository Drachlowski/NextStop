using NextStop.Domain;
using NextStop.Server.DTOs.Timetable;

namespace NextStop.Server.Mappers;

public static class TimetableMapper
{
    public static TimetableResponseDto ToTimetableResponseDto(this Trip trip, Domain.Route route, DateTime departureTime, DateTime arrivalTime)
    {
        return new TimetableResponseDto
        {
            TripId = trip.Id,
            RouteName = route.RouteName,
            Delay = trip.CurrentDelay,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime
        };
    }
}