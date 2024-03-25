namespace Showcase.Contracts.TwoFactorAuthentication;

public record ConfigureTotpRequest
(
    string UserId = ""
);