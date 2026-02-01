namespace DeviceMonitoringApp.Domain.Entities;

public class Device
{
    /// <summary>
    /// id записи устройства
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Время начала сессии
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Время окончания сессии
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Установленная версия
    /// </summary>
    public string Version { get; set; } = null!;
}

