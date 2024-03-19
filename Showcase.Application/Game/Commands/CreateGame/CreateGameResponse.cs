namespace Showcase.Application.Game.Commands.CreateGame;

public record CreateGameResponse
(
    Guid UserId,
    string Username,
    Guid GameId
);
