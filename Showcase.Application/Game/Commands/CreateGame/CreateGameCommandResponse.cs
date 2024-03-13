namespace Showcase.Application.Game.Commands.CreateGame;

public record CreateGameCommandResponse
(
    Guid UserId,
    string Username,
    string Token,
    Guid GameId
);
