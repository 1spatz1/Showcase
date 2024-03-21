namespace Showcase.Contracts.TwoFactorAuthentication;

public record EnableTotpRequest
(
    string Token,
    string UserId = ""
);