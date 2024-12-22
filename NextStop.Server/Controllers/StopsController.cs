using Microsoft.AspNetCore.Mvc;
using NextStop.Domain;
using NextStop.Server.Services;
using NextStop.Server.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NextStop.Server.DTOs.Stop;
using Microsoft.AspNetCore.Authorization;

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
    //[Authorize(Roles = "Admin, User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetAllStops()
    {
        var stops = await _stopService.GetAllStopsAsync();
        return Ok(stops.Select(s => s.ToStopDto()));
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = "Admin, User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    //[Authorize(Roles = "Admin, User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetStopsByName([FromQuery] string name)
    {
        var stops = await _stopService.GetStopsByNameAsync(name);
        return Ok(stops.Select(s => s.ToStopDto()));
    }

    [HttpGet("nearby")]
    //[Authorize(Roles = "Admin, User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetNextStopsByCoordinates([FromQuery] double latitude, [FromQuery] double longitude)
    {
        var stops = await _stopService.GetNextStopsByCoordinatesAsync(latitude, longitude);
        return Ok(stops.Select(s => s.ToStopDto()));
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteStop(int id)
    {
        if (!await _stopService.StopExistsAsync(id))
        {
            return NotFound();
        }

        if (await _stopService.HasLinkedRoutesAsync(id))
        {
            return Conflict("Stop cannot be deleted as it has linked stops.");
        }

        await _stopService.DeleteStopAsync(id);
        return NoContent();
    }
}
