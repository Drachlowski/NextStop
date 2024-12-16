namespace NextStop.Server.Mappers;

using NextStop.Server.DTOs;
using NextStop.Domain;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class RouteMapper
{
    public static partial RouteDto ToRouteDto(this Route route);
    public static partial Route ToRoute(this RouteForCreationDto routeDto);

    [MapperIgnoreTarget(nameof(Route.Id))]
    public static partial void UpdateRoute(this RouteForUpdateDto routeDto, Route route);
}
