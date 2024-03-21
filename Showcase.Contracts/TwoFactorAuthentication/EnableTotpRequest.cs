namespace Showcase.Contracts.TwoFactorAuthentication;

public record EnableTotpRequest
(
    Guid UserId,
    string Username,
    string Token
);