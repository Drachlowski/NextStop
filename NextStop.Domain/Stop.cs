namespace NextStop.Domain;

public class Stop(int id, string name, string shortName, double latitude, double longitude)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string ShortName { get; set; } = shortName;
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
}
