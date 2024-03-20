namespace Showcase.Contracts.Authentication;

public record ConfigureTotpRequest
(
    Guid UserId,
    string Username
);