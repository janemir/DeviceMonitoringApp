using DeviceMonitoringApp.Application.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DeviceMonitoringApp.Application.Services;

public class BackupService : IBackupService
{
    private readonly IDeviceRepository _repository;
    private readonly ILogger<BackupService> _logger;
    private Timer? _backupTimer;
    private DateTime? _lastBackupTime;
    private string? _lastBackupPath;
    public BackupService(IDeviceRepository repository, ILogger<BackupService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task CreateBackupAsync()
    {
        try
        {
            var allDevices = await _repository.GetAllAsync();

            var backupData = new
            {
                BackupDate = DateTime.UtcNow,
                TotalRecords = allDevices.Count,
                Devices = allDevices
            };

            var json = JsonSerializer.Serialize(backupData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var backupDir = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
            Directory.CreateDirectory(backupDir);

            var fileName = $"backup_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(backupDir, fileName);

            await File.WriteAllTextAsync(filePath, json);

            _logger.LogInformation("Backup created successfully: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create backup");
            throw;
        }

    }

    public string GetLastBackupInfo()
    {
        if (_lastBackupTime == null)
            return "No backups created yet";

        var fileInfo = new FileInfo(_lastBackupPath!);
        return $"Last backup: {_lastBackupTime:yyyy-MM-dd HH:mm:ss}, Path: {_lastBackupPath}, Size: {fileInfo.Length} bytes";
    }
}