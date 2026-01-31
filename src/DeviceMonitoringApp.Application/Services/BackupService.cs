using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DeviceMonitoringApp.Application.Services;

public class BackupService : IBackupService
{
    private readonly IDeviceRepository _repository;
    private readonly ILogger<BackupService> _logger;
    private Timer? _backupTimer;
    private readonly TimeSpan _defaultInterval = TimeSpan.FromHours(1);

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

    public void StartPeriodicBackup(TimeSpan interval = default)
    {
        if (interval == default)
            interval = _defaultInterval;

        _backupTimer = new Timer(async _ =>
        {
            try
            {
                await CreateBackupAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during periodic backup");
            }
        }, null, TimeSpan.Zero, interval);

        _logger.LogInformation("Periodic backup started with interval: {Interval}", interval);
    }
}