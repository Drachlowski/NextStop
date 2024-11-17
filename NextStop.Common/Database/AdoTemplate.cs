using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextStop.Common.Database;

public delegate T rowMapper<T>(IDataRecord row);

public class AdoTemplate(IConnectionFactory connectionFactory)
{
    private readonly IConnectionFactory connectionFactory = connectionFactory;

    private void AddParameters(DbCommand command, QueryParameter[] parameters)
    {
        foreach (var parameter in parameters)
        {
            DbParameter dbParam = command.CreateParameter();
            dbParam.ParameterName = parameter.Name;
            dbParam.Value = parameter.Value;
            command.Parameters.Add(dbParam);
        }
    }

    public IEnumerable<T> Query<T>(string sql, rowMapper<T> mapper, params QueryParameter[] parameters)
    {
        using DbConnection connection = connectionFactory.CreateConnection();

        using DbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameters(command, parameters);

        using DbDataReader reader = command.ExecuteReader();

        var items = new List<T>();
        while (reader.Read())
        {
            items.Add(mapper(reader));
        }

        return items;
    }

    public T? QuerySingle<T>(string sql, rowMapper<T> mapper, params QueryParameter[] parameters)
    {
        return Query(sql, mapper, parameters).SingleOrDefault();
    }

    public int Execute(string sql, params QueryParameter[] parameters)
    {
        using DbConnection connection = connectionFactory.CreateConnection();

        using DbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameters(command, parameters);

        return command.ExecuteNonQuery();
    }
}
