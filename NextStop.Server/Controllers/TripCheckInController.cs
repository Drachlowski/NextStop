using Microsoft.AspNetCore.Mvc;
using NextStop.Dal.Interface;
using NextStop.Server.DTOs.TripCheckIn;
using NextStop.Server.Services;

namespace NextStop.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripCheckInController : ControllerBase
{
    private readonly ITripCheckInService _tripCheckInService;
    private readonly ITripService _tripService;
    private readonly IRouteStopService _routeStopService;

    public TripCheckInController(ITripCheckInService tripCheckInService, ITripService tripService, IRouteStopService routeStopService)
    {
        _tripCheckInService = tripCheckInService;
        _tripService = tripService;
        _routeStopService = routeStopService;
    }

    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCheckIn([FromBody] TripCheckInDto checkInDto)
    {
        var trip = await _tripService.GetTripByIdAsync(checkInDto.TripId);
        if (trip is null)
        {
            return NotFound("Trip not found");
        }
        var routeStop = await _routeStopService.GetRouteStopByIdAsync(checkInDto.RouteStopId);
        if (routeStop is null)
        {
            return NotFound("RouteStop not found");
        }

        await _tripCheckInService.AddCheckInAsync(trip, routeStop, checkInDto.CheckInTime);
        return Ok();
    }

    [HttpGet("{tripId}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCheckInsForTrip(int tripId)
    {
        var checkIns = await _tripCheckInService.GetCheckInsForTripAsync(tripId);
        return Ok(checkIns);
    }

    [HttpGet("{tripId}/latest")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLatestCheckIn(int tripId)
    {
        var latestCheckIn = await _tripCheckInService.GetLatestCheckInForTripAsync(tripId);
        return latestCheckIn is null ? NotFound() : Ok(latestCheckIn);
    }
}
