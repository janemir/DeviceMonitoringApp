using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Application.Services;

public class DeviceService(IDeviceRepository deviceRepository) : IDeviceService
{
    public Task<Guid> AddAsync(Device device)
    {
        // For now the application layer just delegates to the repository.
        // Here we could later add validation, mapping, logging, etc.
        return deviceRepository.AddAsync(device);
    }

    public async Task<IReadOnlyCollection<Device>> GetAllDevicesAsync()
    {
        var allDevices = await deviceRepository.GetAllAsync();

        return allDevices
            .GroupBy(d => d.Id)
            .Select(g => g.OrderByDescending(d => d.StartTime).First())
            .ToList();
    }

    public Task<IReadOnlyCollection<Device>> GetDeviceStatsAsync(Guid deviceId)
    {
        return deviceRepository.GetByDeviceIdAsync(deviceId);
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid deviceId)
    {
        var allRecords = await deviceRepository.GetByDeviceIdAsync(deviceId);
        return allRecords.OrderByDescending(d => d.StartTime).FirstOrDefault();
    }
}