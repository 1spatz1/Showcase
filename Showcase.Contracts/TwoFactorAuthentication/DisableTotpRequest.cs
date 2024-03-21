namespace Showcase.Contracts.TwoFactorAuthentication;

public record DisableTotpRequest
(
    string UserId = ""
);