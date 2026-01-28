using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Application.Interfaces;

public interface IDeviceService
{
    /// <summary>
    /// Adds a new device usage record.
    /// </summary>
    Task<Guid> AddAsync(Device device);

    /// <summary>
    /// Returns one record per device to display all devices.
    /// </summary>
    Task<IReadOnlyCollection<Device>> GetAllDevicesAsync();

    /// <summary>
    /// Returns all usage records for a specific device.
    /// </summary>
    Task<IReadOnlyCollection<Device>> GetDeviceStatsAsync(Guid deviceId);
}