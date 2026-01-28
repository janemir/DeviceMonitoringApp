using DeviceMonitoringApp.Api.Models;
using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DeviceMonitoringApp.Api.Controllers;

/// <summary>
/// Device controller
/// </summary>
[Route("api/device")]
[ApiController]
public class DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger) : ControllerBase
{
    /// <summary>
    /// Adds a new device usage entry.
    /// </summary>
    /// <returns>Created device id.</returns>
    [HttpPost]
    public async Task<IActionResult> AddDeviceAsync([FromBody] Device device) // TODO: Сделать модельку
    {
        logger.LogInformation("Received new device usage record for device {DeviceId} (user: {User})",
            device.Id, device.Name);

        var id = await deviceService.AddAsync(device);

        logger.LogInformation("Successfully stored device usage record for device {DeviceId}", id);

        return Ok(id);
    }

    /// <summary>
    /// Returns all devices (one record per device).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDevicesAsync()
    {
        logger.LogInformation("Request to get all devices");

        var devices = await deviceService.GetAllDevicesAsync();

        var result = devices
            .Select(d => new DeviceDto(d.Id, d.Name, d.Version))
            .ToArray();

        logger.LogInformation("Returned {Count} devices", result.Length);

        return Ok(result);
    }

    /// <summary>
    /// Returns usage statistics for a specific device.
    /// </summary>
    /// <param name="deviceId">Device identifier.</param>
    [HttpGet("{deviceId:guid}/stats")]
    public async Task<IActionResult> GetDeviceStatsAsync(Guid deviceId)
    {
        logger.LogInformation("Request to get stats for device {DeviceId}", deviceId);

        var stats = await deviceService.GetDeviceStatsAsync(deviceId);

        var result = stats
            .Select(d => new DeviceUsageDto(d.Id, d.Name, d.StartTime, d.EndTime, d.Version))
            .ToArray();

        logger.LogInformation("Returned {Count} usage records for device {DeviceId}", result.Length, deviceId);

        return Ok(result);
    }
}