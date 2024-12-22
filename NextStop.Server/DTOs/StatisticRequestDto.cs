using System.ComponentModel.DataAnnotations;

namespace NextStop.Server.DTOs;

public class StatisticRequestDto
{

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "StartDate must be between 1753-01-01 and 9999-12-31.")]
    public required DateTime StartDate { get; set; }

    [Range(typeof(DateTime), "1753-01-01", "9999-12-31", ErrorMessage = "EndDate must be between 1753-01-01 and 9999-12-31.")]
    public required DateTime EndDate { get; set; }

    public int? RouteId { get; set; }
}
