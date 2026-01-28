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
    /// Add product
    /// </summary>
    /// <returns>product Id</returns>
    [HttpPost]
    public async Task<IActionResult> AddDeviceAsync([FromBody] Device device) // TODO: Сделать модельку
    {
        return Ok(await deviceInterface.AddAsync(device));
    }    
}