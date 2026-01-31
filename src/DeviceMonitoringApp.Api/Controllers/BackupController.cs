using DeviceMonitoringApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeviceMonitoringApp.Api.Controllers;

[Route("api/backup")]
[ApiController]
public class BackupController : ControllerBase
{
    private readonly IBackupService _backupService;
    private readonly ILogger<BackupController> _logger;

    public BackupController(IBackupService backupService, ILogger<BackupController> logger)
    {
        _backupService = backupService;
        _logger = logger;
    }

    /// <summary>
    /// Manually trigger a backup
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBackup()
    {
        _logger.LogInformation("Manual backup triggered via API");

        try
        {
            await _backupService.CreateBackupAsync();
            return Ok(new {
                message = "Backup created successfully",
                info = _backupService.GetLastBackupInfo()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create manual backup");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get information about last backup
    /// </summary>
    [HttpGet]
    public IActionResult GetBackupInfo()
    {
        var info = _backupService.GetLastBackupInfo();
        return Ok(new { backupInfo = info });
    }
}