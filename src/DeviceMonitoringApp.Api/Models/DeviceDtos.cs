namespace DeviceMonitoringApp.Api.Models;

/// <summary>
/// DTO used to represent a device in lists.
/// </summary>
public record DeviceDto(Guid Id, string Name, string Version,
    DateTime StartTime, DateTime EndTime);

/// <summary>
/// DTO used to represent a single device usage record.
/// </summary>
public record DeviceUsageDto(Guid Id, string Name, DateTime StartTime, DateTime EndTime, string Version);

