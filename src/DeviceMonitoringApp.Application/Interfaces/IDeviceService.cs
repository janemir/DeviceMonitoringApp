namespace DeviceMonitoringApp.Application.Interfaces;

public interface IDeviceService
{
    public Task<Guid> AddAsync(Device device);
}