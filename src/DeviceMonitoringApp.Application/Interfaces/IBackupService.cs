namespace DeviceMonitoringApp.Application.Interfaces;

public interface IBackupService
{
    Task CreateBackupAsync();
    string GetLastBackupInfo();
}
