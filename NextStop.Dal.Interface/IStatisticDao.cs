using NextStop.Domain;

namespace NextStop.Dal.Interface;

public interface IStatisticDao
{
    Task<IEnumerable<DelayStatistic>> GetRouteDelayStatisticsAsync(DateTime startDate, DateTime endDate, int? routeId = null);
}
