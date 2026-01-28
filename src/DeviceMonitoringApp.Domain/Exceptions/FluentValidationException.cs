namespace DeviceMonitoringApp.Domain.Exceptions;

public class FluentValidationException : BadRequestException
{
    public FluentValidationException(string message) : base(message)
    {
    }
}