using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Application.Interfaces;

public interface IDeviceService
{
    /// <summary>
    /// Добавление новой записи об использовании устройства
    /// </summary>
    Task<Guid> AddAsync(Device device);

    /// <summary>
    /// Возвращение одной записи для каждого устройства, чтобы отобразить все устройства
    /// </summary>
    Task<IReadOnlyCollection<Device>> GetAllDevicesAsync();

    /// <summary>
    /// Возвращает все записи об использовании для конкретного устройства
    /// </summary>
    Task<IReadOnlyCollection<Device>> GetDeviceStatsAsync(Guid deviceId);

    /// <summary>
    /// Получение последней запись устройства по id
    /// </summary>
    Task<Device?> GetDeviceByIdAsync(Guid deviceId);
}