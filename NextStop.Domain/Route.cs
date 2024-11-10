namespace NextStop.Domain;

public class Route(int id, string routeName, DateTime validityStartDate, DateTime? validityEndDate, string daysOfOperation)
{
    public int Id { get; set; } = id;
    public string RouteName { get; set; } = routeName;
    public DateTime ValidityStartDate { get; set; } = validityStartDate;
    public DateTime? ValidityEndDate { get; set; } = validityEndDate;
    public string DaysOfOperation { get; set; } = daysOfOperation;
}
