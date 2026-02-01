using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace DeviceMonitoringApp.Infrastructure.Repositories;

public class DeviceRepository(IConfiguration configuration, ILogger<DeviceRepository> logger)
    : IDeviceRepository
{
    private readonly string _connectionString =
        configuration.GetConnectionString("DatabaseConnection")
        ?? throw new InvalidOperationException(
            "DatabaseConnection connection string is not configured.");

    public async Task<Guid> AddAsync(Device device)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string sql = """
                               INSERT INTO device_usage
                                   (device_id, user_name, start_time, end_time, app_version)
                               VALUES
                                   (@Id, @Name, @StartTime, @EndTime, @Version);
                               """;

            var rows = await connection.ExecuteAsync(
                sql,
                device,
                transaction);

            await transaction.CommitAsync();

            logger.LogInformation(
                "Inserted {Rows} row(s) into device_usage for device {DeviceId} (user: {User})",
                rows, device.Id, device.Name);

            return device.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            logger.LogError(
                ex,
                "Failed to insert device usage record for device {DeviceId} (user: {User})",
                device.Id, device.Name);

            throw;
        }
    }

    public async Task<IReadOnlyCollection<Device>> GetAllAsync()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction =
            await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        try
        {
            const string sql = """
                               SELECT DISTINCT ON (device_id)
                                   device_id   AS "Id",
                                   user_name   AS "Name",
                                   start_time  AS "StartTime",
                                   end_time    AS "EndTime",
                                   app_version AS "Version"
                               FROM device_usage
                               ORDER BY device_id, start_time DESC;
                               """;

            var devices = await connection.QueryAsync<Device>(
                sql,
                transaction: transaction);

            await transaction.CommitAsync();

            var result = devices.AsList();

            logger.LogInformation(
                "Loaded {Count} devices (distinct) from database",
                result.Count);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            logger.LogError(ex, "Failed to load devices from database");
            throw;
        }
    }

    public async Task<IReadOnlyCollection<Device>> GetByDeviceIdAsync(Guid deviceId)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await using var transaction =
            await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted);


        try
        {
            const string sql = """
                               SELECT
                                   device_id   AS "Id",
                                   user_name   AS "Name",
                                   start_time  AS "StartTime",
                                   end_time    AS "EndTime",
                                   app_version AS "Version"
                               FROM device_usage
                               WHERE device_id = @DeviceId
                               ORDER BY start_time;
                               """;

            var devices = await connection.QueryAsync<Device>(
                sql,
                new { DeviceId = deviceId },
                transaction);

            await transaction.CommitAsync();

            var result = devices.AsList();

            logger.LogInformation(
                "Loaded {Count} usage records for device {DeviceId}",
                result.Count, deviceId);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            logger.LogError(
                ex,
                "Failed to load usage records for device {DeviceId}",
                deviceId);

            throw;
        }
    }

    private static NpgsqlConnection CreateConnection(string cs) => new(cs);

    private NpgsqlConnection CreateConnection() => new(_connectionString);
}