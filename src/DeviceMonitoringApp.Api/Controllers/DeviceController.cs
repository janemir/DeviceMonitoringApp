using Microsoft.AspNetCore.Mvc;
using DeviceMonitoringApp.Application.Interfaces;
using DeviceMonitoringApp.Domain.Entities;

namespace DeviceMonitoringApp.Api.Controllers;

/// <summary>
/// Device controller
/// </summary>
[Route("api/device")]
[ApiController]
public class DeviceController(IDeviceService deviceInterface) : ControllerBase
{
    /// <summary>
    /// Adds a new device usage entry.
    /// </summary>
    /// <returns>Created device id.</returns>
    [HttpPost]
    public async Task<IActionResult> AddDeviceAsync([FromBody] Device device) // TODO: Сделать модельку
    {
        return Ok(await deviceInterface.AddAsync(device));
    }

    /// <summary>
    /// Returns all devices (one record per device).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDevicesAsync()
    {
        var devices = await deviceInterface.GetAllDevicesAsync();
        return Ok(devices);
    }

    /// <summary>
    /// Returns usage statistics for a specific device.
    /// </summary>
    /// <param name="deviceId">Device identifier.</param>
    [HttpGet("{deviceId:guid}/stats")]
    public async Task<IActionResult> GetDeviceStatsAsync(Guid deviceId)
    {
        var stats = await deviceInterface.GetDeviceStatsAsync(deviceId);
        return Ok(stats);
    }
}