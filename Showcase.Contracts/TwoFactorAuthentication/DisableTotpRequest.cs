namespace Showcase.Contracts.TwoFactorAuthentication;

public record DisableTotpRequest
(
    Guid UserId,
    string Username
);