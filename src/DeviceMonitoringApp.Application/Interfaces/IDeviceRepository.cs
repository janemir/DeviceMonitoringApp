using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Application.Interfaces;

public interface IDeviceRepository
{
    /// <summary>
    /// Adds a new device usage record.
    /// </summary>
    /// <param name="device">Usage record to add.</param>
    /// <returns>Identifier of the created record (device id from the message).</returns>
    Task<Guid> AddAsync(Device device);

    /// <summary>
    /// Returns all distinct devices.
    /// </summary>
    /// <returns>All devices (at least one record per device).</returns>
    Task<IReadOnlyCollection<Device>> GetAllAsync();

    /// <summary>
    /// Returns all usage records for a specific device.
    /// </summary>
    /// <param name="deviceId">Device identifier.</param>
    Task<IReadOnlyCollection<Device>> GetByDeviceIdAsync(Guid deviceId);
}

