using NextStop.Common.Exceptions;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;
using NextStop.Dal.Simple;
using NextStop.Domain;
using Xunit;

namespace NextStop.Tests.Dal.Simple;

public class SimpleRouteStopDaoTests
{
    private readonly IRouteStopDao _sut;
    private readonly Route _route1;
    private readonly Route _route2;
    private readonly Stop _stop1;
    private readonly Stop _stop2;
    private readonly Stop _stop3;

    public SimpleRouteStopDaoTests()
    {
        _sut = new SimpleRouteStopDao();
        _route1 = new Route(1, "Route A", DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"), "Weekdays");
        _route2 = new Route(2, "Route B", DateTime.Parse("2024-06-01"), null, "Weekends");
        _stop1 = new Stop(1, "Amstetten Hauptbahnhof", "AM-HB", 48.1221, 14.9767);
        _stop2 = new Stop(2, "Amstetten Zentrum", "AM-Z", 48.1234, 14.9750);
        _stop3 = new Stop(3, "Amstetten Süd", "AM-S", 48.1190, 14.9732);
    }

    internal void PrefillRouteStopDao()
    {
        _sut.AddRouteStop(new RouteStop(1, _route1.Id, _stop1.Id, 1, 5, _route1, _stop1));
        _sut.AddRouteStop(new RouteStop(2, _route1.Id, _stop2.Id, 2, 10, _route1, _stop2));
        _sut.AddRouteStop(new RouteStop(3, _route2.Id, _stop3.Id, 1, 15, _route2, _stop3));
    }

    [Fact]
    public void GetAllRouteStops_ShouldReturnEmptyList_WhenNoRouteStopsAreAdded()
    {
        var result = _sut.GetAllRouteStops();
        Assert.NotNull(result);
        Assert.IsType<List<RouteStop>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public void AddRouteStop_ShouldAddRouteStop_WhenRouteStopIsValid()
    {
        var routeStop = new RouteStop(1, _route1.Id, _stop1.Id, 1, 5, _route1, _stop1);
        _sut.AddRouteStop(routeStop);
        var result = _sut.GetAllRouteStops();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(result, rs => rs.RouteId == _route1.Id && rs.StopId == _stop1.Id);
    }

    [Fact]
    public void AddRouteStop_ShouldNotAllowDuplicateId()
    {
        var routeStop1 = new RouteStop(1, _route1.Id, _stop1.Id, 1, 5, _route1, _stop1);
        var routeStop2 = new RouteStop(1, _route1.Id, _stop2.Id, 2, 10, _route1, _stop2);
        _sut.AddRouteStop(routeStop1);

        Assert.Throws<DuplicateEntityException>(() => _sut.AddRouteStop(routeStop2));
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(3, 2, 3)]
    public void GetRouteStopById_ShouldReturnRouteStop_WhenIdIsValid(int id, int expectedRouteId, int expectedStopId)
    {
        PrefillRouteStopDao();
        var result = _sut.GetRouteStopById(id);

        Assert.NotNull(result);
        Assert.Equal(expectedRouteId, result.RouteId);
        Assert.Equal(expectedStopId, result.StopId);
    }

    [Fact]
    public void GetRouteStopById_ShouldReturnNull_WhenIdIsInvalid()
    {
        PrefillRouteStopDao();
        var result = _sut.GetRouteStopById(9999);

        Assert.Null(result);
    }

    [Fact]
    public void GetAllRoutesForStop_ShouldReturnCorrectRoutes_WhenStopIdIsValid()
    {
        PrefillRouteStopDao();
        var result = _sut.GetAllRoutesForStop(_stop1.Id);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, r => r.Id == _route1.Id);
    }

    [Fact]
    public void GetAllRoutesForStop_ShouldReturnEmpty_WhenStopIdIsInvalid()
    {
        PrefillRouteStopDao();
        var result = _sut.GetAllRoutesForStop(9999);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllStopsForRoute_ShouldReturnCorrectStops_WhenRouteIdIsValid()
    {
        PrefillRouteStopDao();
        var result = _sut.GetAllStopsForRoute(_route1.Id);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.Id == _stop1.Id);
        Assert.Contains(result, s => s.Id == _stop2.Id);
    }

    [Fact]
    public void GetAllStopsForRoute_ShouldReturnEmpty_WhenRouteIdIsInvalid()
    {
        PrefillRouteStopDao();
        var result = _sut.GetAllStopsForRoute(9999);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void DeleteRouteStop_ShouldRemoveRouteStop_WhenIdIsValid()
    {
        PrefillRouteStopDao();
        var initialCount = _sut.GetAllRouteStops().Count();

        _sut.DeleteRouteStop(1);

        var result = _sut.GetAllRouteStops();
        Assert.Equal(initialCount - 1, result.Count());
        Assert.Null(_sut.GetRouteStopById(1));
    }

    [Fact]
    public void DeleteRouteStop_ShouldNotChangeList_WhenIdIsInvalid()
    {
        PrefillRouteStopDao();
        var initialCount = _sut.GetAllRouteStops().Count();

        _sut.DeleteRouteStop(9999);

        var result = _sut.GetAllRouteStops();
        Assert.Equal(initialCount, result.Count());
    }

    [Fact]
    public void UpdateRouteStop_ShouldModifyRouteStop_WhenIdIsValid()
    {
        PrefillRouteStopDao();
        var updatedRouteStop = new RouteStop(1, _route2.Id, _stop2.Id, 2, 20, _route2, _stop2);

        _sut.UpdateRouteStop(updatedRouteStop);

        var result = _sut.GetRouteStopById(1);
        Assert.NotNull(result);
        Assert.Equal(_route2.Id, result.RouteId);
        Assert.Equal(_stop2.Id, result.StopId);
        Assert.Equal(2, result.StopSequence);
        Assert.Equal(20, result.Scheduled);
    }

    [Fact]
    public void UpdateRouteStop_ShouldAddRouteStop_WhenIdIsInvalid()
    {
        PrefillRouteStopDao();
        var newRouteStop = new RouteStop(9999, _route2.Id, _stop3.Id, 3, 25, _route2, _stop3);

        _sut.UpdateRouteStop(newRouteStop);

        var result = _sut.GetRouteStopById(newRouteStop.Id);
        Assert.NotNull(result);
        Assert.Equal(newRouteStop, result);
    }
}
