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

    internal void PrefillHolidayDao()
    {
        _sut.AddHoliday(new(1, "Weihnachtsfeiertage", DateTime.Parse("2024-12-24"), DateTime.Parse("2024-12-26"), false));
        _sut.AddHoliday(new(2, "Neujahr", DateTime.Parse("2025-01-01"), null, false));
        _sut.AddHoliday(new(3, "Weihnachtsferien", DateTime.Parse("2024-12-23"), DateTime.Parse("2025-01-06"), true));
    }

    [Fact]
    public void GetAllHolidays_ShouldReturnEmptyList_WhenNoHolidaysAreAdded()
    {
        var result = _sut.GetAllHolidays();
        Assert.NotNull(result);
        Assert.IsType<List<Holiday>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public void AddHoliday_ShouldAddHoliday_WhenHolidayIsValid()
    {
        Holiday holiday = new(1, "Weihnachten", DateTime.Parse("2024-12-24"), null, true);
        _sut.AddHoliday(holiday);
        var result = _sut.GetAllHolidays();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Weihnachten", result.First().Name);
    }

    [Fact]
    public void AddHoliday_ShouldNotAllowDuplicateId()
    {
        Holiday holiday1 = new(1, "Weihnachten", DateTime.Parse("2024-12-24"), null, true);
        Holiday holiday2 = new(1, "Silvester", DateTime.Parse("2024-12-31"), null, true);
        _sut.AddHoliday(holiday1);

        Assert.Throws<DuplicateEntityException>(() => _sut.AddHoliday(holiday2));
    }

    [Theory]
    [InlineData(1, "Weihnachtsfeiertage")]
    [InlineData(2, "Neujahr")]
    [InlineData(3, "Weihnachtsferien")]
    public void GetHolidayById_ShouldReturnHolidayWithCorrectName_WhenIdIsValid(int id, string expected)
    {
        PrefillHolidayDao();
        var result = _sut.GetHolidayById(id);

        Assert.NotNull(result);
        Assert.Equal(expected, result.Name);
    }

    [Fact]
    public void GetHolidayById_ShouldReturnNull_WhenIdIsInvalid()
    {
        PrefillHolidayDao();
        var result = _sut.GetHolidayById(9999);

        Assert.Null(result);
    }

    [Fact]
    public void GetHolidaysByDate_ShouldReturnCorrectHolidays_WhenDateMatches()
    {
        PrefillHolidayDao();
        var result = _sut.GetHolidaysByDate(DateTime.Parse("2024-12-24"));

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Weihnachtsfeiertage", result.First().Name);
    }

    [Fact]
    public void GetHolidaysByDate_ShouldReturnEmpty_WhenNoDateMatches()
    {
        PrefillHolidayDao();
        var result = _sut.GetHolidaysByDate(DateTime.Parse("2024-02-15"));

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("2024-12-24", "2024-12-27", 1)]
    [InlineData("2024-12-01", "2025-01-31", 1)]
    [InlineData("2024-12-01", "2024-12-23", 1)]
    [InlineData("2025-01-06", "2025-01-23", 1)]
    public void GetSchoolHolidays_ShouldReturnCorrectHolidays_WhenDatesMatch(string start, string end, int expected)
    {
        PrefillHolidayDao();
        var result = _sut.GetSchoolHolidays(DateTime.Parse(start), DateTime.Parse(end));

        Assert.NotNull(result);
        Assert.Equal(expected, result.Count());
        foreach (var item in result)
        {
            Console.WriteLine(item.Name);
        }
    }

    [Fact]
    public void GetSchoolHolidays_ShouldReturnEmpty_WhenNoSchoolHolidaysInRange()
    {
        PrefillHolidayDao();
        var result = _sut.GetSchoolHolidays(DateTime.Parse("2024-06-01"), DateTime.Parse("2024-06-15"));

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void DeleteHoliday_ShouldRemoveHoliday_WhenIdIsValid()
    {
        PrefillHolidayDao();
        var initialCount = _sut.GetAllHolidays().Count();

        _sut.DeleteHoliday(1);

        var result = _sut.GetAllHolidays();
        Assert.Equal(initialCount - 1, result.Count());
        Assert.Null(_sut.GetHolidayById(1));
    }

    [Fact]
    public void DeleteHoliday_ShouldNotChangeList_WhenIdIsInvalid()
    {
        PrefillHolidayDao();
        var initialCount = _sut.GetAllHolidays().Count();

        _sut.DeleteHoliday(9999);

        var result = _sut.GetAllHolidays();
        Assert.Equal(initialCount, result.Count());
    }

    [Fact]
    public void UpdateHoliday_ShouldModifyHoliday_WhenIdIsValid()
    {
        PrefillHolidayDao();
        var updatedHoliday = new Holiday(1, "Updated Holiday", DateTime.Parse("2024-12-25"), DateTime.Parse("2024-12-26"), true);

        _sut.UpdateHoliday(updatedHoliday);

        var result = _sut.GetHolidayById(1);
        Assert.NotNull(result);
        Assert.Equal("Updated Holiday", result.Name);
    }

    [Fact]
    public void UpdateHoliday_ShouldAddHoliday_WhenIdIsInvalid()
    {
        PrefillHolidayDao();
        var nonExistentHoliday = new Holiday(9999, "Non-existent Holiday", DateTime.Parse("2024-12-25"), DateTime.Parse("2024-12-26"), true);
        _sut.UpdateHoliday(nonExistentHoliday);

        var result = _sut.GetHolidayById(nonExistentHoliday.Id);
        Assert.NotNull(result);
        Assert.Equal(result, nonExistentHoliday);
    }
}
