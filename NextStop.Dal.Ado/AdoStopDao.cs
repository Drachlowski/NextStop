using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NextStop.Common.Database;
using NextStop.Domain;
using NextStop.Dal.Interface;

namespace NextStop.Dal.Ado;

public class AdoStopDao : IStopDao
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly AdoTemplate _template;

    public AdoStopDao(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _template = new AdoTemplate(connectionFactory);
    }

    private Stop MapRowToStop(IDataRecord row) => new Stop(
        id: (int)row["Id"],
        name: (string)row["Name"],
        shortName: (string)row["ShortName"],
        latitude: (double)row["Latitude"],
        longitude: (double)row["Longitude"]
    );

    public async Task<Stop?> GetStopByIdAsync(int id)
    {
        return await _template.QuerySingleAsync(
            "SELECT Id, Name, ShortName, Latitude, Longitude FROM Stop WHERE Id = @id",
            MapRowToStop,
            new QueryParameter("id", id)
        );
    }

    public async Task<IEnumerable<Stop>> GetAllStopsAsync()
    {
        return await _template.QueryAsync(
            "SELECT Id, Name, ShortName, Latitude, Longitude FROM Stop",
            MapRowToStop
        );
    }

    public async Task<IEnumerable<Stop>> GetStopsByNameAsync(string name)
    {
        return await _template.QueryAsync(
            "SELECT Id, Name, ShortName, Latitude, Longitude FROM Stop WHERE Name LIKE @name",
            MapRowToStop,
            new QueryParameter("name", $"%{name}%")
        );
    }

    public async Task<IEnumerable<Stop>> GetNextStopsByCoordinatesAsync(double latitude, double longitude)
    {
        return await _template.QueryAsync(
            @"SELECT Id, Name, ShortName, Latitude, Longitude,
                   (ABS(Latitude - @latitude) + ABS(Longitude - @longitude)) AS Distance
                  FROM Stop
                  ORDER BY Distance ASC",
            MapRowToStop,
            new QueryParameter("latitude", latitude),
            new QueryParameter("longitude", longitude)
        );
    }

    public async Task AddStopAsync(Stop stop)
    {
        await _template.ExecuteAsync(
            """
            SET IDENTITY_INSERT Stop ON;

            INSERT INTO Stop (Id, Name, ShortName, Latitude, Longitude) VALUES (@id, @name, @shortName, @latitude, @longitude);

            SET IDENTITY_INSERT Stop OFF;
            """,
            new QueryParameter("id", stop.Id),
            new QueryParameter("name", stop.Name),
            new QueryParameter("shortName", stop.ShortName),
            new QueryParameter("latitude", stop.Latitude),
            new QueryParameter("longitude", stop.Longitude)
        );
    }

    public async Task UpdateStopAsync(Stop stop)
    {
        await _template.ExecuteAsync(
            "UPDATE Stop SET Name = @name, ShortName = @shortName, Latitude = @latitude, Longitude = @longitude WHERE Id = @id",
            new QueryParameter("id", stop.Id),
            new QueryParameter("name", stop.Name),
            new QueryParameter("shortName", stop.ShortName),
            new QueryParameter("latitude", stop.Latitude),
            new QueryParameter("longitude", stop.Longitude)
        );
    }

    public async Task DeleteStopAsync(int id)
    {
        await _template.ExecuteAsync(
            "DELETE FROM Stop WHERE Id = @id",
            new QueryParameter("id", id)
        );
    }
}
