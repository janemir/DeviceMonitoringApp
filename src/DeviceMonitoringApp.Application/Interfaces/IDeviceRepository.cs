using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Application.Interfaces;

public interface IDeviceRepository
{
    /// <summary>
    /// Добавление новой записи об использовании устройства
    /// </summary>
    Task<Guid> AddAsync(Device device);

    /// <summary>
    /// Возвращение всех уникальных устройств
    /// </summary>
    Task<IReadOnlyCollection<Device>> GetAllAsync();

    /// <summary>
    /// Возвращение всех записей об использовании для конкретного устройства
    /// </summary>
    Task<IReadOnlyCollection<Device>> GetByDeviceIdAsync(Guid deviceId);
}

