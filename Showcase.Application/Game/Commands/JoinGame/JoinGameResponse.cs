namespace Showcase.Application.Game.Commands.JoinGame;

public record JoinGameResponse
(
    Guid UserId,
    Guid GameId
);