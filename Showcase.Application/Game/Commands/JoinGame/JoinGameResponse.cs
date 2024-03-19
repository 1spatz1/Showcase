namespace Showcase.Application.Game.Commands.JoinGame;

public record JoinGameResponse
(
    Guid UserId,
    string Username,
    Guid GameId
);