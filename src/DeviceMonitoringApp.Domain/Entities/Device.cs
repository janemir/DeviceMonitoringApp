namespace DeviceMonitoringApp.Domain.Entities;

/// <summary>
/// Represents a single device usage record.
/// Matches the message sent by the external application.
/// </summary>
public class Device
{
    /// <summary>
    /// Record/device identifier (_id in JSON).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User name (name).
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Session start time (startTime).
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Session end time (endTime).
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Installed application version (version).
    /// </summary>
    public string Version { get; set; } = null!;
}

