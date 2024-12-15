using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextStop.Server;
using NextStop.Server.Services;
using Microsoft.Extensions.Configuration;
using NextStop.Common.Database;
using NextStop.Dal.Ado;
using NextStop.Dal.Domain;

namespace NextStop.Tests.Services;

public class HolidayServiceTests : IDisposable
{
    private readonly IHolidayService _sut;
    private IConfiguration configuration;
    private IConnectionFactory connectionFactory;
    private string? holidayTableName;

    internal void createTable()
    {
        using var connection = connectionFactory.CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"""
            CREATE TABLE {holidayTableName} (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                Date DATE NOT NULL,
                EndDate DATE NULL,
                IsSchoolHoliday BIT DEFAULT 0
            );
            """;
        command.ExecuteNonQuery();
    }

    public HolidayServiceTests()
    {
        configuration = ConfigurationUtil.GetConfiguration();
        holidayTableName = configuration["HolidayTableName"] + "ForService";
        if (holidayTableName is null)
        {
            throw new ArgumentException("Configuration property 'HolidayTableName' does not exist");
        }

        connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "NextStopDbConnection", "ProviderName");
        createTable();
        _sut = new HolidayService(connectionFactory, holidayTableName);
    }

    // Teardown
    public void Dispose()
    {
        using var connection = connectionFactory.CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = $"DROP TABLE {holidayTableName};";
        command.ExecuteNonQuery();
    }

    internal async void PrefillHolidayDao()
    {
        await _sut.AddHolidayAsync(new(1, "Weihnachtsfeiertage", DateTime.Parse("2024-12-24"), DateTime.Parse("2024-12-26"), false));
        await _sut.AddHolidayAsync(new(2, "Neujahr", DateTime.Parse("2025-01-01"), null, false));
        await _sut.AddHolidayAsync(new(3, "Weihnachtsferien", DateTime.Parse("2024-12-23"), DateTime.Parse("2025-01-06"), true));
    }


    [Fact]
    public async void GetHolidays_ShouldReturnEmptyList_WhenNoHolidaysAreAdded()
    {
        var result = await _sut.GetHolidaysAsync();
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Holiday>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void AddHoliday_ShouldAddHoliday_WhenHolidayIsValid()
    {
        Holiday holiday = new(1, "Weihnachten", DateTime.Parse("2024-12-24"), null, true);
        await _sut.AddHolidayAsync(holiday);
        var result = await _sut.GetHolidaysAsync();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Weihnachten", result.First().Name);
    }
}
