namespace Showcase.Contracts.TwoFactorAuthentication;

public record EnableTotpRequest
(
    string RecaptchaToken,
    string Token,
    string UserId = ""
);