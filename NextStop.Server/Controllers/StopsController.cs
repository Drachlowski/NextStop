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
public class StopsController : ControllerBase
{
    private readonly IStopService _stopService;

    public StopsController(IStopService stopService)
    {
        _stopService = stopService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetAllStops()
    {
        var stops = await _stopService.GetAllStopsAsync();
        return Ok(stops.Select(s => s.ToStopDto()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StopDto>> GetStopById(int id)
    {
        var stop = await _stopService.GetStopByIdAsync(id);
        if (stop == null)
        {
            return NotFound();
        }
        return Ok(stop.ToStopDto());
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetStopsByName([FromQuery] string name)
    {
        var stops = await _stopService.GetStopsByNameAsync(name);
        return Ok(stops.Select(s => s.ToStopDto()));
    }

    [HttpGet("nearby")]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetNextStopsByCoordinates([FromQuery] double latitude, [FromQuery] double longitude)
    {
        var stops = await _stopService.GetNextStopsByCoordinatesAsync(latitude, longitude);
        return Ok(stops.Select(s => s.ToStopDto()));
    }

    [HttpPost]
    public async Task<ActionResult<StopDto>> AddStop([FromBody] StopForCreationDto stopDto)
    {
        if (await _stopService.StopExistsAsync(stopDto.Id))
        {
            return Conflict();
        }
        var stop = stopDto.ToStop();
        await _stopService.AddStopAsync(stop);
        return CreatedAtAction(nameof(GetStopById), new { id = stop.Id }, stop.ToStopDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStop(int id, [FromBody] StopForUpdateDto stopDto)
    {
        var stop = await _stopService.GetStopByIdAsync(id);
        if (stop == null)
        {
            return NotFound();
        }

        stopDto.UpdateStop(stop);
        await _stopService.UpdateStopAsync(stop);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStop(int id)
    {
        var stop = await _stopService.GetStopByIdAsync(id);
        if (stop == null)
        {
            return NotFound();
        }

        await _stopService.DeleteStopAsync(id);
        return NoContent();
    }
}
