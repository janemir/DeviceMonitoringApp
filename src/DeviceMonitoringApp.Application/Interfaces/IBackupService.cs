namespace DeviceMonitoringApp.Application.Interfaces;

public interface IBackupService
{
    Task CreateBackupAsync();
    void StartPeriodicBackup(TimeSpan interval = default);
}