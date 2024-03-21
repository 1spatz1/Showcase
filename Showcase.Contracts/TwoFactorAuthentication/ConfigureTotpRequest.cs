namespace Showcase.Contracts.TwoFactorAuthentication;

public record ConfigureTotpRequest
(
    Guid UserId,
    string Username
);