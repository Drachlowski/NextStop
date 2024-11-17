namespace NextStop.Dal.Domain;

public class Holiday(int id, string name, DateTime date, DateTime? endDate, bool isSchoolHoliday)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime Date { get; set; } = date;
    public DateTime? EndDate { get; set; } = endDate;
    public bool IsSchoolHoliday { get; set; } = isSchoolHoliday;

    public override bool Equals(object? obj)
    {
        if (obj is not Holiday other) return false;

        return Id == other.Id &&
               Name == other.Name &&
               Date == other.Date &&
               EndDate == other.EndDate &&
               IsSchoolHoliday == other.IsSchoolHoliday;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Date, EndDate, IsSchoolHoliday);
    }
}
