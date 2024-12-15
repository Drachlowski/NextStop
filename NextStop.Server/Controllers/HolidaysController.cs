using Microsoft.AspNetCore.Mvc;
using NextStop.Dal.Domain;
using NextStop.Server.DTOs;
using NextStop.Server.Mappers;
using NextStop.Server.Services;

namespace NextStop.Server.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class HolidaysController(IHolidayService _holidayService) : ControllerBase
{
    private readonly IHolidayService _holidayService = _holidayService;

    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HolidayDto>>> GetHolidays() => Ok((await _holidayService.GetHolidaysAsync()).Select(c => c.ToHolidayDto()));

    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{holidayId}")]
    public async Task<ActionResult<HolidayDto>> GetHolidayById(int holidayId)
    {
        Holiday? holiday = (await _holidayService.GetHolidayByIdAsync(holidayId));
        if (holiday == null)
        {
            return NotFound();
        }
        return Ok(holiday.ToHolidayDto());
    }

    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<HolidayForCreationDto>> CreateHoliday([FromBody] HolidayForCreationDto holidayForCreationDto)
    {
        if (await _holidayService.HolidayExistsAsync(holidayForCreationDto.Id))
        {
            return Conflict();
        }

        Holiday holiday = holidayForCreationDto.ToHoliday();
        await _holidayService.AddHolidayAsync(holiday);
        return CreatedAtAction(
            actionName: nameof(GetHolidayById),
            routeValues: new { holidayId = holiday.Id },
            value: holiday.ToHolidayDto()
        );
    }

    [HttpPut("{holidayId}")]
    public async Task<ActionResult> UpdateHoliday(int holidayId, [FromBody] HolidayForUpdateDto holidayForCreationDto)
    {
        Holiday? holiday = await _holidayService.GetHolidayByIdAsync(holidayId);

        if (holiday is null) return NotFound();

        holidayForCreationDto.UpdateHoliday(holiday);
        await _holidayService.UpdateHolidayAsync(holiday);
        return NoContent();
    }

    [HttpDelete("{holidayId}")]
    public async Task<ActionResult> DeleteHoliday(int holidayId)
    {
        if (await _holidayService.GetHolidayByIdAsync(holidayId) is null)
        {
            return NotFound();
        }

        await _holidayService.DeleteHolidayAsync(holidayId);
        return NoContent();
    }
}
