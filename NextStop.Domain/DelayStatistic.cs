namespace NextStop.Domain;

public class DelayStatistic
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public double AverageDelay { get; set; }
    public double OnTimePercentage { get; set; } // < 2 min delay
    public double SlightlyDelayedPercentage { get; set; } // 2-5 min delay
    public double DelayedPercentage { get; set; } // 5-10 min delay
    public double HeavilyDelayedPercentage { get; set; } // > 10 min delay
}

