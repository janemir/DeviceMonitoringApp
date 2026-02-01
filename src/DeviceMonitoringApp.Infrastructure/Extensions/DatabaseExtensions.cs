using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceMonitoringApp.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Выполнение миграции БД
    /// </summary>
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