namespace Showcase.Contracts.TwoFactorAuthentication;

public record VerifyTotpRequest
(
    string Token,
    string UserId = ""
);