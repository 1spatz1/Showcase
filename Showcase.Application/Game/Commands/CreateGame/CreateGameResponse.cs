namespace Showcase.Application.Game.Commands.CreateGame;

public record CreateGameResponse
(
    Guid UserId,
    string Username,
    string Token,
    Guid GameId
);
