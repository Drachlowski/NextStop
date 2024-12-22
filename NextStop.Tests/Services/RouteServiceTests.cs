using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NextStop.Server.Services;
using NextStop.Common.Database;
using NextStop.Dal.Ado;
using NextStop.Domain;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NextStop.Tests.Services;

public class RouteServiceTests : IDisposable
{
    private readonly IRouteService _sut;
    private readonly IConfiguration _configuration;
    private readonly IConnectionFactory _connectionFactory;
    private readonly string _routeTableName;
    private readonly string _tripCheckInTableName;
    private readonly string _tripTableName;
    private readonly string _routeStopTableName;
    private readonly string _stopTableName;

    internal void CreateTable()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            CREATE TABLE {_routeTableName} (
                Id INT PRIMARY KEY IDENTITY(1,1),
                RouteName NVARCHAR(100) NOT NULL,
                ValidityStartDate DATE NOT NULL,
                ValidityEndDate DATE NULL,
                DaysOfOperation NVARCHAR(50) NULL
            );
        """;
        command.ExecuteNonQuery();
    }

    public RouteServiceTests()
    {
        _configuration = ConfigurationUtil.GetConfiguration();
        _routeTableName = _configuration["RouteTableName"] + "ForService";
        _tripCheckInTableName = _configuration["TripCheckInTableName"] + "ForService";
        _tripTableName = _configuration["TripTableName"] + "ForService";
        _routeStopTableName = _configuration["RouteStopTableName"] + "ForService";
        _stopTableName = _configuration["StopTableName"] + "ForService";
        _connectionFactory = DefaultConnectionFactory.FromConfiguration(_configuration, "NextStopDbConnection", "ProviderName");

        CreateTable();

        var routeDao = new AdoRouteDao(_connectionFactory, _routeTableName);
        var tripCheckInDao = new AdoTripCheckInDao(_connectionFactory, _tripCheckInTableName);
        var routeStopDao = new AdoRouteStopDao(_connectionFactory, _routeStopTableName, _routeTableName, _stopTableName);
        var statisticDao = new AdoStatisticDao(_connectionFactory, _tripCheckInTableName,_tripTableName, _routeStopTableName, _routeTableName );

        _sut = new RouteService(routeDao, tripCheckInDao, routeStopDao, statisticDao);
    }

    public void Dispose()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"DROP TABLE {_routeTableName};";
        command.ExecuteNonQuery();
    }

    internal async Task PrefillRouteDataAsync()
    {
        await _sut.AddRouteAsync(new(1, "Route 1: Bahnhof - Greinsfurth - Euratsfeld", DateTime.Parse("2024-11-17"), null, "Mo-Fr"));
        await _sut.AddRouteAsync(new(2, "Route 2: Bahnhof - Hauptplatz - Neuhofen", DateTime.Parse("2024-11-17"), null, "Mo-So"));
    }

    [Fact]
    public async Task GetAllRoutes_ShouldReturnEmptyList_WhenNoRoutesAreAdded()
    {
        var result = await _sut.GetAllRoutesAsync();
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Route>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddRoute_ShouldAddRoute_WhenRouteIsValid()
    {
        var route = new Route(1, "Route 1: Test", DateTime.Parse("2024-11-17"), null, "Mo-Fr");
        await _sut.AddRouteAsync(route);

        var result = await _sut.GetAllRoutesAsync();
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Route 1: Test", result.First().RouteName);
    }

    [Fact]
    public async Task DeleteRoute_ShouldRemoveRoute_WhenRouteExists()
    {
        await PrefillRouteDataAsync();

        var routesBeforeDelete = await _sut.GetAllRoutesAsync();
        Assert.Equal(2, routesBeforeDelete.Count());

        await _sut.DeleteRouteAsync(1);

        var routesAfterDelete = await _sut.GetAllRoutesAsync();
        Assert.Single(routesAfterDelete);
        Assert.DoesNotContain(routesAfterDelete, r => r.Id == 1);
    }

    [Fact]
    public async Task GetRouteById_ShouldReturnCorrectRoute_WhenRouteExists()
    {
        await PrefillRouteDataAsync();

        var result = await _sut.GetRouteByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Route 1: Bahnhof - Greinsfurth - Euratsfeld", result!.RouteName);
    }

    //[Fact]
    //public async Task HasLinkedStops_ShouldReturnFalse_WhenRouteHasNoLinkedStops()
    //{
    //    await PrefillRouteDataAsync();

    //    var result = await _sut.HasLinkedStopsAsync(1);

    //    Assert.False(result);
    //}
}