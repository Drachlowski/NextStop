using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NextStop.Common.Database;

public class DefaultConnectionFactory : IConnectionFactory
{
    private readonly DbProviderFactory dbProviderFactroy;

    public static IConnectionFactory FromConfiguration(IConfiguration configuration, string connectionConfigName, string providerConfigName)
    {
        (string connectionString, string providerName) =
            ConfigurationUtil.GetConnectionParameters(configuration, connectionConfigName, providerConfigName);
        return new DefaultConnectionFactory(connectionString, providerName);
    }

    public DefaultConnectionFactory(string connectionString, string providerName)
    {
        ConnectionString = connectionString;
        ProviderName = providerName;
        
        DbUtil.RegisterAdoProviders();
        dbProviderFactroy = DbProviderFactories.GetFactory(providerName);
    }

    public string ConnectionString { get; }

    public string ProviderName { get; }

    public DbConnection CreateConnection()
    {
        DbConnection? connection = dbProviderFactroy.CreateConnection();
        if (connection is null)
        {
            throw new InvalidOperationException("DbProviderFactory.CreateConnection() returned null");
        }

        connection.ConnectionString = ConnectionString;

        connection.Open();
        return connection;
    }
}
