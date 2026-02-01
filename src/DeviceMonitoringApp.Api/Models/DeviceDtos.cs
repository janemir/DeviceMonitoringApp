namespace DeviceMonitoringApp.Api.Models;

/// <summary>
/// DTO для представления устройства в списках
/// </summary>
public record DeviceDto(Guid Id, string Name, string Version,
    DateTime StartTime, DateTime EndTime);

/// <summary>
/// DTO для представления записи об использовании одного устройства
/// </summary>
public record DeviceUsageDto(Guid Id, string Name, DateTime StartTime, DateTime EndTime, string Version);

