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
    /// Запуск резервного копировния
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBackup()
    {
        _logger.LogInformation("Ручное резервное копирование запущено через API");

        try
        {
            await _backupService.CreateBackupAsync();
            return Ok(new {
                message = "Резервная копия успешно создана",
                info = _backupService.GetLastBackupInfo()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось создать резервную копию вручную");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Получение информации о последнем резервном копировании
    /// </summary>
    [HttpGet]
    public IActionResult GetBackupInfo()
    {
        var info = _backupService.GetLastBackupInfo();
        return Ok(new { backupInfo = info });
    }
}