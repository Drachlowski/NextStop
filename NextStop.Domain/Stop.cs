namespace NextStop.Domain;

public class Stop
{
    public Stop(int id, string name, string shortName, double latitude, double longitude)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Latitude = latitude;
        Longitude = longitude;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
