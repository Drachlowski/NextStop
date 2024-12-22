using Microsoft.AspNetCore.Mvc;
using NextStop.Dal.Interface;
using NextStop.Domain;
using NextStop.Server.DTOs.Timetable;
using NextStop.Server.DTOs.Trip;
using NextStop.Server.Mappers;
using NextStop.Server.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextStop.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly IRouteService _routeService;

    public TripsController(ITripService tripService, IRouteService routeService)
    {
        _tripService = tripService;
        _routeService = routeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetAllTrips()
    {
        var trips = await _tripService.GetAllTripsAsync();
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TripDto>> GetTripById(int id)
    {
        var trip = await _tripService.GetTripByIdAsync(id);
        if (trip == null)
        {
            return NotFound();
        }
        return Ok(trip.ToTripDto());
    }

    [HttpPost]
    public async Task<ActionResult<TripDto>> AddTrip([FromBody] TripForCreationDto tripDto)
    {
        if (await _tripService.TripExistsAsync(tripDto.Id))
        {
            return Conflict();
        }
        var route = await _routeService.GetRouteByIdAsync(tripDto.RouteId);
        if (route == null)
        {
            return BadRequest("The provided route doesn't exist");
        }
        tripDto.Route = route;

        var trip = tripDto.ToTrip();
        await _tripService.AddTripAsync(trip);
        return CreatedAtAction(nameof(GetTripById), new { id = trip.Id }, trip.ToTripDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripForUpdateDto tripDto)
    {
        var existingTrip = await _tripService.GetTripByIdAsync(id);
        if (existingTrip == null)
        {
            return NotFound();
        }

        var route = await _routeService.GetRouteByIdAsync(tripDto.RouteId);
        if (route == null)
        {
            return BadRequest("The provided route doesn't exist");
        }
        tripDto.Route = route;

        tripDto.UpdateTrip(existingTrip);
        await _tripService.UpdateTripAsync(existingTrip);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        if (!await _tripService.TripExistsAsync(id))
        {
            return NotFound();
        }

        await _tripService.DeleteTripAsync(id);
        return NoContent();
    }

    [HttpGet("ByRoute/{routeId}")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetTripsByRouteId(int routeId)
    {
        var trips = await _tripService.GetTripsByRouteIdAsync(routeId);
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("ByDate")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetTripsByDate([FromQuery] DateTime date)
    {
        var trips = await _tripService.GetTripsByDateAsync(date);
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("WithCurrentDelay")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetTripsWithCurrentDelay()
    {
        var trips = await _tripService.GetTripsWithCurrentDelayAsync();
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("ByTimeRange")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetTripsByTimeRange([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var trips = await _tripService.GetTripsByTimeRangeAsync(startTime, endTime);
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("Delayed/{routeId}")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetDelayedTripsByRouteId(int routeId)
    {
        var trips = await _tripService.GetDelayedTripsByRouteIdAsync(routeId);
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("AverageDelay/{routeId}")]
    public async Task<ActionResult<double>> GetAverageDelayForRoute(int routeId)
    {
        var averageDelay = await _tripService.GetAverageDelayForRouteAsync(routeId);
        return Ok(averageDelay);
    }

    [HttpGet("Active")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetActiveTrips([FromQuery] DateTime currentTime)
    {
        var trips = await _tripService.GetActiveTripsAsync(currentTime);
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("Paginated")]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetTripsPaginated([FromQuery] int page, [FromQuery] int pageSize)
    {
        var trips = await _tripService.GetTripsPaginatedAsync(page, pageSize);
        return Ok(trips.Select(t => t.ToTripDto()));
    }

    [HttpGet("next-departures/{stopId}")]
    public async Task<ActionResult<IEnumerable<TripForNextDeparturesDto>>> GetNextDeparturesForStop(
        int stopId,
        [FromQuery] DateTime? currentTime = null,
        [FromQuery] int limit = 5)
    {
        currentTime ??= DateTime.UtcNow;

        var tripsWithRouteStops = await _tripService.GetNextDeparturesForStopAsync(stopId, currentTime.Value, limit);

        if (!tripsWithRouteStops.Any())
        {
            return NotFound($"No upcoming trips found for stop ID {stopId}.");
        }

        var result = tripsWithRouteStops
            .Select(t => new TripForNextDeparturesDto(t.Trip, t.RouteStop))
            .ToList();

        return Ok(result);
    }

    [HttpPost("timetable")]
    public async Task<ActionResult<IEnumerable<TimetableResponseDto>>> GetTimetable([FromBody] TimetableRequestDto request)
    {
        var timetable = await _tripService.GetTimetableAsync(request.StartStopId, request.EndStopId, request.DepartureTime);
        return Ok(timetable);
    }
}
