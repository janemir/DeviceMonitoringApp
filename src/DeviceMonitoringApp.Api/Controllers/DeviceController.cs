using DeviceMonitoringApp.Api.Models;
using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DeviceMonitoringApp.Api.Controllers;

/// <summary>
/// Контроллер устройства
/// </summary>
[Route("api/device")]
[ApiController]
public class DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger) : ControllerBase
{
    /// <summary>
    /// Добавление новой записи об использовании устройства
    /// </summary>
    /// <returns> id созданного устройства </returns>
    [HttpPost]
    public async Task<IActionResult> AddDeviceAsync([FromBody] Device device)
    {
        logger.LogInformation("Получена новая запись об использовании устройства {DeviceId} (Пользователь: {User})",
            device.Id, device.Name);

        var id = await deviceService.AddAsync(device);

        logger.LogInformation("Запись об использовании устройства {DeviceId} успешно созранена", id);

        return Ok(id);
    }

    /// <summary>
    /// Возвращение по одной записи на каждое устройство
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDevicesAsync()
    {
        logger.LogInformation("Запрос на получение информации обо всех устройствах");

        var devices = await deviceService.GetAllDevicesAsync();

        var result = devices
            .Select(d => new DeviceDto(d.Id, d.Name, d.Version, d.StartTime, d.EndTime))
            .ToArray();

        logger.LogInformation("Устройств возвращено: {Count} ", result.Length);

        return Ok(result);
    }

    [HttpGet("{deviceId:guid}")]
    public async Task<IActionResult> GetDeviceByIdAsync(Guid deviceId)
    {
        logger.LogInformation("Запрос на получение устройства по id: {DeviceId}", deviceId);

        var device = await deviceService.GetDeviceByIdAsync(deviceId);

        if (device == null)
        {
            logger.LogWarning("Устройство с id {DeviceId} не найдено", deviceId);
            return NotFound();
        }

        var result = new DeviceDto(device.Id, device.Name, device.Version,
            device.StartTime, device.EndTime);

        logger.LogInformation("Возвращено устройство с id {DeviceId}", deviceId);

        return Ok(result);
    }

    /// <summary>
    /// Возвращение статистики использования конкретного устройства
    /// </summary>
    /// <param name="deviceId">Идентификатор устройства</param>
    [HttpGet("{deviceId:guid}/stats")]
    public async Task<IActionResult> GetDeviceStatsAsync(Guid deviceId)
    {
        logger.LogInformation("Запрос на получение статистики для устройства {DeviceId}", deviceId);

        var stats = await deviceService.GetDeviceStatsAsync(deviceId);

        var result = stats
            .Select(d => new DeviceUsageDto(d.Id, d.Name, d.StartTime, d.EndTime, d.Version))
            .ToArray();

        logger.LogInformation("Возвращено {Count} записей об использовании устройства {DeviceId}", result.Length, deviceId);

        return Ok(result);
    }
}