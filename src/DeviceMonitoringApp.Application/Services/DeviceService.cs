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
}