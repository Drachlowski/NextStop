using NextStop.Domain;
using NextStop.Server.DTOs.Trip;
using Riok.Mapperly.Abstractions;

namespace NextStop.Server.Mappers;

[Mapper]
public static partial class TripMapper
{
    public static partial TripDto ToTripDto(this Trip trip);

    public static partial Trip ToTrip(this TripForCreationDto tripForCreationDto);

    [MapperIgnoreTarget(nameof(Trip.Id))]
    public static partial void UpdateTrip(this TripForUpdateDto tripForUpdateDto, Trip trip);
}