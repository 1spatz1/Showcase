namespace Showcase.Application.Game.Commands.JoinGame;

public record JoinGameResponse
(
    Guid UserId,
    string Username,
    string Token,
    Domain.Entities.Game Game
);