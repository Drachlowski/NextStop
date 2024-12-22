using NextStop.Dal.Domain;
using NextStop.Domain;
using NextStop.Server.DTOs.RouteStop;
using Riok.Mapperly.Abstractions;

namespace NextStop.Server.Mappers;

[Mapper]
public static partial class RouteStopMapper
{
    public static partial RouteStopDto ToRouteStopDto(this Domain.RouteStop routeStop);

    public static partial Domain.RouteStop ToRouteStop(this RouteStopForCreationDto routeStop);


    [MapperIgnoreTarget(nameof(Domain.RouteStop.Id))]
    public static partial void UpdateRouteStop(this RouteStopForUpdateDto routeStopForUpdateDto, Domain.RouteStop routeStop);
}