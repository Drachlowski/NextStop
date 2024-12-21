using NextStop.Dal.Domain;
using NextStop.Domain;
using NextStop.Server.DTOs;
using Riok.Mapperly.Abstractions;

namespace NextStop.Server.Mappers;

[Mapper]
public static partial class RouteStopMapper
{
    public static partial RouteStopDto ToRouteStopDto(this RouteStop routeStop);

    public static partial RouteStop ToRouteStop(this RouteStopForCreationDto routeStop);


    [MapperIgnoreTarget(nameof(RouteStop.Id))]
    public static partial void UpdateRouteStop(this RouteStopForUpdateDto routeStopForUpdateDto, RouteStop routeStop);
}