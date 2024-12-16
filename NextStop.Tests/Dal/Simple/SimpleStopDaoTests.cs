//using NextStop.Common.Exceptions;
//using NextStop.Dal.Interface;
//using NextStop.Dal.Simple;
//using NextStop.Domain;

//namespace NextStop.Tests.Dal.Simple;

//public class SimpleStopDaoTests
//{
//    private readonly IStopDao _sut;

//    // Setup
//    public SimpleStopDaoTests()
//    {
//        _sut = new SimpleStopDao();
//    }

//    internal void PrefillStopDao()
//    {
//        _sut.AddStop(new Stop(1, "Amstetten Hauptbahnhof", "AM-HB", 48.1221f, 14.9767f));
//        _sut.AddStop(new Stop(2, "Amstetten Krankenhaus", "AM-KH", 48.1241f, 14.9782f));
//        _sut.AddStop(new Stop(3, "Amstetten Schulzentrum", "AM-SZ", 48.1210f, 14.9800f));
//        _sut.AddStop(new Stop(4, "Amstetten Zentrum", "AM-Z", 48.1234f, 14.9750f));
//        _sut.AddStop(new Stop(5, "Amstetten Süd", "AM-S", 48.1190f, 14.9732f));
//        _sut.AddStop(new Stop(6, "Amstetten Nord", "AM-N", 48.1250f, 14.9775f));
//        _sut.AddStop(new Stop(7, "Amstetten West", "AM-W", 48.1215f, 14.9721f));
//        _sut.AddStop(new Stop(8, "Amstetten Ost", "AM-O", 48.1202f, 14.9790f));
//        _sut.AddStop(new Stop(9, "Amstetten Einkaufszentrum", "AM-EZ", 48.1230f, 14.9740f));
//        _sut.AddStop(new Stop(10, "Amstetten Industriegebiet", "AM-IG", 48.1260f, 14.9820f));
//        _sut.AddStop(new Stop(11, "Amstetten Freibad", "AM-FB", 48.1180f, 14.9770f));
//        _sut.AddStop(new Stop(12, "Amstetten Schule 1", "AM-S1", 48.1245f, 14.9792f));
//        _sut.AddStop(new Stop(13, "Amstetten Schule 2", "AM-S2", 48.1249f, 14.9755f));
//        _sut.AddStop(new Stop(14, "Amstetten Park", "AM-P", 48.1205f, 14.9785f));
//        _sut.AddStop(new Stop(15, "Amstetten Kino", "AM-K", 48.1218f, 14.9760f));
//        _sut.AddStop(new Stop(16, "Amstetten Theater", "AM-T", 48.1222f, 14.9745f));
//        _sut.AddStop(new Stop(17, "Amstetten Rathaus", "AM-RH", 48.1240f, 14.9778f));
//        _sut.AddStop(new Stop(18, "Amstetten Polizeistation", "AM-PS", 48.1212f, 14.9735f));
//        _sut.AddStop(new Stop(19, "Amstetten Bibliothek", "AM-BL", 48.1228f, 14.9764f));
//        _sut.AddStop(new Stop(20, "Amstetten Sportzentrum", "AM-SP", 48.1200f, 14.9712f));
//    }


//    [Fact]
//    public void GetAllStops_ShouldReturnEmptyList_WhenNoStopsAreAdded()
//    {
//        var result = _sut.GetAllStops();
//        Assert.NotNull(result);
//        Assert.IsType<List<Stop>>(result);
//        Assert.Empty(result);
//    }

//    [Fact]
//    public void AddStop_ShouldAddStop_WhenStopIsValid()
//    {
//        Stop stop = new Stop(1, "Amstetten Hauptbahnhof", "AM-HB", 48.1221f, 14.9767f);
//        _sut.AddStop(stop);
//        var result = _sut.GetAllStops();
//        Assert.NotNull(result);
//        Assert.NotEmpty(result);
//        Assert.Equal("Amstetten Hauptbahnhof", result.First().Name);
//    }

//    [Fact]
//    public void AddStop_ShouldNotAllowDuplicateId()
//    {
//        Stop stop1 = new Stop(1, "Amstetten Hauptbahnhof", "AM-HB", 48.1221f, 14.9767f);
//        Stop stop2 = new Stop(1, "Amstetten Krankenhaus", "AM-KH", 48.1241f, 14.9782f);
//        _sut.AddStop(stop1);

//        Assert.Throws<DuplicateEntityException>(() => _sut.AddStop(stop2));
//    }

//    [Theory]
//    [InlineData(1, "Amstetten Hauptbahnhof")]
//    [InlineData(2, "Amstetten Krankenhaus")]
//    [InlineData(3, "Amstetten Schulzentrum")]
//    public void GetStopById_ShouldReturnStopWithCorrectName_WhenIdIsValid(int id, string expected)
//    {
//        PrefillStopDao();
//        var result = _sut.GetStopById(id);

//        Assert.NotNull(result);
//        Assert.Equal(expected, result.Name);
//    }

//    [Fact]
//    public void GetStopById_ShouldReturnNull_WhenIdIsInvalid()
//    {
//        PrefillStopDao();
//        var result = _sut.GetStopById(9999);

//        Assert.Null(result);
//    }

//    [Theory]
//    [InlineData("Amstetten Hauptbahnhof", 1)]
//    [InlineData("Amstetten Zentrum", 1)]
//    [InlineData("Amstetten", 20)]
//    public void GetStopsByName_ShouldReturnMatchingStops_WhenNamePartiallyMatches(string name, int expectedCount)
//    {
//        PrefillStopDao();
//        var result = _sut.GetStopsByName(name);

//        Assert.NotNull(result);
//        Assert.Equal(expectedCount, result.Count());
//    }

//    [Fact]
//    public void GetStopsByName_ShouldReturnEmpty_WhenNoNameMatches()
//    {
//        PrefillStopDao();
//        var result = _sut.GetStopsByName("Non-existent");

//        Assert.NotNull(result);
//        Assert.Empty(result);
//    }

//    [Theory]
//    [InlineData(48.1221f, 14.9767f, 5, new[] { "AM-HB", "AM-K", "AM-BL", "AM-T", "AM-Z" })]
//    [InlineData(48.1241f, 14.9782f, 5, new[] { "AM-KH", "AM-RH", "AM-S1", "AM-N", "AM-BL" })]
//    [InlineData(48.1205f, 14.9755f, 5, new[] { "AM-K", "AM-PS", "AM-HB", "AM-T", "AM-P" })]
//    public void GetNextStopsByCoordinates_ShouldReturnClosestStops_WhenCoordinatesAreNear(float latitude, float longitude, int expectedCount, string[] expectedShortNames)
//    {
//        PrefillStopDao();
//        var result = _sut.GetNextStopsByCoordinates(latitude, longitude);

//        Assert.NotNull(result);
//        Assert.Equal(expectedCount, result.Count());
//    for (int i = 0; i < expectedCount; i++)
//    {
//        Assert.Equal(expectedShortNames[i], result.ElementAt(i).ShortName);
//    }
//    }

//    [Fact]
//    public void DeleteStop_ShouldRemoveStop_WhenIdIsValid()
//    {
//        PrefillStopDao();
//        var initialCount = _sut.GetAllStops().Count();

//        _sut.DeleteStop(1);

//        var result = _sut.GetAllStops();
//        Assert.Equal(initialCount - 1, result.Count());
//        Assert.Null(_sut.GetStopById(1));
//    }

//    [Fact]
//    public void DeleteStop_ShouldNotChangeList_WhenIdIsInvalid()
//    {
//        PrefillStopDao();
//        var initialCount = _sut.GetAllStops().Count();

//        _sut.DeleteStop(9999);

//        var result = _sut.GetAllStops();
//        Assert.Equal(initialCount, result.Count());
//    }

//    [Fact]
//    public void UpdateStop_ShouldModifyStop_WhenIdIsValid()
//    {
//        PrefillStopDao();
//        var updatedStop = new Stop(1, "Updated Stop", "AM-Updated", 48.1250f, 14.9770f);

//        _sut.UpdateStop(updatedStop);

//        var result = _sut.GetStopById(1);
//        Assert.NotNull(result);
//        Assert.Equal("Updated Stop", result.Name);
//    }

//    [Fact]
//    public void UpdateStop_ShouldAddStop_WhenIdIsInvalid()
//    {
//        PrefillStopDao();
//        var newStop = new Stop(9999, "New Stop", "AM-New", 48.1260f, 14.9780f);

//        _sut.UpdateStop(newStop);

//        var result = _sut.GetStopById(newStop.Id);
//        Assert.NotNull(result);
//        Assert.Equal(result, newStop);
//    }
//}
