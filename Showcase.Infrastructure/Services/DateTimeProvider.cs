using Showcase.Application.Common.Interfaces.Services;

namespace Showcase.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public ulong UtcNowUnixTimeSeconds => (ulong) UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    public ulong UtcNowUnixTimeMilliseconds => (ulong) UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
}