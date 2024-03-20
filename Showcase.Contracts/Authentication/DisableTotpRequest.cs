namespace Showcase.Contracts.Authentication;

public record DisableTotpRequest
(
    Guid UserId,
    string Username
);