namespace Showcase.Application.Common.Interfaces.Services;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
    public ulong UtcNowUnixTimeSeconds { get; }
    public ulong UtcNowUnixTimeMilliseconds { get; }
}