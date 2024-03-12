namespace Showcase.Contracts.Game;

public record CreateGameRequest
(
    Guid UserId,
    string Username,
    string Token
);