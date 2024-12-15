using Microsoft.Data.SqlClient;
using NextStop.Common.Database;
using NextStop.Common.Exceptions;
using NextStop.Dal.Domain;
using NextStop.Dal.Interface;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NextStop.Dal.Ado;

public class AdoHolidayDao(IConnectionFactory connectionFactory, string holidayTableName) : IHolidayDao
{
    private readonly IConnectionFactory connectionFactroy = connectionFactory;
    private readonly AdoTemplate template = new AdoTemplate(connectionFactory);
    private readonly string holidayTableName = holidayTableName;

    private Holiday MapRowToHoliday(IDataRecord row) => new Holiday(
        id: (int)row["Id"],
        name: (string)row["Name"],
        date: (DateTime)row["Date"],
        endDate: row["EndDate"] is DBNull ? null : (DateTime?)row["EndDate"],
        isSchoolHoliday: (bool)row["IsSchoolHoliday"]
    );


    public async Task AddHolidayAsync(Holiday holiday)
    {
        try
        {
            await template.ExecuteAsync($"""
                SET IDENTITY_INSERT {holidayTableName} ON;

                INSERT INTO {holidayTableName} (Id, Name, Date, EndDate, IsSchoolHoliday)
                VALUES (@id, @name, @date, @endDate, @isSchoolHoliday);

                SET IDENTITY_INSERT {holidayTableName} OFF;
            """,
            new QueryParameter("id", holiday.Id),
            new QueryParameter("name", holiday.Name),
            new QueryParameter("date", holiday.Date),
            new QueryParameter("endDate", holiday.EndDate.HasValue ? holiday.EndDate : DBNull.Value),
            new QueryParameter("isSchoolHoliday", holiday.IsSchoolHoliday)
            );
        }
        catch (SqlException ex) when (ex.Number == 2627)
        {
            throw new DuplicateEntityException("Holiday", holiday.Id);
        }
    }

    public async Task DeleteHolidayAsync(int id)
    {
        await template.ExecuteAsync($"DELETE FROM {holidayTableName} WHERE Id=@id", new QueryParameter("id", id));
    }

    public async Task<IEnumerable<Holiday>> GetAllHolidaysAsync()
    {
        return await template.QueryAsync($"SELECT Id, Name, Date, EndDate, IsSchoolHoliday FROM {holidayTableName}", MapRowToHoliday);
    }

    public async Task<Holiday?> GetHolidayByIdAsync(int id)
    {
        return await template.QuerySingleAsync(
            $"SELECT Id, Name, Date, EndDate, IsSchoolHoliday FROM {holidayTableName} WHERE id=@id",
            MapRowToHoliday,
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(DateTime date)
    {
        return await template.QueryAsync($"""
            SELECT Id, Name, Date, EndDate, IsSchoolHoliday FROM {holidayTableName}
                WHERE IsSchoolHoliday = 0 AND (
                    Date = @date OR 
                    (
                        EndDate IS NOT NULL AND 
                        Date <= @date AND @date <= EndDate
                    )
                )
        """, MapRowToHoliday,
        new QueryParameter("date", date));
    }

    public async Task<IEnumerable<Holiday>> GetSchoolHolidaysAsync(DateTime startDate, DateTime endDate)
    {
        return await template.QueryAsync($"""
            SELECT Id, Name, Date, EndDate, IsSchoolHoliday FROM {holidayTableName}
                WHERE IsSchoolHoliday = 1 AND (
                    EndDate >= @startDate AND Date <= @endDate
                )
        """, MapRowToHoliday,
        new QueryParameter("startDate", startDate),
        new QueryParameter("endDate", endDate)
        );
    }

    public async Task UpdateHolidayAsync(Holiday holiday)
    {
        await template.ExecuteAsync($"""
                SET IDENTITY_INSERT {holidayTableName} ON;
                BEGIN TRANSACTION;
                UPDATE {holidayTableName} SET Name=@name, Date=@date, EndDate=@endDate, IsSchoolHoliday=@isSchoolHoliday WHERE Id=@id;
                IF @@ROWCOUNT = 0
                BEGIN
                    INSERT INTO {holidayTableName} (Id, Name, Date, EndDate, IsSchoolHoliday)
                    VALUES (@id, @name, @date, @endDate, @isSchoolHoliday);
                END;
                COMMIT TRANSACTION;
                SET IDENTITY_INSERT {holidayTableName} OFF;
            """,
            new QueryParameter("id", holiday.Id),
            new QueryParameter("name", holiday.Name),
            new QueryParameter("date", holiday.Date),
            new QueryParameter("endDate", holiday.EndDate.HasValue ? holiday.EndDate : DBNull.Value),
            new QueryParameter("isSchoolHoliday", holiday.IsSchoolHoliday)
            );
    }

}
