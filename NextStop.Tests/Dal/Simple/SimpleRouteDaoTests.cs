using NextStop.Common.Exceptions;
using NextStop.Dal.Interface;
using NextStop.Dal.Simple;
using NextStop.Domain;

namespace NextStop.Tests.Dal.Simple;

public class SimpleRouteDaoTests
{
    private readonly IRouteDao _sut;

    // Setup
    public SimpleRouteDaoTests()
    {
        _sut = new SimpleRouteDao();
    }

    internal void PrefillRouteDao()
    {
        _sut.AddRoute(new Route(1, "Route A", DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"), "Weekdays"));
        _sut.AddRoute(new Route(2, "Route B", DateTime.Parse("2024-06-01"), null, "Weekends"));
        _sut.AddRoute(new Route(3, "Route C", DateTime.Parse("2024-01-01"), DateTime.Parse("2024-03-31"), "Holidays"));
    }

    [Fact]
    public void GetAllRoutes_ShouldReturnEmptyList_WhenNoRoutesAreAdded()
    {
        var result = _sut.GetAllRoutes();
        Assert.NotNull(result);
        Assert.IsType<List<Route>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public void AddRoute_ShouldAddRoute_WhenRouteIsValid()
    {
        Route route = new Route(1, "Route A", DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"), "Weekdays");
        _sut.AddRoute(route);
        var result = _sut.GetAllRoutes();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Route A", result.First().RouteName);
    }

    [Fact]
    public void AddRoute_ShouldNotAllowDuplicateId()
    {
        Route route1 = new Route(1, "Route A", DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"), "Weekdays");
        Route route2 = new Route(1, "Route B", DateTime.Parse("2024-06-01"), null, "Weekends");
        _sut.AddRoute(route1);

        Assert.Throws<DuplicateEntityException>(() => _sut.AddRoute(route2));
    }

    [Theory]
    [InlineData(1, "Route A")]
    [InlineData(2, "Route B")]
    [InlineData(3, "Route C")]
    public void GetRouteById_ShouldReturnRouteWithCorrectName_WhenIdIsValid(int id, string expected)
    {
        PrefillRouteDao();
        var result = _sut.GetRouteById(id);

        Assert.NotNull(result);
        Assert.Equal(expected, result.RouteName);
    }

    [Fact]
    public void GetRouteById_ShouldReturnNull_WhenIdIsInvalid()
    {
        PrefillRouteDao();
        var result = _sut.GetRouteById(9999);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("2024-01-15", 2)]
    [InlineData("2024-07-01", 2)]
    [InlineData("2024-04-01", 1)]
    [InlineData("2023-12-31", 0)]
    public void GetActiveRoutes_ShouldReturnCorrectRoutes_WhenDateIsWithinValidity(string date, int expected)
    {
        PrefillRouteDao();
        var result = _sut.GetActiveRoutes(DateTime.Parse(date));

        Assert.NotNull(result);
        Assert.Equal(expected, result.Count());
    }

    [Fact]
    public void GetActiveRoutes_ShouldReturnEmpty_WhenDateIsOutOfRange()
    {
        PrefillRouteDao();
        var result = _sut.GetActiveRoutes(DateTime.Parse("2023-01-01"));

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void DeleteRoute_ShouldRemoveRoute_WhenRouteIsValid()
    {
        PrefillRouteDao();
        var initialCount = _sut.GetAllRoutes().Count();

        _sut.DeleteRoute(1);

        var result = _sut.GetAllRoutes();
        Assert.Equal(initialCount - 1, result.Count());
        Assert.Null(_sut.GetRouteById(1));
    }

    [Fact]
    public void DeleteRoute_ShouldNotChangeList_WhenRouteIsInvalid()
    {
        PrefillRouteDao();
        var initialCount = _sut.GetAllRoutes().Count();

        _sut.DeleteRoute(9999);

        var result = _sut.GetAllRoutes();
        Assert.Equal(initialCount, result.Count());
    }

    [Fact]
    public void UpdateRoute_ShouldModifyRoute_WhenIdIsValid()
    {
        PrefillRouteDao();
        var updatedRoute = new Route(1, "Updated Route", DateTime.Parse("2024-01-15"), DateTime.Parse("2024-12-31"), "Weekdays");

        _sut.UpdateRoute(updatedRoute);

        var result = _sut.GetRouteById(1);
        Assert.NotNull(result);
        Assert.Equal("Updated Route", result.RouteName);
    }

    [Fact]
    public void UpdateRoute_ShouldAddRoute_WhenIdIsInvalid()
    {
        PrefillRouteDao();
        var newRoute = new Route(9999, "New Route", DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"), "Holidays");

        _sut.UpdateRoute(newRoute);

        var result = _sut.GetRouteById(newRoute.Id);
        Assert.NotNull(result);
        Assert.Equal(result, newRoute);
    }
}
