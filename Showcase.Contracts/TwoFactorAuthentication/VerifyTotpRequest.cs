namespace Showcase.Contracts.TwoFactorAuthentication;

public record VerifyTotpRequest
(
    Guid UserId,
    string Username,
    string Token
);