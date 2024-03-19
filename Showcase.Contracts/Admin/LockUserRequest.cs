namespace Showcase.Contracts.Moderation;

public record LockUserRequest(string Email, int DurationDays);