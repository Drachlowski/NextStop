using Microsoft.AspNetCore.Mvc;
using NextStop.Domain;
using NextStop.Server.DTOs;
using NextStop.Server.Services;
using NextStop.Server.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextStop.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoutesController : ControllerBase
{
    private readonly IRouteService _routeService;

    public RoutesController(IRouteService routeService)
    {
        _routeService = routeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteDto>>> GetAllRoutes()
    {
        var routes = await _routeService.GetAllRoutesAsync();
        return Ok(routes.Select(r => r.ToRouteDto()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RouteDto>> GetRouteById(int id)
    {
        var route = await _routeService.GetRouteByIdAsync(id);
        if (route == null)
        {
            return NotFound();
        }
        return Ok(route.ToRouteDto());
    }

    [HttpPost]
    public async Task<ActionResult<RouteDto>> AddRoute([FromBody] RouteForCreationDto routeDto)
    {
        if (await _routeService.RouteExistsAsync(routeDto.Id))
        {
            return Conflict();
        }
        var route = routeDto.ToRoute();
        await _routeService.AddRouteAsync(route);
        return CreatedAtAction(nameof(GetRouteById), new { id = route.Id }, route.ToRouteDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoute(int id, [FromBody] RouteForUpdateDto routeDto)
    {
        var route = await _routeService.GetRouteByIdAsync(id);
        if (route == null)
        {
            return NotFound();
        }

        routeDto.UpdateRoute(route);
        await _routeService.UpdateRouteAsync(route);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoute(int id)
    {
        if (!await _routeService.RouteExistsAsync(id))
        {
            return NotFound();
        }

        if (await _routeService.HasLinkedStopsAsync(id))
        {
            return Conflict("Route cannot be deleted as it has linked stops.");
        }

        await _routeService.DeleteRouteAsync(id);
        return NoContent();
    }
}