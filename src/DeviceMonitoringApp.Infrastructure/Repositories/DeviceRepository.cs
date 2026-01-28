using System.Data;
using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DeviceMonitoringApp.Infrastructure.Repositories;

public class DeviceRepository(IConfiguration configuration) : IDeviceRepository
{
    private readonly string _connectionString =
        configuration.GetConnectionString("DatabaseConnection")
        ?? throw new InvalidOperationException("DatabaseConnection connection string is not configured.");

    public async Task<Guid> AddAsync(Device device)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        const string sql = """
                           INSERT INTO device_usage (device_id, user_name, start_time, end_time, app_version)
                           VALUES (@device_id, @user_name, @start_time, @end_time, @app_version);
                           """;

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("device_id", device.Id);
        command.Parameters.AddWithValue("user_name", device.Name);
        command.Parameters.AddWithValue("start_time", device.StartTime);
        command.Parameters.AddWithValue("end_time", device.EndTime);
        command.Parameters.AddWithValue("app_version", device.Version);

        await command.ExecuteNonQueryAsync();

        return device.Id;
    }

    public async Task<IReadOnlyCollection<Device>> GetAllAsync()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        // Return one record per device (latest by start_time) to represent the device in the list.
        const string sql = """
                           SELECT DISTINCT ON (device_id)
                               device_id,
                               user_name,
                               start_time,
                               end_time,
                               app_version
                           FROM device_usage
                           ORDER BY device_id, start_time DESC;
                           """;

        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        var result = new List<Device>();
        while (await reader.ReadAsync())
        {
            result.Add(MapDevice(reader));
        }

        return result;
    }

    public async Task<IReadOnlyCollection<Device>> GetByDeviceIdAsync(Guid deviceId)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        const string sql = """
                           SELECT device_id,
                                  user_name,
                                  start_time,
                                  end_time,
                                  app_version
                           FROM device_usage
                           WHERE device_id = @device_id
                           ORDER BY start_time;
                           """;

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("device_id", deviceId);

        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        var result = new List<Device>();
        while (await reader.ReadAsync())
        {
            result.Add(MapDevice(reader));
        }

        return result;
    }

    private static Device MapDevice(IDataRecord record) =>
        new()
        {
            Id = record.GetGuid(record.GetOrdinal("device_id")),
            Name = record.GetString(record.GetOrdinal("user_name")),
            StartTime = record.GetDateTime(record.GetOrdinal("start_time")),
            EndTime = record.GetDateTime(record.GetOrdinal("end_time")),
            Version = record.GetString(record.GetOrdinal("app_version"))
        };

    private NpgsqlConnection CreateConnection() => new(_connectionString);
}

