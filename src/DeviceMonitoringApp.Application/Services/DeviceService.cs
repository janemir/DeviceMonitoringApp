using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Application.Services;

public class DeviceService : IDeviceService
{
    public Task<Guid> AddAsync(Device device)
    {
        throw new NotImplementedException();
    }
}