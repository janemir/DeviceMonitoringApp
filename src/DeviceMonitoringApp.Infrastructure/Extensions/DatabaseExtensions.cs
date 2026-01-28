using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceMonitoringApp.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Do database migration
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    /// <returns><see cref="IConfiguration"/>.</returns>
    public static IServiceCollection MigrateDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DatabaseConnection");

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .WithVariablesDisabled()
            .LogToConsole()
            .Build();

        upgrader.PerformUpgrade();

        return services;
    }
}