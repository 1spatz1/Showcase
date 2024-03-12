namespace Showcase.Application.Authentication.Common;

public record AuthenticationResponse
(
    Guid UserId,
    string Username,
    string Token,
    DateTime ValidUntil
);