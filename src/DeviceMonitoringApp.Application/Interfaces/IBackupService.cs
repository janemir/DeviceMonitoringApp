namespace DeviceMonitoringApp.Application.Interfaces;

public interface IBackupService
{
    /// <summary>
    /// Создание резервной копию данных устройств в JSON файл
    /// </summary>
    Task CreateBackupAsync();
    string GetLastBackupInfo();
}
