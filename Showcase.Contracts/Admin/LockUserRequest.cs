namespace Showcase.Contracts.Admin;

public record LockUserRequest(string Email, int DurationDays);