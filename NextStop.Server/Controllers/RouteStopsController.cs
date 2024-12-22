using Microsoft.AspNetCore.Mvc;
using NextStop.Domain;
using NextStop.Server.DTOs.RouteStop;
using NextStop.Server.Mappers;
using NextStop.Server.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextStop.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteStopsController : ControllerBase
{
    private readonly IRouteStopService _routeStopService;
    private readonly IStopService _stopService;
    private readonly IRouteService _routeService;

    public RouteStopsController(IRouteStopService routeStopService, IStopService stopService, IRouteService routeService)
    {
        _routeStopService = routeStopService;
        _stopService = stopService;
        _routeService = routeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteStopDto>>> GetAllRouteStops()
    {
        var routeStops = await _routeStopService.GetAllRouteStopsAsync();
        return Ok(routeStops.Select(rs => rs.ToRouteStopDto()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RouteStopDto>> GetRouteStopById(int id)
    {
        var routeStop = await _routeStopService.GetRouteStopByIdAsync(id);
        if (routeStop == null)
        {
            return NotFound();
        }
        return Ok(routeStop.ToRouteStopDto());
    }

    [HttpPost]
    public async Task<ActionResult<RouteStopDto>> AddRouteStop([FromBody] RouteStopForCreationDto routeStopDto)
    {
        if (await _routeStopService.RouteStopExistsAsync(routeStopDto.Id))
        {
            return Conflict("A RouteStop with the given ID already exists.");
        }

        Domain.Route? route = await _routeService.GetRouteByIdAsync(routeStopDto.RouteId);
        if (route == null)
        {
            return BadRequest("The provided route doesn't exist");
        }
        routeStopDto.Route = route;

        Stop? stop = await _stopService.GetStopByIdAsync(routeStopDto.StopId);
        if (stop == null)
        {
            return BadRequest("The provided stop doesn't exist.");
        }
        routeStopDto.Stop = stop;

        var routeStop = routeStopDto.ToRouteStop();
        await _routeStopService.AddRouteStopAsync(routeStop);
        return CreatedAtAction(nameof(GetRouteStopById), new { id = routeStop.Id }, routeStop.ToRouteStopDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRouteStop(int id, [FromBody] RouteStopForUpdateDto routeStopDto)
    {
        var existingRouteStop = await _routeStopService.GetRouteStopByIdAsync(id);
        if (existingRouteStop == null)
        {
            return NotFound();
        }

        Domain.Route? route = await _routeService.GetRouteByIdAsync(routeStopDto.RouteId);
        if (route == null)
        {
            return BadRequest("The provided route doesn't exist");
        }
        routeStopDto.Route = route;

        Stop? stop = await _stopService.GetStopByIdAsync(routeStopDto.StopId);
        if (stop == null)
        {
            return BadRequest("The provided stop doesn't exist.");
        }
        routeStopDto.Stop = stop;

        routeStopDto.UpdateRouteStop(existingRouteStop);
        await _routeStopService.UpdateRouteStopAsync(existingRouteStop);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRouteStop(int id)
    {
        if (!await _routeStopService.RouteStopExistsAsync(id))
        {
            return NotFound();
        }

        await _routeStopService.DeleteRouteStopAsync(id);
        return NoContent();
    }
}
