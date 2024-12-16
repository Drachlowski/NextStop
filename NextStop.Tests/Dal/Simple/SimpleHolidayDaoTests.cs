using NextStop.Common.Exceptions;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;
using NextStop.Dal.Simple;

namespace NextStop.Tests.Dal.Simple;

public class SimpleHolidayDaoTests
{
    private readonly IHolidayDao _sut;

    // Setup
    public SimpleHolidayDaoTests()
    {
        _sut = new SimpleHolidayDao();
    }

    internal async Task PrefillHolidayDao()
    {
        await _sut.AddHolidayAsync(new(1, "Weihnachtsfeiertage", DateTime.Parse("2024-12-24"), DateTime.Parse("2024-12-26"), false));
        await _sut.AddHolidayAsync(new(2, "Neujahr", DateTime.Parse("2025-01-01"), null, false));
        await _sut.AddHolidayAsync(new(3, "Weihnachtsferien", DateTime.Parse("2024-12-23"), DateTime.Parse("2025-01-06"), true));
    }

    [Fact]
    public async void GetAllHolidays_ShouldReturnEmptyList_WhenNoHolidaysAreAdded()
    {
        var result = await _sut.GetAllHolidaysAsync();
        Assert.NotNull(result);
        Assert.IsType<List<Holiday>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void AddHoliday_ShouldAddHoliday_WhenHolidayIsValid()
    {
        Holiday holiday = new(1, "Weihnachten", DateTime.Parse("2024-12-24"), null, true);
        await _sut.AddHolidayAsync(holiday);
        var result = await _sut.GetAllHolidaysAsync();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Weihnachten", result.First().Name);
    }

    [Fact]
    public async Task AddHoliday_ShouldNotAllowDuplicateId()
    {
        Holiday holiday1 = new(1, "Weihnachten", DateTime.Parse("2024-12-24"), null, true);
        Holiday holiday2 = new(1, "Silvester", DateTime.Parse("2024-12-31"), null, true);
        await _sut.AddHolidayAsync(holiday1);

        await Assert.ThrowsAsync<DuplicateEntityException>(async () => await _sut.AddHolidayAsync(holiday2));
    }

    [Theory]
    [InlineData(1, "Weihnachtsfeiertage")]
    [InlineData(2, "Neujahr")]
    [InlineData(3, "Weihnachtsferien")]
    public async void GetHolidayById_ShouldReturnHolidayWithCorrectName_WhenIdIsValid(int id, string expected)
    {
        await PrefillHolidayDao();
        var result = await _sut.GetHolidayByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(expected, result.Name);
    }

    [Fact]
    public async void GetHolidayById_ShouldReturnNull_WhenIdIsInvalid()
    {
        await PrefillHolidayDao();
        var result = await _sut.GetHolidayByIdAsync(9999);

        Assert.Null(result);
    }

    [Fact]
    public async void GetHolidaysByDate_ShouldReturnCorrectHolidays_WhenDateMatches()
    {
        await PrefillHolidayDao();
        var result = await _sut.GetHolidaysByDateAsync(DateTime.Parse("2024-12-24"));

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Weihnachtsfeiertage", result.First().Name);
    }

    [Fact]
    public async void GetHolidaysByDate_ShouldReturnEmpty_WhenNoDateMatches()
    {
        await PrefillHolidayDao();
        var result = await _sut.GetHolidaysByDateAsync(DateTime.Parse("2024-02-15"));

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("2024-12-24", "2024-12-27", 1)]
    [InlineData("2024-12-01", "2025-01-31", 1)]
    [InlineData("2024-12-01", "2024-12-23", 1)]
    [InlineData("2025-01-06", "2025-01-23", 1)]
    public async void GetSchoolHolidays_ShouldReturnCorrectHolidays_WhenDatesMatch(string start, string end, int expected)
    {
        await PrefillHolidayDao();
        var result = await _sut.GetSchoolHolidaysByDateRangeAsync(DateTime.Parse(start), DateTime.Parse(end));

        Assert.NotNull(result);
        Assert.Equal(expected, result.Count());
        foreach (var item in result)
        {
            Console.WriteLine(item.Name);
        }
    }

    [Fact]
    public async void GetSchoolHolidays_ShouldReturnEmpty_WhenNoSchoolHolidaysInRange()
    {
        await PrefillHolidayDao();
        var result = await _sut.GetSchoolHolidaysByDateRangeAsync(DateTime.Parse("2024-06-01"), DateTime.Parse("2024-06-15"));

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void DeleteHoliday_ShouldRemoveHoliday_WhenIdIsValid()
    {
        await PrefillHolidayDao();
        var initialCount = (await _sut.GetAllHolidaysAsync()).Count();

        await _sut.DeleteHolidayAsync(1);

        var result = await _sut.GetAllHolidaysAsync();
        Assert.Equal(initialCount - 1, result.Count());
        Assert.Null(await _sut.GetHolidayByIdAsync(1));
    }

    [Fact]
    public async void DeleteHoliday_ShouldNotChangeList_WhenIdIsInvalid()
    {
        await PrefillHolidayDao();
        var initialCount = (await _sut.GetAllHolidaysAsync()).Count();

        await _sut.DeleteHolidayAsync(9999);

        var result = await _sut.GetAllHolidaysAsync();
        Assert.Equal(initialCount, result.Count());
    }

    [Fact]
    public async void UpdateHoliday_ShouldModifyHoliday_WhenIdIsValid()
    {
        await PrefillHolidayDao();
        var updatedHoliday = new Holiday(1, "Updated Holiday", DateTime.Parse("2024-12-25"), DateTime.Parse("2024-12-26"), true);

        await _sut.UpdateHolidayAsync(updatedHoliday);

        var result = await _sut.GetHolidayByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Updated Holiday", result.Name);
    }

    [Fact]
    public async void UpdateHoliday_ShouldAddHoliday_WhenIdIsInvalid()
    {
        await PrefillHolidayDao();
        var nonExistentHoliday = new Holiday(9999, "Non-existent Holiday", DateTime.Parse("2024-12-25"), DateTime.Parse("2024-12-26"), true);
        await _sut.UpdateHolidayAsync(nonExistentHoliday);

        var result = await _sut.GetHolidayByIdAsync(nonExistentHoliday.Id);
        Assert.NotNull(result);
        Assert.Equal(result, nonExistentHoliday);
    }
}
